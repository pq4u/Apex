using Apex.Application.Abstractions;
using Apex.Application.DTO;
using Apex.Domain.Entities;

namespace Apex.Application.Queries.Meetings;

public record GetMeetingsQuery() : IQuery<List<Meeting>?>;