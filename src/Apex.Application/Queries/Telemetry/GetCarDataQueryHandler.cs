using Apex.Application.Abstractions;
using Apex.Domain.Repositories;
using Apex.Domain.TimeSeries;

namespace Apex.Application.Queries.Telemetry;

public class GetCarDataQueryHandler : IQueryHandler<GetCarDataQuery, IEnumerable<CarData>>
{
    private readonly ITelemetryRepository _telemetryRepository;

    public GetCarDataQueryHandler(ITelemetryRepository telemetryRepository)
        => _telemetryRepository = telemetryRepository;

    public async Task<IEnumerable<CarData>> HandleAsync(GetCarDataQuery query, CancellationToken cancellationToken = default)
        => await _telemetryRepository.GetCarDataAsync(query.SessionId, query.DriverId, query.Start, query.End);
}