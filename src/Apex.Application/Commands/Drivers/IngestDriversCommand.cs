using Apex.Application.Abstractions;

namespace Apex.Application.Commands.Drivers;

public record IngestDriversCommand(int SessionKey) : ICommand;