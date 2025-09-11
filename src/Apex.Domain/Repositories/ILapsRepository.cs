using Apex.Domain.Entities;
namespace Apex.Domain.Repositories;

public interface ILapRepository
{
    Task AddDriverLapsAsync(List<Lap> laps, CancellationToken cancellationToken = default);
    Task<int> GetDriverLapsInSessionCountAsync(int sessionId, int driverId, CancellationToken cancellationToken = default);
}
