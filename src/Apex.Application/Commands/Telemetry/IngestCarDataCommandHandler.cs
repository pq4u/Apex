using Apex.Application.Abstractions;
using Apex.Application.Services;
using Apex.Domain.Requests;
using Serilog;

namespace Apex.Application.Commands.Telemetry;

public class IngestCarDataCommandHandler : ICommandHandler<IngestCarDataCommand>
{
    private readonly ITelemetryIngestionService _ingestionService;

    public IngestCarDataCommandHandler(ITelemetryIngestionService ingestionService)
    {
        _ingestionService = ingestionService;
    }

    public async Task HandleAsync(IngestCarDataCommand command)
    {
        var request = new TelemetryIngestionRequest(command.SessionKey, command.SessionId, command.DriverNumber, command.DriverId, command.StartDate);

        var result = await _ingestionService.IngestDriverTelemetryAsync(request);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Telemetry ingestion failed: {result.ErrorMessage}");
        }

        Log.Information("Successfully completed telemetry ingestion for driver {DriverNumber}. Records processed: {Records}; Duration: {Duration}",
            command.DriverNumber, result.TotalRecordsProcessed, result.ProcessingDuration);
    }
}