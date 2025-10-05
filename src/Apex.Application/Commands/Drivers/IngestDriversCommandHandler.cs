using Apex.Application.Abstractions;
using Apex.Application.Client;
using Apex.Application.Mappings;
using Apex.Domain.Configuration;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Microsoft.Extensions.Options;
using Serilog;

namespace Apex.Application.Commands.Drivers;

public class IngestDriversCommandHandler : ICommandHandler<IngestDriversCommand>
{
    private readonly IDriverRepository _driverRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IngestionOptions _options;

    public IngestDriversCommandHandler(IDriverRepository driverRepository,
        ITeamRepository teamRepository, IOpenF1ApiClient apiClient, IOptions<IngestionOptions> options)
    {
        _driverRepository = driverRepository;
        _teamRepository = teamRepository;
        _apiClient = apiClient;
        _options = options.Value;
    }

    public async Task HandleAsync(IngestDriversCommand command)
    {
        try
        {
            var driverDtos = await _apiClient.GetDriversAsync(command.SessionKey);
            
            Log.Information("Found {Count} drivers for session {SessionKey}", driverDtos!.Count, command.SessionKey);

            var dbTeams = (await _teamRepository.GetAllAsync())?.ToList();
            var dbDrivers = (await _driverRepository.GetAllAsync())?.ToList();

            foreach (var driver in driverDtos)
            {
                // Add driver if not exists
                var existingDriver = dbDrivers?.FirstOrDefault(x => x.DriverNumber == driver.Driver_Number);
                if (existingDriver == null)
                {
                    var newDriver = driver.ToEntity();
                    await _driverRepository.AddAsync(newDriver);
                    Log.Information("Added driver {FullName} - {DriverNumber} to database",
                        newDriver.FullName, newDriver.DriverNumber);
                    dbDrivers?.Add(newDriver);
                }

                // Add team if not exists
                var existingTeam = dbTeams?.FirstOrDefault(x => x.Name == driver.Team_Name);
                if (existingTeam == null)
                {
                    var newTeam = driver.ExtractTeam();
                    await _teamRepository.AddAsync(newTeam);
                    Log.Information("Added team {TeamName} to database", newTeam.Name);
                    dbTeams?.Add(newTeam);
                }
            }
            
            Log.Information("Successfully ingested {Count} drivers and teams for session {SessionKey}", driverDtos.Count, command.SessionKey);
            
            await Task.Delay(_options.ApiDelayMs);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to ingest drivers and teams for session {SessionKey}", command.SessionKey);
            throw;
        }
    }
}