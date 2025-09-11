using Apex.Application.Abstractions;
using Apex.Domain.Entities;

namespace Apex.Application.Queries.Sessions;

public record GetSessionQuery(int SessionKey) : IQuery<Session?>;