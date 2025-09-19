using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface IMeetingRepository
{
    Task<List<Meeting>?> GetAllAsync(CancellationToken cancellationToken = default);
}