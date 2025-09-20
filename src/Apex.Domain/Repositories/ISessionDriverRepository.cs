using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface ISessionDriverRepository
{
    Task<IEnumerable<Driver>> GetDriversBySessionIdAsync(int sessionId);
    Task AddSessionDriverAsync(SessionDriver sessionDriver);
    Task<bool> SessionDriverExistsAsync(SessionDriver sessionDriver);
}
