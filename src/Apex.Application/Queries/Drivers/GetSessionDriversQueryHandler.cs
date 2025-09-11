using Apex.Application.Abstractions;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Drivers;

public class GetSessionDriversQueryHandler : IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>>
{
    private readonly IDriverRepository _driverRepository;

    public GetSessionDriversQueryHandler(IDriverRepository driverRepository)
        => _driverRepository = driverRepository;

    public async Task<IEnumerable<Driver>> HandleAsync(GetSessionDriversQuery query, CancellationToken cancellationToken = default)
        => await _driverRepository.GetDriversBySessionIdAsync(query.SessionId, cancellationToken);
}