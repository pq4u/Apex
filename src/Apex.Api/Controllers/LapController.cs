using Apex.Application.Abstractions;
using Apex.Application.DTO.Api;
using Apex.Application.Mappings;
using Apex.Application.Queries.Laps;
using Apex.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Api.Controllers;

[ApiController]
[Route("laps")]
public class LapController : ControllerBase
{
    private readonly IQueryHandler<GetDriverLapsInSessionQuery, IEnumerable<Lap>> _getDriverLapsInSessionQueryHandler;

    public LapController(IQueryHandler<GetDriverLapsInSessionQuery, IEnumerable<Lap>> getDriverLapsInSessionQueryHandler)
        =>_getDriverLapsInSessionQueryHandler = getDriverLapsInSessionQueryHandler;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LapResultDto>>> GetDriverLaps([FromQuery] GetDriverLapsInSessionQuery query)
    {
        var laps = await _getDriverLapsInSessionQueryHandler.HandleAsync(query);

        var result = laps.Select(x => x.ToResultDto());
        
        return Ok(result);
    }
}