using Apex.Application.Abstractions;
using Apex.Application.DTO;
using Apex.Application.DTO.Api;
using Apex.Application.Queries.Meetings;
using Apex.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Api.Controllers;

[ApiController]
[Route("meetings")]
public class MeetingController : ControllerBase
{
    private readonly IQueryHandler<GetMeetingsQuery, IEnumerable<Meeting>> _getMeetingsQueryHandler;

    public MeetingController(IQueryHandler<GetMeetingsQuery, IEnumerable<Meeting>> getMeetingsQueryHandler)
    {
        _getMeetingsQueryHandler = getMeetingsQueryHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeetingResultDto>>> Get()
    {
        var query = new GetMeetingsQuery();
        var meetings = await _getMeetingsQueryHandler.HandleAsync(new GetMeetingsQuery());

        var response = meetings.Select(m => new MeetingResultDto
        {
            Id = m.Id,
            Name = m.Name,
            Location = m.Location,
            CountryName = m.CountryName,
            CircuitName = m.CircuitShortName,
            DateStart = m.DateStart
        });

        return Ok(response);

    }
}