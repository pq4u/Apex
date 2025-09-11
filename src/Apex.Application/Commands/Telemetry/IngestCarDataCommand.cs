using Apex.Application.Abstractions;

namespace Apex.Application.Commands.Telemetry;

public record IngestCarDataCommand(int SessionKey, int SessionId, int DriverNumber, int DriverId) : ICommand;