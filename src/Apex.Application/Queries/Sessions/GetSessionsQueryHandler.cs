using Apex.Application.Abstractions;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Sessions;

public class GetSessionsQueryHandler : IQueryHandler<GetSessionsQuery, List<Session>?>
{
    private readonly ISessionRepository _sessionRepository;

    public GetSessionsQueryHandler(ISessionRepository sessionRepository)
        => _sessionRepository = sessionRepository;

    public async Task<List<Session>?> HandleAsync(GetSessionsQuery query, CancellationToken cancellationToken = default)
        => await _sessionRepository.GetAllAsync(cancellationToken);
}