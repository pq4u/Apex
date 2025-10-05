using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.IntegrationTests;

internal class TestMeetingRepository : IMeetingRepository
{
    private readonly List<Meeting> _meetings = new();
    public async Task<IEnumerable<Meeting>?> GetAllAsync()
    {
        await Task.CompletedTask;
        return _meetings.ToList();
    }

    public async Task AddAsync(Meeting meeting)
    {
        _meetings.Add(meeting);
        await Task.CompletedTask;
    }
}