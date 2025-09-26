using Apex.Application.Abstractions;
using Apex.Domain.Entities;

namespace Apex.Application.Queries.Races;

public record GetAllRacesQuery() : IQuery<IEnumerable<Session>?>;