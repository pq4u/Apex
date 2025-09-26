using Apex.Application.Abstractions;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;

namespace Apex.Application.Queries.Stints;

public class GetStintsInSessionQueryHandler : IQueryHandler<GetStintsInSessionQuery, IEnumerable<Stint>?>
{
    private readonly IStintRepository _stintRepository;

    public GetStintsInSessionQueryHandler(IStintRepository stintRepository)
        => _stintRepository = stintRepository;

    public async Task<IEnumerable<Stint>?> HandleAsync(GetStintsInSessionQuery query)
        => await _stintRepository.GetBySessionIdAsync(query.SessionId);
}