using Apex.Application.Abstractions;
using Apex.Application.DTO.Api;
using Apex.Application.Mappings;
using Apex.Application.Queries.Telemetry;
using Apex.Domain.TimeSeries;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Api.Controllers;

[ApiController]
[Route("telemetry")]
public class TelemetryController : ControllerBase
{
    private readonly IQueryHandler<GetTelemetryQuery, IEnumerable<Telemetry>?> _getTelemetryQueryHandler;

    public TelemetryController(IQueryHandler<GetTelemetryQuery, IEnumerable<Telemetry>?> getTelemetryQueryHandler)
        => _getTelemetryQueryHandler = getTelemetryQueryHandler;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TelemetryResultDto>?>> Get([FromQuery] GetTelemetryQuery query)
    {
        var telemetryData = await _getTelemetryQueryHandler.HandleAsync(query);

        var result = telemetryData?.Select(x => x.ToResultDto());

        return Ok(result);
    }
}