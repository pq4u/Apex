using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

internal class LapRepository : ILapRepository
{
    private readonly ApexDbContext _dbContext;
    private readonly DbSet<Lap> _laps;

    public LapRepository(ApexDbContext dbContext)
    {
        _dbContext = dbContext;
        _laps = _dbContext.Laps;
    }

    public async Task<IEnumerable<Lap>?> GetDriverLapsInSessionAsync(int sessionId, int driverId)
        => await _laps
            .Where(l => l.SessionId == sessionId && l.DriverId == driverId)
            .ToListAsync();

    public async Task<int> GetDriverLapsInSessionCountAsync(int sessionId, int driverId)
        => await _laps
            .Where(l => l.SessionId == sessionId && l.DriverId == driverId)
            .CountAsync();

    public async Task AddDriverLapsAsync(List<Lap> laps)
    {
        await _laps.AddRangeAsync(laps);
    }

    public async Task<IEnumerable<Lap>?> GetConsecutiveLapsAsync(int sessionId, int driverId, int lapNumber)
        => await _laps.Where(x => x.SessionId == sessionId && x.DriverId == driverId && (x.LapNumber == lapNumber || x.LapNumber == (lapNumber + 1)))
            .ToListAsync();
}
