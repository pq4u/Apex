using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly ApexDbContext _dbContext;

    public SessionRepository(ApexDbContext dbContext) => _dbContext = dbContext;

    public async Task<List<Session>?> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Sessions.ToListAsync(cancellationToken);

    public async Task<Session> CreateAsync(Session session, CancellationToken cancellationToken = default)
    {
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return session;
    }

    public async Task<int> CreateSessionDriverAsync(int sessionId, int driverId, int teamId,
        CancellationToken cancellationToken = default)
    {
        var sessionDriver = new SessionDriver
        {
            SessionId = sessionId,
            DriverId = driverId,
            TeamId = teamId
        };
        
        _dbContext.SessionDrivers.Add(sessionDriver);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return sessionDriver.DriverId;
    }

    public async Task<bool> SessionDriverExistsAsync(int sessionId, int driverId, CancellationToken cancellationToken = default)
        => await _dbContext.SessionDrivers.AnyAsync(sd => sd.SessionId == sessionId && sd.DriverId == driverId, cancellationToken);
}