using Apex.Application.Abstractions;
using Apex.Domain.TimeSeries;

namespace Apex.Application.Queries.Telemetry;

public record GetCarDataQuery(int SessionId, int DriverId, DateTime? Start = null, DateTime? End = null) 
    : IQuery<IEnumerable<CarData>>;