using Apex.Application.Abstractions;
using Apex.Application.DTO;
using Apex.Application.DTO.Api;
using Apex.Application.Mappings;
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
        var meetings = await _getMeetingsQueryHandler.HandleAsync(query);

        var response = meetings.Select(x => x.ToResultDto());

        return Ok(response);

    }
}