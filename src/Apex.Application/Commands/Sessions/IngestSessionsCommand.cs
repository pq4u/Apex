using Apex.Application.Abstractions;

namespace Apex.Application.Commands.Sessions;

public record IngestSessionsCommand(int MeetingKey) : ICommand;