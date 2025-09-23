using Apex.Domain.TimeSeries;

namespace Apex.Domain.Repositories;

public interface ITelemetryRepository
{
    Task BulkInsertCarDataAsync(List<TelemetryData> carDataList, CancellationToken cancellationToken);
    Task<long> GetCarDataCountAsync(int sessionId, int driverId);
    Task<IEnumerable<TelemetryData>> GetCarDataAsync(int sessionId, int driverId);
}
