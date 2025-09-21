using Apex.Application.Client;
using Apex.Application.Mappings;
using Apex.Domain.Configuration;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Microsoft.Extensions.Options;
using Serilog;

namespace Apex.Application.Services;

public class DriverAssociationService : IDriverAssociationService
{
    private readonly ISessionDriverRepository _sessionDriverRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IngestionOptions _options;

    public DriverAssociationService(ISessionDriverRepository sessionDriverRepository, IDriverRepository driverRepository,
        ITeamRepository teamRepository, IOpenF1ApiClient apiClient, IOptions<IngestionOptions> options)
    {
        _sessionDriverRepository = sessionDriverRepository;
        _driverRepository = driverRepository;
        _teamRepository = teamRepository;
        _apiClient = apiClient;
        _options = options.Value;
    }

    public async Task AssociateDriversWithSessionAsync(int sessionId, int sessionKey)
    {
        try
        {
            await ProcessDriverAssociations(sessionId, sessionKey);

            Log.Information("Successfully associated {Count} drivers with session {SessionKey}", sessionKey);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to associate drivers with session {SessionKey}", sessionKey);
        }
    }

    private async Task ProcessDriverAssociations(int sessionId, int sessionKey)
    {
        await Task.Delay(_options.ApiDelayMs);
        var driverDtos = await _apiClient.GetDriversAsync(sessionKey);
        
        Log.Information("Found {Count} drivers for session {SessionKey}", driverDtos.Count, sessionKey);

        var dbTeams = await _teamRepository.GetAllAsync();
        var dbDrivers = await _driverRepository.GetAllAsync();

        foreach (var driver in driverDtos)
        {
            var driverExistsInDb = dbDrivers.Any(x => x.DriverNumber == driver.Driver_Number);
            if (!driverExistsInDb)
            {
                var newDriver = driver.ToEntity();
                await _driverRepository.AddAsync(newDriver);
                Log.Information("Added driver {FullName} - {DriverNumber} to database}",
                    newDriver.FullName, newDriver.DriverNumber);
                
                dbDrivers = await _driverRepository.GetAllAsync();
            }
            
            var teamExistsInDb = dbTeams.Any(x => x.Name == driver.Team_Name);
            
            if (!teamExistsInDb)
            {
                var newTeam = driver.ExtractTeam();
                await _teamRepository.AddAsync(newTeam);
                Log.Information("Added team {TeamName} to database", newTeam.Name);
                
                dbTeams = await _teamRepository.GetAllAsync();
            }

            var sessionDriver = new SessionDriver()
            {
                DriverId = dbDrivers.First(x => x.DriverNumber == driver.Driver_Number).Id,
                TeamId = dbTeams.First(x => x.Name == driver.Team_Name).Id,
                SessionId = sessionId
            };

            var exists = await _sessionDriverRepository.ExistsAsync(sessionId, sessionDriver.DriverId);
            
            if (exists) return;
            
            await _sessionDriverRepository.AddAsync(sessionDriver);
        }
    }

}