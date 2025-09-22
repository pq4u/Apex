using Apex.Application.Abstractions;
using Apex.Domain.Entities;

namespace Apex.Application.Queries.Drivers;

public record GetSessionDriversQuery(int SessionId) : IQuery<IEnumerable<Driver>?>;