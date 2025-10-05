using Apex.Application.Abstractions;
using Apex.Application.DTO.Api;
using Apex.Application.Mappings;
using Apex.Application.Queries.Drivers;
using Apex.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Api.Controllers;

[ApiController]
[Route("drivers")]
public class DriverController : ControllerBase
{
    private readonly IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>?> _getSessionDriversQueryHandler;

    public DriverController(IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>?> getSessionDriversQueryHandler)
        => _getSessionDriversQueryHandler = getSessionDriversQueryHandler;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriverResultDto>>> GetDriversInSession([FromQuery] GetSessionDriversQuery command)
    {
        var driversInSession = await _getSessionDriversQueryHandler.HandleAsync(command);

        var result = driversInSession?.Select(x => x.ToResultDto());

        return Ok(result);
    }
}