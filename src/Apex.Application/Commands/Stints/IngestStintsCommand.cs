using Apex.Application.Abstractions;
using Apex.Domain.Entities;

namespace Apex.Application.Commands.Stints;

public record IngestStintsCommand(int SessionId, int SessionKey) : ICommand;