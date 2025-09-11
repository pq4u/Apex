using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface ISessionRepository
{
    Task<Session?> GetByKeyAsync(int sessionKey, CancellationToken cancellationToken = default);
    Task<Session> CreateAsync(Session session, CancellationToken cancellationToken = default);
    Task<int> CreateSessionDriverAsync(int sessionId, int driverId, int teamId, CancellationToken cancellationToken = default);
    Task<bool> SessionDriverExistsAsync(int sessionId, int driverId, CancellationToken cancellationToken = default);
}
