using Apex.Domain.TimeSeries;

namespace Apex.Domain.Repositories;

public interface ITelemetryRepository
{
    Task BulkInsertCarDataAsync(List<Telemetry> carDataList, CancellationToken cancellationToken);
    Task<long> GetCarDataCountAsync(int sessionId, int driverId);
    Task<IEnumerable<Telemetry>?> GetCarDataAsync(int sessionId, int driverId, DateTime? dateFrom, DateTime? dateTo);
}
