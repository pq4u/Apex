using Apex.Application.Abstractions;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Sessions;

public class GetSessionsQueryHandler : IQueryHandler<GetSessionsQuery, IEnumerable<Session>?>
{
    private readonly ISessionRepository _sessionRepository;

    public GetSessionsQueryHandler(ISessionRepository sessionRepository)
        => _sessionRepository = sessionRepository;

    public async Task<IEnumerable<Session>?> HandleAsync(GetSessionsQuery query)
        => await _sessionRepository.GetAllAsync();
}