using Apex.Application.Client;
using Apex.Application.DTO;
using Apex.Application.Mappings;
using Apex.Domain.Configuration;
using Apex.Domain.Repositories;
using Apex.Domain.Requests;
using Apex.Domain.Results;
using Apex.Domain.States;
using Apex.Domain.TimeSeries;
using Microsoft.Extensions.Options;
using Serilog;

namespace Apex.Application.Services;

public class TelemetryIngestionService : ITelemetryIngestionService
{
    private readonly ITelemetryRepository _telemetryRepository;
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IngestionOptions _options;

    public TelemetryIngestionService(ITelemetryRepository telemetryRepository,
        IOpenF1ApiClient apiClient, IOptions<IngestionOptions> options)
    {
        _telemetryRepository = telemetryRepository;
        _apiClient = apiClient;
        _options = options.Value;
    }

    public async Task<TelemetryIngestionResult> IngestDriverTelemetryAsync(
        TelemetryIngestionRequest request, CancellationToken cancellationToken = default)
    {
        var state = new TelemetryIngestionState();
        
        try
        {
            var existingCount = await _telemetryRepository.GetCarDataCountAsync(request.SessionId, request.DriverId);
            if (existingCount > 0)
            {
                Log.Information("Skipping driver {DriverNumber}, already have {Count} records", 
                    request.DriverNumber, existingCount);
                return TelemetryIngestionResult.Success(0, TimeSpan.Zero);
            }

            await IngestTelemetryBatches(request, state, cancellationToken);
            
            Log.Information("Completed telemetry ingestion for driver {DriverNumber} with {Total} total records", 
                request.DriverNumber, state.TotalRecordsProcessed);
                
            return state.ToResult();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Critical error during telemetry ingestion for driver {DriverNumber}", request.DriverNumber);
            return state.ToFailedResult(ex.Message);
        }
    }

    private async Task IngestTelemetryBatches(TelemetryIngestionRequest request, TelemetryIngestionState state, CancellationToken cancellationToken)
    {
        var interval = TimeSpan.FromMinutes(_options.BatchIntervalMinutes);
        var startDate = request.StartDate;
        var endDate = startDate + interval;

        var carDataBatch = new List<Telemetry>();

        try
        {
            while (!ShouldStopIngestion(state) && !cancellationToken.IsCancellationRequested)
            {
                var processedSuccessfully = false;
                try
                {
                    await Task.Delay(_options.ApiDelayMs, cancellationToken);
                    var carDataDtos = await _apiClient.GetCarDataBatchAsync(
                        request.SessionKey, request.DriverNumber, startDate, endDate);

                    if (carDataDtos?.Any() == true)
                    {
                        ProcessDataBatch(carDataDtos, request, carDataBatch, state);

                        if (ShouldFlushBatch(carDataBatch))
                        {
                            await FlushBatch(carDataBatch, request.DriverNumber, cancellationToken);
                        }
                        processedSuccessfully = true;
                    }
                    else
                    {
                        state.RecordEmptyResponse();
                        processedSuccessfully = true;
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (HttpRequestException httpEx)
                {
                    LogAndRecordApiError(httpEx, request.DriverNumber, startDate, endDate, state);
                    state.RecordApiError();
                }
                catch (Exception ex)
                {
                    LogAndRecordApiError(ex, request.DriverNumber, startDate, endDate, state);
                    state.RecordUnexpectedError();
                }

                if (processedSuccessfully)
                {
                    startDate = endDate;
                    endDate = startDate + interval;
                }
            }
        }
        finally
        {
            if (carDataBatch.Count > 0)
            {
                try
                {
                    await FlushBatch(carDataBatch, request.DriverNumber, cancellationToken);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to flush final batch for driver {DriverNumber}", request.DriverNumber);
                    throw;
                }
            }
        }
    }

    private void ProcessDataBatch(IEnumerable<CarDataDto> carDataDtos, TelemetryIngestionRequest request,
        List<Telemetry> carDataBatch, TelemetryIngestionState state)
    {
        var batchCount = 0;
        foreach (var dto in carDataDtos)
        {
            carDataBatch.Add(dto.ToTimeSeries(request.SessionId, request.DriverId));
            batchCount++;
        }
        
        state.RecordDataBatch(batchCount);
    }

    private bool ShouldStopIngestion(TelemetryIngestionState state)
        => state.ConsecutiveEmptyBatches >= _options.MaxEmptyBatches && state.HasData;

    private bool ShouldFlushBatch(List<Telemetry> carDataBatch)
        => carDataBatch.Count >= _options.BatchSize;

    private async Task FlushBatch(List<Telemetry> carDataBatch, int driverNumber, CancellationToken cancellationToken)
    {
        var count = carDataBatch.Count;
        await _telemetryRepository.BulkInsertCarDataAsync(carDataBatch, cancellationToken);
        Log.Information("Flushed {Count} telemetry records for driver {DriverNumber}", count, driverNumber);
        carDataBatch.Clear();
    }

    private static void LogAndRecordApiError(Exception ex, int driverNumber,
        DateTime startDate, DateTime endDate, TelemetryIngestionState state)
    {
        var errorMessage = $"API error for driver {driverNumber} at {startDate:yyyy-MM-dd HH:mm}-{endDate:yyyy-MM-dd HH:mm}";
        Log.Warning(ex, "{ErrorMessage}. Continuing...", errorMessage);
        state.ProcessingNotes.Add(errorMessage);
    }
}