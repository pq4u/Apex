using Apex.Domain.Requests;
using Apex.Domain.Results;

namespace Apex.Application.Services;

public interface ILapService
{
    Task<LapIngestionResult> IngestLapsAsync(LapIngestionRequest request, CancellationToken cancellationToken = default);
}
