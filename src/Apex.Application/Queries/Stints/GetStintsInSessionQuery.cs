using Apex.Application.Abstractions;
using Apex.Domain.Entities;

namespace Apex.Application.Queries.Stints;

public record GetStintsInSessionQuery(int SessionId) : IQuery<IEnumerable<Stint>?>;