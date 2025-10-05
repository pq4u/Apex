using Apex.Application.Abstractions;
using Apex.Application.DTO.Api;
using Apex.Application.Mappings;
using Apex.Application.Queries.Sessions;
using Apex.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Apex.Api.Controllers;

[ApiController]
[Route("sessions")]
public class SessionController : ControllerBase
{
    private readonly IQueryHandler<GetSessionsInMeetingQuery, IEnumerable<Session>?> _getSessionsInMeetingQueryHandler;

    public SessionController(IQueryHandler<GetSessionsInMeetingQuery, IEnumerable<Session>?> getSessionsInMeetingQueryHandler)
        => _getSessionsInMeetingQueryHandler = getSessionsInMeetingQueryHandler;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SessionResultDto>>> Get([FromQuery] GetSessionsInMeetingQuery query)
    {
        var sessions = await _getSessionsInMeetingQueryHandler.HandleAsync(query);

        var result = sessions?.Select(x=> x.ToResultDto());
        
        return Ok(result);
    }
}