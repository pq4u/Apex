using Apex.Domain.Requests;
using Apex.Domain.Results;

namespace Apex.Application.Services;

public interface ITelemetryIngestionService
{
    Task<TelemetryIngestionResult> IngestDriverTelemetryAsync(TelemetryIngestionRequest request, CancellationToken cancellationToken = default);
}