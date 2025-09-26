using Apex.Application.Abstractions;
using Apex.Application.DTO.Api;
using Apex.Application.Queries.Stints;
using Apex.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Api.Controllers;

[ApiController]
[Route("/stints")]
public class StintController : ControllerBase
{
    private readonly IQueryHandler<GetStintsInSessionQuery, IEnumerable<Stint>?> _getStintsInSessionQueryHandler;

    public StintController(IQueryHandler<GetStintsInSessionQuery, IEnumerable<Stint>?> getStintsInSessionQueryHandler)
        => _getStintsInSessionQueryHandler = getStintsInSessionQueryHandler;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StintResultDto>>> GetInSession([FromQuery] GetStintsInSessionQuery query)
    {
        var stints = await _getStintsInSessionQueryHandler.HandleAsync(query);

        var result = stints.Select(x => new StintResultDto
        {
            SessionId = x.SessionId,
            DriverId = x.DriverId,
            StintNumber = x.StintNumber,
            StartLap = x.LapStart,
            EndLap = x.LapEnd,
            Compound = x.Compound,
            StartTyreAge = x.TyreAgeAtStart
        });
        
        return Ok(result);
    }
}