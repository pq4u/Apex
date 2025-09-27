using Apex.Application.Abstractions;
using Apex.Application.DTO.Api;
using Apex.Application.Queries.Telemetry;
using Apex.Domain.TimeSeries;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Api.Controllers;

[ApiController]
[Route("telemetry")]
public class TelemetryController : ControllerBase
{
    private readonly IQueryHandler<GetTelemetryQuery, IEnumerable<TelemetryData>> _getTelemetryQueryHandler;

    public TelemetryController(IQueryHandler<GetTelemetryQuery, IEnumerable<TelemetryData>> getTelemetryQueryHandler)
    {
        _getTelemetryQueryHandler = getTelemetryQueryHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TelemetryDataResultDto>?>> Get([FromQuery] GetTelemetryQuery query)
    {
        var telemetryData = await _getTelemetryQueryHandler.HandleAsync(query);

        var result = telemetryData.Select(x => new TelemetryDataResultDto
        {
            Time = x.Time,
            SessionId = x.SessionId,
            DriverId = x.DriverId,
            Speed = x.Speed,
            Rpm = x.Rpm,
            Gear = x.Gear,
            Throttle = x.Throttle,
            Brake = x.Brake,
            Drs = x.Drs,
            NGear = x.NGear,
        });

        return Ok(result);
    }
}