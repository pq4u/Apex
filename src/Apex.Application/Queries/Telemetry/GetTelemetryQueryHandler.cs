using Apex.Application.Abstractions;
using Apex.Domain.Repositories;
using Apex.Domain.TimeSeries;

namespace Apex.Application.Queries.Telemetry;

public class GetTelemetryQueryHandler : IQueryHandler<GetTelemetryQuery, IEnumerable<Domain.TimeSeries.Telemetry>?>
{
    private readonly ITelemetryRepository _telemetryRepository;
    private readonly ILapRepository _lapRepository;

    public GetTelemetryQueryHandler(ITelemetryRepository telemetryRepository, ILapRepository lapRepository)
    {
        _telemetryRepository = telemetryRepository;
        _lapRepository = lapRepository;
    }

    public async Task<IEnumerable<Domain.TimeSeries.Telemetry>?> HandleAsync(GetTelemetryQuery query)
    {
        var lapDatesRange = await _lapRepository.GetConsecutiveLapsAsync(query.SessionId, query.DriverId, query.LapNumber);

        DateTime? dateFrom = null;
        DateTime? dateTo = null;
        if (lapDatesRange.Count() == 2)
        {
            dateFrom = lapDatesRange.FirstOrDefault(x => x.LapNumber == query.LapNumber).DateStart;
            dateTo = lapDatesRange.FirstOrDefault(x => x.LapNumber == (query.LapNumber + 1)).DateStart;
        }
        // handle if other cases
        
        return await _telemetryRepository.GetCarDataAsync(query.SessionId, query.DriverId, dateFrom, dateTo);
    }
}