using Apex.Application.Abstractions;
using Apex.Domain.Entities;

namespace Apex.Application.Queries.Laps;

public record GetDriverLapsInSessionQuery(int SessionId, int DriverId) : IQuery<IEnumerable<Lap>?>;