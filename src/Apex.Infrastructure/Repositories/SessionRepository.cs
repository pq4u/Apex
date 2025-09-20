using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly ApexDbContext _dbContext;
    private readonly DbSet<Session> _sessions;

    public SessionRepository(ApexDbContext dbContext)
    {
        _dbContext = dbContext;
        _sessions = _dbContext.Sessions;
    }

    public async Task<IEnumerable<Session>?> GetAllAsync()
        => await _sessions.ToListAsync();

    public async Task AddAsync(Session session)
    {
        await _sessions.AddAsync(session);
    }

    public async Task<bool> ExistsByKeyAsync(int key)
        => await _sessions.AnyAsync(x => x.Key == key);
}