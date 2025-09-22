using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

public class SessionDriverRepository : ISessionDriverRepository
{
    private readonly ApexDbContext _dbContext;
    private readonly DbSet<SessionDriver> _sessionDrivers;

    public SessionDriverRepository(ApexDbContext dbContext)
    {
        _dbContext = dbContext;
        _sessionDrivers = _dbContext.SessionDrivers;
    }

    public async Task<IEnumerable<Driver>?> GetDriversBySessionIdAsync(int sessionId)
        => await _sessionDrivers
                .Where(sd => sd.SessionId == sessionId)
                .Include(d => d.Team)
                .Select(sd => sd.Driver)
                .ToListAsync();

    public async Task AddAsync(SessionDriver sessionDriver)
    {
        await _sessionDrivers.AddAsync(sessionDriver);
    }
    
    public async Task<bool> ExistsAsync(int sessionId, int driverId)
        => await _dbContext.SessionDrivers
            .AnyAsync(sd => sd.SessionId == sessionId 
                            && sd.DriverId == driverId);
}
