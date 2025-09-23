using Apex.Application.Abstractions;
using Apex.Application.DTO.Api;
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
    {
        _getDriverLapsInSessionQueryHandler = getDriverLapsInSessionQueryHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LapResultDto>>> GetDriverLaps([FromQuery] GetDriverLapsInSessionQuery command)
    {
        var laps = await _getDriverLapsInSessionQueryHandler.HandleAsync(command);

        var result = laps.Select(x => new LapResultDto
        {
            Id = x.Id,
            SessionId = x.SessionId,
            DriverId = x.DriverId,
            LapNumber = x.LapNumber,
            DateStart = x.DateStart,
            LapDurationMs = x.LapDurationMs,
            DurationSector1Ms = x.DurationSector1Ms,
            DurationSector2Ms = x.DurationSector2Ms,
            DurationSector3Ms = x.DurationSector3Ms,
            I1Speed = x.I1Speed,
            I2Speed = x.I2Speed,
            FinishLineSpeed = x.FinishLineSpeed,
            StSpeed = x.StSpeed,
            IsPitOutLap = x.IsPitOutLap,
            SegmentsJson = x.SegmentsJson
        });
        
        return Ok(result);
    }
}