using Apex.Application.Client;
using Apex.Application.DTO;
using Apex.Application.Mappings;
using Apex.Domain.Entities;
using Apex.Domain.Results;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Apex.Application.Services;

public class DriverAssociationService : IDriverAssociationService
{
    private readonly ApexDbContext _dbContext;
    private readonly IOpenF1ApiClient _apiClient;

    public DriverAssociationService(ApexDbContext context, IOpenF1ApiClient apiClient)
    {
        _dbContext = context;
        _apiClient = apiClient;
    }

    public async Task<DriverAssociationResult> AssociateDriversWithSessionAsync(int sessionKey, int sessionId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var driverMapping = await ProcessDriverAssociations(sessionKey, sessionId, cancellationToken);
            
            Log.Information("Successfully associated {Count} drivers with session {SessionKey}", 
                driverMapping.Count, sessionKey);
            
            return DriverAssociationResult.Success(driverMapping);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to associate drivers with session {SessionKey}", sessionKey);
            return DriverAssociationResult.Failed(ex.Message);
        }
    }

    private async Task<Dictionary<int, int>> ProcessDriverAssociations(int sessionKey, int sessionId,
        CancellationToken cancellationToken)
    {
        var driverDtos = await _apiClient.GetDriversAsync(sessionKey);
        Log.Information("Found {Count} drivers for session {SessionKey}", driverDtos.Count, sessionKey);

        var driverNumberToId = new Dictionary<int, int>();
        var teamCache = new Dictionary<string, int>();

        foreach (var driverDto in driverDtos)
        {
            var team = await EnsureTeamExistsAsync(driverDto, teamCache, cancellationToken);
            var driver = await EnsureDriverExistsAsync(driverDto, cancellationToken);
            
            driverNumberToId[driver.DriverNumber] = driver.Id;

            await EnsureSessionDriverAssociationAsync(sessionId, driver.Id, team!.Id, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return driverNumberToId;
    }

    private async Task<Team?> EnsureTeamExistsAsync(DriverDto driverDto, Dictionary<string, int> teamCache,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(driverDto.Team_Name))
            return null;

        var teamName = driverDto.Team_Name;

        if (teamCache.TryGetValue(teamName, out var cachedTeamId))
        {
            return await _dbContext.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == cachedTeamId, cancellationToken);
        }

        var existingTeam = await _dbContext.Teams
            .FirstOrDefaultAsync(t => t.Name == teamName, cancellationToken);

        if (existingTeam != null)
        {
            teamCache[teamName] = existingTeam.Id;
            return existingTeam;
        }

        var newTeam = driverDto.ExtractTeam();
        _dbContext.Teams.Add(newTeam);
        await _dbContext.SaveChangesAsync(cancellationToken);

        teamCache[teamName] = newTeam.Id;
        Log.Information("Created team {TeamName}", newTeam.Name);
        
        return newTeam;
    }

    private async Task<Driver> EnsureDriverExistsAsync(DriverDto driverDto, CancellationToken cancellationToken)
    {
        var existingDriver = await _dbContext.Drivers
            .FirstOrDefaultAsync(d => d.DriverNumber == driverDto.Driver_Number, cancellationToken);

        if (existingDriver != null)
            return existingDriver;

        var newDriver = driverDto.ToEntity();
        _dbContext.Drivers.Add(newDriver);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Log.Information("Created driver {DriverNumber} - {DriverName}", newDriver.DriverNumber, newDriver.FullName);
        
        return newDriver;
    }

    private async Task EnsureSessionDriverAssociationAsync(int sessionId, int driverId, int teamId,
        CancellationToken cancellationToken)
    {
        var exists = await _dbContext.SessionDrivers
            .AnyAsync(sd => sd.SessionId == sessionId && sd.DriverId == driverId, cancellationToken);

        if (exists) return;

        var sessionDriver = new SessionDriver
        {
            SessionId = sessionId,
            DriverId = driverId,
            TeamId = teamId
        };

        _dbContext.SessionDrivers.Add(sessionDriver);
        Log.Debug("Created session-driver association for driver {DriverId} in session {SessionId}", driverId, sessionId);
    }
}