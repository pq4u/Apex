using Apex.Application.Abstractions;

namespace Apex.Application.Commands.Sessions;

public record CreateSessionCommand(int SessionKey) : ICommand;