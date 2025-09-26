using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface IStintRepository
{
    Task AddRangeAsync(IEnumerable<Stint> stint);
    Task<IEnumerable<Stint>?> GetBySessionIdAsync(int sessionId);
}