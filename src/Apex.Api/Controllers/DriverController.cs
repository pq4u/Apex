using Apex.Application.Abstractions;
using Apex.Application.DTO.Api;
using Apex.Application.Queries.Drivers;
using Apex.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Api.Controllers;

[ApiController]
[Route("drivers")]
public class DriverController : ControllerBase
{
    private readonly IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>> _getDriverLapsInSessionQueryHandler;

    public DriverController(IQueryHandler<GetSessionDriversQuery, IEnumerable<Driver>> getDriverLapsInSessionQueryHandler)
    {
        _getDriverLapsInSessionQueryHandler = getDriverLapsInSessionQueryHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriverResultDto>>> GetDriversInSession([FromQuery] GetSessionDriversQuery command)
    {
        var driversInSession = await _getDriverLapsInSessionQueryHandler.HandleAsync(command);

        var result = driversInSession.Select(x => new DriverResultDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            NameAcronym = x.NameAcronym,
            DriverNumber = x.DriverNumber,
            CountryCode = x.CountryCode,
            HeadshotUrl = x.HeadshotUrl
        });

        return Ok(result);
    }
}