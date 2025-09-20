using Apex.Application.Abstractions;
using Apex.Application.Services;
using Apex.Domain.Requests;
using Serilog;

namespace Apex.Application.Commands.Sessions;

public class IngestSessionsCommandHandler : ICommandHandler<IngestSessionsCommand>
{
    private readonly ISessionIngestionService _sessionIngestionService;

    public IngestSessionsCommandHandler(ISessionIngestionService sessionIngestionService)
        => _sessionIngestionService = sessionIngestionService;

    public async Task HandleAsync(IngestSessionsCommand command)
    {
        var request = new SessionIngestionRequest(command.MeetingKey);
        var result = await _sessionIngestionService.IngestSessionsAsync(request);

        if (!result.Any())
        {
            Log.Information("No ingested sessions for {MeetingKey}", command.MeetingKey);;
        }
    }
}