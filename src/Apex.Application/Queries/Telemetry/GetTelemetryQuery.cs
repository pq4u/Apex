using Apex.Application.Abstractions;
using Apex.Domain.TimeSeries;

namespace Apex.Application.Queries.Telemetry;

public record GetTelemetryQuery(int SessionId, int DriverId, int LapNumber) : IQuery<IEnumerable<Domain.TimeSeries.Telemetry>?>;