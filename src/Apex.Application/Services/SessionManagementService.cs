using Apex.Application.Client;
using Apex.Application.Mappings;
using Apex.Domain.Entities;
using Apex.Domain.Exceptions;
using Apex.Domain.Results;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Apex.Application.Services;

public class SessionManagementService : ISessionManagementService
{
    private readonly ApexDbContext _dbContext;
    private readonly IOpenF1ApiClient _apiClient;

    public SessionManagementService(ApexDbContext context, IOpenF1ApiClient apiClient)
    {
        _dbContext = context;
        _apiClient = apiClient;
    }

    public async Task<SessionCreationResult> EnsureSessionExistsAsync(int sessionKey, CancellationToken cancellationToken = default)
    {
        var existingSession = await _dbContext.Sessions
            .Include(s => s.Meeting)
            .FirstOrDefaultAsync(s => s.Key == sessionKey, cancellationToken);

        if (existingSession != null)
        {
            Log.Information("Session {SessionKey} already exists", sessionKey);
            return SessionCreationResult.AlreadyExists(existingSession);
        }

        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var session = await CreateNewSessionAsync(sessionKey, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            Log.Information("Created session {SessionKey}", sessionKey);
            return SessionCreationResult.Created(session);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            Log.Error(ex, "Failed to create session {SessionKey}", sessionKey);
            return SessionCreationResult.Failed(ex.Message);
        }
    }

    private async Task<Session> CreateNewSessionAsync(int sessionKey, CancellationToken cancellationToken)
    {
        var sessionDto = await _apiClient.GetSessionAsync(sessionKey);
        if (sessionDto == null)
        {
            throw new SessionNotFoundInApiException(sessionKey);
        }

        var meeting = await EnsureMeetingExistsAsync(sessionDto.Meeting_Key, cancellationToken);
        
        var session = sessionDto.ToEntity();
        session.MeetingId = meeting.Id;
        
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return session;
    }

    private async Task<Meeting> EnsureMeetingExistsAsync(int meetingKey, CancellationToken cancellationToken)
    {
        var existingMeeting = await _dbContext.Meetings
            .FirstOrDefaultAsync(m => m.Key == meetingKey, cancellationToken);

        if (existingMeeting != null)
            return existingMeeting;

        var meetingDto = await _apiClient.GetMeetingAsync(meetingKey);
        if (meetingDto == null)
        {
            throw new MeetingNotFoundInApiException(meetingKey);
        }

        var meeting = meetingDto.ToEntity();
        _dbContext.Meetings.Add(meeting);
        await _dbContext.SaveChangesAsync(cancellationToken);

        Log.Information("Created meeting {MeetingKey}", meetingKey);
        return meeting;
    }
}