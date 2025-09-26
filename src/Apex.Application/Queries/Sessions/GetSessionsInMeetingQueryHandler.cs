using Apex.Application.Abstractions;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Sessions;

public class GetSessionsInMeetingQueryHandler : IQueryHandler<GetSessionsInMeetingQuery, IEnumerable<Session>?>
{
    private readonly ISessionRepository _sessionRepository;

    public GetSessionsInMeetingQueryHandler(ISessionRepository sessionRepository)
        => _sessionRepository = sessionRepository;

    public async Task<IEnumerable<Session>?> HandleAsync(GetSessionsInMeetingQuery query)
        => await _sessionRepository.GetAllSessionsInMeetingAsync(query.MeetingId);
}