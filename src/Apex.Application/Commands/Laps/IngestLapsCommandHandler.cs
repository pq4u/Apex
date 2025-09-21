using Apex.Application.Abstractions;
using Apex.Application.Client;
using Apex.Application.Mappings;
using Apex.Application.Services;
using Apex.Domain.Configuration;
using Apex.Domain.Repositories;
using Apex.Domain.Requests;
using Microsoft.Extensions.Options;
using Serilog;

namespace Apex.Application.Commands.Laps;

public class IngestLapsCommandHandler : ICommandHandler<IngestLapsCommand>
{
    private readonly ILapRepository _lapRepository;
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IngestionOptions _options;
    public IngestLapsCommandHandler(ILapRepository lapRepository, IOpenF1ApiClient apiClient, IOptions<IngestionOptions> options)
    {
        _lapRepository = lapRepository;
        _apiClient = apiClient;
        _options = options.Value;
    }

    public async Task HandleAsync(IngestLapsCommand command)
    {
        try
        {
            var existingDriverLaps = await _lapRepository.GetDriverLapsInSessionCountAsync(command.SessionKey, command.DriverId);

            if (existingDriverLaps > 0)
            {
                Log.Information("Laps of driver {DriverNumber} in session key {SessionKey} are already added",
                    command.DriverNumber, command.SessionKey);
                return;
            }


            var laps = await _apiClient.GetLapsAsync(command.SessionKey, command.DriverId);
            var entityLaps = laps.Select(l => l.ToEntity(command.SessionId, command.DriverId)).ToList();

            await _lapRepository.AddDriverLapsAsync(entityLaps);
            Log.Information("Added {LapsCount} laps of driver {DriverNumber} in session key {SessionKey}",
                entityLaps.Count(), command.DriverNumber, command.SessionKey);
        
            await Task.Delay(_options.ApiDelayMs);
        }
        catch (Exception ex)
        {
            Log.Information(ex, "Failed adding laps of driver {DriverNumber} in session {SessionKey}",
                command.DriverNumber, command.SessionKey);
        }
    }
}
