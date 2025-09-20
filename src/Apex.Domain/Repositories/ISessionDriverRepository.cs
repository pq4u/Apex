using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface ISessionDriverRepository
{
    Task<IEnumerable<Driver>> GetDriversBySessionIdAsync(int sessionId);
    Task AddAsync(SessionDriver sessionDriver);
    Task<bool> ExistsAsync(int sessionId, int driverId);
}
