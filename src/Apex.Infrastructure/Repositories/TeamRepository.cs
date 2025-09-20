using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApexDbContext _dbContext;
    private readonly DbSet<Team> _teams;

    public TeamRepository(ApexDbContext dbContext)
    {
        _dbContext = dbContext;
        _teams = _dbContext.Teams;
    }

    public async Task<Team?> GetByIdAsync(int id)
        => await _teams.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Team?> GetByNameAsync(string name)
        => await _teams.FirstOrDefaultAsync(x => x.Name == name);

    public async Task<IEnumerable<Team>?> GetAllAsync()
        => await _teams.ToListAsync();

    public async Task AddAsync(Team team)
    {
        await _teams.AddAsync(team);
    }
}