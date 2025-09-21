using Apex.Application.Abstractions;
using Apex.Application.Client;
using Apex.Application.DTO;
using Apex.Application.Mappings;
using Apex.Application.Services;
using Apex.Domain.Configuration;
using Apex.Domain.Repositories;
using Apex.Domain.Requests;
using Microsoft.Extensions.Options;
using Serilog;

namespace Apex.Application.Commands.Sessions;

public class IngestSessionsCommandHandler : ICommandHandler<IngestSessionsCommand>
{
    private readonly IOpenF1ApiClient _apiClient;
    private readonly ISessionRepository _sessionRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IngestionOptions _options;

    public IngestSessionsCommandHandler(ISessionRepository sessionRepository, IMeetingRepository meetingRepository,
        IOpenF1ApiClient apiClient, IOptions<IngestionOptions> options)
    {
        _sessionRepository = sessionRepository;
        _meetingRepository = meetingRepository;
        _apiClient = apiClient;
        _options = options.Value;
    }

    public async Task HandleAsync(IngestSessionsCommand command)
    {
        var sessionsDtos = await _apiClient.GetSessionsAsync(command.MeetingKey);
        var addedSessions = new List<SessionDto>();
        var meetings = await _meetingRepository.GetAllAsync();
        
        foreach (var sessionDto in sessionsDtos)
        {
            var exists = await _sessionRepository.ExistsByKeyAsync(sessionDto.Session_Key);

            if (exists)
            {
                Log.Information("Session with key {SessionKey} already exists", sessionDto.Session_Key);
                continue;
            }
            
            var session = sessionDto.ToEntity();

            session.MeetingId = meetings
                .Where(m => m.Key == sessionDto.Meeting_Key)
                .Select(m => m.Id)
                .FirstOrDefault();
            
            await _sessionRepository.AddAsync(session);

            addedSessions.Add(sessionDto);
        }
        
        await Task.Delay(_options.ApiDelayMs);
    }
}