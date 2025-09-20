using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface ISessionRepository
{
    Task<IEnumerable<Session>?> GetAllAsync();
    Task AddAsync(Session session);
}
