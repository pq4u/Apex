using Apex.Application.Abstractions;
using Apex.Application.Queries.Sessions;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Races;

public class GetAllRacesQueryHandler : IQueryHandler<GetAllRacesQuery, IEnumerable<Session>?>
{
    private readonly ISessionRepository _sessionRepository;

    public GetAllRacesQueryHandler(ISessionRepository sessionRepository)
        => _sessionRepository = sessionRepository;

    public async Task<IEnumerable<Session>?> HandleAsync(GetAllRacesQuery query)
        => await _sessionRepository.GetAllRacesAsync();
}