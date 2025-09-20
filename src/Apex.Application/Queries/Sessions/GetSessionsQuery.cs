using Apex.Application.Abstractions;
using Apex.Domain.Entities;

namespace Apex.Application.Queries.Sessions;

public record GetSessionsQuery() : IQuery<IEnumerable<Session>?>;