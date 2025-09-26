using Apex.Application.Abstractions;

namespace Apex.Application.Commands.SessionDrivers;

public record CreateSessionDriversCommand(int SessionId, int SessionKey) : ICommand;