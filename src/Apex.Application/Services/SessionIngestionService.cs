using Apex.Application.Client;
using Apex.Application.DTO;
using Apex.Application.Mappings;
using Apex.Domain.Configuration;
using Apex.Domain.Requests;
using Apex.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace Apex.Application.Services;

public class SessionIngestionService : ISessionIngestionService
{
    private readonly IOpenF1ApiClient _apiClient;
    private readonly ApexDbContext _dbContext;
    private readonly IngestionOptions _options;

    public SessionIngestionService(IOpenF1ApiClient apiClient, ApexDbContext dbContext, IOptions<IngestionOptions> options)
    {
        _apiClient = apiClient;
        _dbContext = dbContext;
        _options = options.Value;
    }

    public async Task<List<SessionDto>> IngestSessionsAsync(SessionIngestionRequest request, CancellationToken cancellationToken = default)
    {
        var sessionsDtos = await _apiClient.GetSessionsAsync(request.MeetingKey);
        var addedSessions = new List<SessionDto>();
        var meetings = await _dbContext.Meetings.ToListAsync(cancellationToken);
        
        foreach (var sessionDto in sessionsDtos)
        {
            var exists = await _dbContext.Sessions.AnyAsync(x => x.Key == sessionDto.Meeting_Key, cancellationToken);

            if (exists)
            {
                Log.Information("Session with key {SessionKey} already exists", sessionDto.Meeting_Key);
                continue;
            }
            
            var session = sessionDto.ToEntity();

            session.MeetingId = meetings
                .Where(m => m.Key == sessionDto.Meeting_Key)
                .Select(m => m.Id)
                .FirstOrDefault();
            
            await _dbContext.Sessions.AddAsync(session, cancellationToken);

            addedSessions.Add(sessionDto);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await Task.Delay(_options.ApiDelayMs, cancellationToken);
        return addedSessions;
    }
}