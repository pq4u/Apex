using Apex.Application.Client;
using Apex.Application.Commands.Meetings;
using Apex.Application.Mappings;
using Apex.Domain.Exceptions;
using Apex.Infrastructure.DAL;
using Apex.Worker.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Apex.Application.Services;

public class MeetingIngestionService : IMeetingIngestionService
{
    private readonly IOpenF1ApiClient _apiClient;
    private readonly ApexDbContext _dbContext;

    public MeetingIngestionService(IOpenF1ApiClient apiClient, ApexDbContext dbContext)
    {
        _apiClient = apiClient;
        _dbContext = dbContext;
    }

    public async Task<bool> IngestMeetingsAsync(IngestMeetingsCommand request, CancellationToken cancellationToken = default)
    {
        var existingMeetings = await _dbContext.Meetings.ToListAsync(cancellationToken);

        var meetings = await _apiClient.GetMeetingsAsync();
        if (meetings == null)
        {
            throw new MeetingsNotFoundInApiException();
        }

        var meetingsEntities = meetings.Select(m => m.ToEntity()).ToList();

        foreach (var meeting in meetingsEntities)
        {
            var meetingExists = existingMeetings.Any(m => m.Key == meeting.Key);
            if (meetingExists)
            {
                Log.Information("Meeting already exists in database {MeetingKey}", meeting.Key);
                continue;
            }
            
            await _dbContext.Meetings.AddAsync(meeting, cancellationToken);
            Log.Information("Created meeting {MeetingKey}", meeting.Key);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}