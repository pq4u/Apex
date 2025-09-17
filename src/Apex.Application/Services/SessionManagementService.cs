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

        try
        {
            var session = await CreateNewSessionsAsync(sessionKey, cancellationToken);
            Log.Information("Created session {SessionKey}", sessionKey);
            return SessionCreationResult.Created(session);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to create session {SessionKey}", sessionKey);
            return SessionCreationResult.Failed(ex.Message);
        }
    }

    private async Task<Session> CreateNewSessionsAsync(int sessionKey, CancellationToken cancellationToken)
    {
        var sessionDto = await _apiClient.GetSessionAsync(sessionKey);
        if (sessionDto == null)
        {
            throw new SessionNotFoundInApiException(sessionKey);
        }

        var meetings = await _dbContext.Meetings.ToListAsync(cancellationToken);
        
        var session = sessionDto.ToEntity();
        session.MeetingId = meetings
            .Where(m => m.Key == sessionDto.Meeting_Key)
            .Select(m => m.Id)
            .FirstOrDefault();
        
        await _dbContext.Sessions.AddAsync(session, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return session;
    }
}