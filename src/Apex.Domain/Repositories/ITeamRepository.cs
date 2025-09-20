using Apex.Domain.Entities;

namespace Apex.Domain.Repositories;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(int id);
    Task<Team?> GetByNameAsync(string id);
    Task<IEnumerable<Team>?> GetAllAsync();
    Task AddAsync(Team team);
}