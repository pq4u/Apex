using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

internal class LapRepository : ILapRepository
{
    private readonly ApexDbContext _dbContext;

    public LapRepository(ApexDbContext dbContext) => _dbContext = dbContext;

    public async Task AddDriverLapsAsync(List<Lap> laps, CancellationToken cancellationToken = default)
        => await _dbContext.Laps.AddRangeAsync(laps, cancellationToken);

    public async Task<int> GetDriverLapsInSessionCountAsync(int sessionId, int driverId, CancellationToken cancellationToken = default)
        => await _dbContext.Laps.Where(l => l.SessionId == sessionId && l.DriverId == driverId).CountAsync();
}
