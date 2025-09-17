using Apex.Application.Client;
using Apex.Application.Mappings;
using Apex.Domain.Configuration;
using Apex.Domain.Repositories;
using Apex.Domain.Requests;
using Apex.Domain.Results;
using Microsoft.Extensions.Options;
using Serilog;

namespace Apex.Application.Services;

public class LapService : ILapService
{
    private readonly ILapRepository _lapRepository;
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IngestionOptions _options;
    public LapService(ILapRepository lapRepository, IOpenF1ApiClient apiClient, IOptions<IngestionOptions> options)
    {
        _lapRepository = lapRepository;
        _apiClient = apiClient;
        _options = options.Value;
    }

    public async Task<LapIngestionResult> IngestLapsAsync(LapIngestionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingDriverLaps = await _lapRepository.GetDriverLapsInSessionCountAsync(request.SessionKey, request.DriverId, cancellationToken);

            if (existingDriverLaps > 0)
            {
                Log.Information("Laps of driver {DriverNumber} in session key {SessionKey} are already added",
                    request.DriverNumber, request.SessionKey);
                return LapIngestionResult.Success(request.DriverNumber);
            }


            var laps = await _apiClient.GetLapsAsync(request.SessionKey, request.DriverId);
            
            // if no laps......
            var entityLaps = laps.Select(l => l.ToEntity(request.SessionId, request.DriverId)).ToList();

            await _lapRepository.AddDriverLapsAsync(entityLaps, cancellationToken);
            Log.Information("Added {LapsCount} laps of driver {DriverNumber} in session key {SessionKey}",
                entityLaps.Count(), request.DriverNumber, request.SessionKey);
            
            await Task.Delay(_options.ApiDelayMs, cancellationToken);
            
            return LapIngestionResult.Success(request.DriverNumber);

        }
        catch (Exception ex)
        {
            Log.Information(ex, "Failed adding laps of driver {DriverNumber} in session {SessionKey}", request.DriverNumber, request.SessionKey);
            return LapIngestionResult.Failed(request.DriverNumber, ex.Message);
        }
    }
}
