using Apex.Domain.Entities;
namespace Apex.Domain.Repositories;

public interface ILapRepository
{
    Task<IEnumerable<Lap>?> GetDriverLapsInSessionAsync(int sessionId, int driverId);
    Task<int> GetDriverLapsInSessionCountAsync(int sessionId, int driverId);
    Task AddDriverLapsAsync(List<Lap> laps);
}
