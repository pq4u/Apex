using Apex.Application.Abstractions;

namespace Apex.Application.Commands.Laps;

public record IngestLapsCommand(int SessionKey, int SessionId, int DriverNumber, int DriverId) : ICommand;