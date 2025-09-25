using Apex.Application.Abstractions;
using Apex.Domain.Entities;

namespace Apex.Application.Queries.Sessions;

public record GetSessionsInMeetingQuery(int meetingId) : IQuery<IEnumerable<Session>?>;