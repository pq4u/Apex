using Apex.Application.Client;
using Apex.Application.DTO;
using Apex.Application.Mappings;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Domain.Results;
using Serilog;

namespace Apex.Application.Services;

public class DriverAssociationService : IDriverAssociationService
{
    private readonly ISessionDriverRepository _sessionDriverRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenF1ApiClient _apiClient;

    public DriverAssociationService(ISessionDriverRepository sessionDriverRepository, IDriverRepository driverRepository, IOpenF1ApiClient apiClient)
    {
        _sessionDriverRepository = sessionDriverRepository;
        _driverRepository = driverRepository;
        _apiClient = apiClient;
    }

    public async Task AssociateDriversWithSessionAsync(int sessionKey, int sessionId)
    {
        try
        {
            var driverMapping = await ProcessDriverAssociations(sessionKey, sessionId);

            Log.Information("Successfully associated {Count} drivers with session {SessionKey}",
                driverMapping.Count, sessionKey);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to associate drivers with session {SessionKey}", sessionKey);
        }
    }

    private async Task<Dictionary<int, int>> ProcessDriverAssociations(int sessionKey, int sessionId)
    {
        var driverDtos = await _apiClient.GetDriversAsync(sessionKey);
        Log.Information("Found {Count} drivers for session {SessionKey}", driverDtos.Count, sessionKey);

        var driverNumberToId = new Dictionary<int, int>();
        var teamCache = new Dictionary<string, int>();

        
        
        foreach (var driverDto in driverDtos)
        {
            var team = await EnsureTeamExistsAsync(driverDto, teamCache);
            var driver = await EnsureDriverExistsAsync(driverDto);
            
            driverNumberToId[driver.DriverNumber] = driver.Id;

            await EnsureSessionDriverAssociationAsync(sessionId, driver.Id, team!.Id);
        }

        return driverNumberToId;
    }

    private async Task<Team?> EnsureTeamExistsAsync(DriverDto driverDto, Dictionary<string, int> teamCache)
    {
        if (string.IsNullOrEmpty(driverDto.Team_Name))
            return null;

        var teamName = driverDto.Team_Name;

        if (teamCache.TryGetValue(teamName, out var cachedTeamId))
        {
            return await _teamRepository.GetByIdAsync(cachedTeamId);
        }

        var existingTeam = await _teamRepository.GetByNameAsync(teamName);

        if (existingTeam != null)
        {
            teamCache[teamName] = existingTeam.Id;
            return existingTeam;
        }

        var newTeam = driverDto.ExtractTeam();
        await _teamRepository.AddAsync(newTeam);

        teamCache[teamName] = newTeam.Id;
        Log.Information("Created team {TeamName}", newTeam.Name);
        
        return newTeam;
    }

    private async Task<Driver> EnsureDriverExistsAsync(DriverDto driverDto)
    {
        var existingDriver = await _driverRepository.GetByNumberAsync(driverDto.Driver_Number);

        if (existingDriver != null)
            return existingDriver;

        var newDriver = driverDto.ToEntity();
        
        await _driverRepository.AddAsync(newDriver);

        Log.Information("Created driver {DriverNumber} - {DriverName}", newDriver.DriverNumber, newDriver.FullName);
        
        return newDriver;
    }

    private async Task EnsureSessionDriverAssociationAsync(int sessionId, int driverId, int teamId)
    {
        var exists = await _sessionDriverRepository.ExistsAsync(sessionId, driverId);
        
            // _dbContext.SessionDrivers
            // .AnyAsync(sd => sd.SessionId == sessionId && sd.DriverId == driverId);

        if (exists) return;

        var sessionDriver = new SessionDriver
        {
            SessionId = sessionId,
            DriverId = driverId,
            TeamId = teamId
        };

        _sessionDriverRepository.AddAsync(sessionDriver);
        Log.Debug("Created session-driver association for driver {DriverId} in session {SessionId}", driverId, sessionId);
    }
}