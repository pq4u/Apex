using Apex.Application.Abstractions;
using Apex.Domain.Repositories;
using Apex.Domain.TimeSeries;

namespace Apex.Application.Queries.Telemetry;

public class GetTelemetryQueryHandler : IQueryHandler<GetTelemetryQuery, IEnumerable<TelemetryData>>
{
    private readonly ITelemetryRepository _telemetryRepository;

    public GetTelemetryQueryHandler(ITelemetryRepository telemetryRepository)
        => _telemetryRepository = telemetryRepository;

    public async Task<IEnumerable<TelemetryData>> HandleAsync(GetTelemetryQuery query)
        => await _telemetryRepository.GetCarDataAsync(query.SessionId, query.DriverId);
}