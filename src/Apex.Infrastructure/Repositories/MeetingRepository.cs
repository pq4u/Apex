using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace Apex.Infrastructure.Repositories;

public class MeetingRepository : IMeetingRepository
{
    private readonly ApexDbContext _dbContext;
    private readonly DbSet<Meeting> _meetings;

    public MeetingRepository(ApexDbContext dbContext)
    {
        _dbContext = dbContext;
        _meetings = _dbContext.Meetings;
    }

    public async Task<IEnumerable<Meeting>?> GetAllAsync()
        => await _meetings.ToListAsync();

    public async Task AddAsync(Meeting meeting)
    {
        await _meetings.AddAsync(meeting);
    }
}