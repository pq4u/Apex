using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly ApexDbContext _dbContext;

    public DriverRepository(ApexDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Driver>> GetDriversBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default)
        => await _dbContext.SessionDrivers
                .Where(sd => sd.SessionId == sessionId)
                .Include(d => d.Team)
                .Select(sd => sd.Driver)
                .ToListAsync(cancellationToken);
}
