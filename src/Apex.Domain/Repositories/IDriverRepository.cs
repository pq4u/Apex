using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface IDriverRepository
{
    Task<IEnumerable<Driver>> GetDriversBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default);
}
