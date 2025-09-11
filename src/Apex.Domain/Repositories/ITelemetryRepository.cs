using Apex.Domain.TimeSeries;

namespace Apex.Domain.Repositories;

public interface ITelemetryRepository
{
    Task BulkInsertCarDataAsync(List<CarData> carDataList, CancellationToken cancellationToken);
    Task<long> GetCarDataCountAsync(int sessionId, int driverId);
    Task<IEnumerable<CarData>> GetCarDataAsync(int sessionId, int driverId, DateTime? start, DateTime? end);
}
