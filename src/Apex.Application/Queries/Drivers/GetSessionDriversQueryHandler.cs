using Apex.Application.Abstractions;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Drivers;

public class GetSessionDriversQueryHandler : IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>>
{
    private readonly ISessionDriverRepository _sessionDriverRepository;

    public GetSessionDriversQueryHandler(ISessionDriverRepository sessionDriverRepository)
        => _sessionDriverRepository = sessionDriverRepository;

    public async Task<IEnumerable<Driver>> HandleAsync(GetSessionDriversQuery query)
        => await _sessionDriverRepository.GetDriversBySessionIdAsync(query.SessionId);
}