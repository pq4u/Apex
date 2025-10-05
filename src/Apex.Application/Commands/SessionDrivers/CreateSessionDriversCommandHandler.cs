using Apex.Application.Abstractions;
using Apex.Application.Client;
using Apex.Domain.Configuration;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Microsoft.Extensions.Options;
using Serilog;

namespace Apex.Application.Commands.SessionDrivers;

public class CreateSessionDriversCommandHandler : ICommandHandler<CreateSessionDriversCommand>
{
    private readonly ISessionDriverRepository _sessionDriverRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IngestionOptions _options;

    public CreateSessionDriversCommandHandler(ISessionDriverRepository sessionDriverRepository,
        IDriverRepository driverRepository, ITeamRepository teamRepository,
        IOpenF1ApiClient apiClient, IOptions<IngestionOptions> options)
    {
        _sessionDriverRepository = sessionDriverRepository;
        _driverRepository = driverRepository;
        _teamRepository = teamRepository;
        _apiClient = apiClient;
        _options = options.Value;
    }

    public async Task HandleAsync(CreateSessionDriversCommand command)
    {
        try
        {
            var driverDtos = await _apiClient.GetDriversAsync(command.SessionKey);

            Log.Information("Creating session-driver associations for {Count} drivers in session {SessionId}",
                driverDtos?.Count, command.SessionId);

            var dbTeams = await _teamRepository.GetAllAsync();
            var dbDrivers = await _driverRepository.GetAllAsync();

            foreach (var driver in driverDtos!)
            {
                var currentDriver = dbDrivers.First(x => x.DriverNumber == driver.Driver_Number);
                var currentTeam = dbTeams.First(x => x.Name == driver.Team_Name);

                var sessionDriver = new SessionDriver()
                {
                    DriverId = currentDriver.Id,
                    TeamId = currentTeam.Id,
                    SessionId = command.SessionId
                };

                var exists = await _sessionDriverRepository.ExistsAsync(command.SessionId, sessionDriver.DriverId);
                if (exists) return;
                
                await _sessionDriverRepository.AddAsync(sessionDriver);
                Log.Information("Associated driver {DriverNumber} with session {SessionId}", currentDriver.DriverNumber, command.SessionId);
            }

            Log.Information("Successfully created session-driver associations for session {SessionId}",
                command.SessionId);

            await Task.Delay(_options.ApiDelayMs);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to create session-driver associations for session {SessionId}",
                command.SessionId);
            throw;
        }
    }
}