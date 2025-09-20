using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface IMeetingRepository
{
    Task<IEnumerable<Meeting>?> GetAllAsync();
}