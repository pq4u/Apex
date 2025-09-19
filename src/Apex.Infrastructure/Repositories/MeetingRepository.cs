using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

public class MeetingRepository : IMeetingRepository
{
    private readonly ApexDbContext _dbContext;

    public MeetingRepository(ApexDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<List<Meeting>?> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Meetings.ToListAsync(cancellationToken);
}