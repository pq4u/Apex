using Apex.Application.Abstractions;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Sessions;

public class GetSessionQueryHandler : IQueryHandler<GetSessionQuery, Session?>
{
    private readonly ISessionRepository _sessionRepository;

    public GetSessionQueryHandler(ISessionRepository sessionRepository)
        => _sessionRepository = sessionRepository;

    public async Task<Session?> HandleAsync(GetSessionQuery query, CancellationToken cancellationToken = default)
        => await _sessionRepository.GetByKeyAsync(query.SessionKey, cancellationToken);
}