using Apex.Application.Abstractions;
using Apex.Application.DTO.Api;
using Apex.Application.Queries.Sessions;
using Apex.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Api.Controllers;

[ApiController]
[Route("sessions")]
public class SessionController : ControllerBase
{
    private readonly IQueryHandler<GetSessionsQuery, IEnumerable<Session>> _getMeetingsQueryHandler;

    public SessionController(IQueryHandler<GetSessionsQuery, IEnumerable<Session>> getMeetingsQueryHandler)
    {
        _getMeetingsQueryHandler = getMeetingsQueryHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SessionResultDto>>> Get()
    {
        var query = new GetSessionsQuery();
        var sessions = await _getMeetingsQueryHandler.HandleAsync(query);

        var result = sessions.Select(x => new SessionResultDto
        {
            Id = x.Id,
            MeetingId = x.MeetingId,
            Name = x.Name,
            StartDate = x.StartDate,
            EndDate = x.EndDate
        });
        
        return Ok(result);
    }
}