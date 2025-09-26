using System.Collections;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

public class StintRepository : IStintRepository
{
    private readonly ApexDbContext _dbContext;
    private readonly DbSet<Stint> _stints;

    public StintRepository(ApexDbContext dbContext)
    {
        _dbContext = dbContext;
        _stints = _dbContext.Stints;
    }

    public async Task AddRangeAsync(IEnumerable<Stint> stints)
        => await _stints.AddRangeAsync(stints);

    public async Task<IEnumerable<Stint>?> GetBySessionIdAsync(int sessionId)
        => await _stints.Where(x => x.SessionId == sessionId).ToListAsync();
}