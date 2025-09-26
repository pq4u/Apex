using Apex.Application.Abstractions;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Laps;

public class GetDriverLapsInSessionQueryHandler : IQueryHandler<GetDriverLapsInSessionQuery, IEnumerable<Lap>?>
{
    private readonly ILapRepository _lapRepository;

    public GetDriverLapsInSessionQueryHandler(ILapRepository lapRepository)
    {
        _lapRepository = lapRepository;
    }

    public async Task<IEnumerable<Lap>?> HandleAsync(GetDriverLapsInSessionQuery query)
        => await _lapRepository.GetDriverLapsInSessionAsync(query.SessionId, query.DriverId);
}