using Apex.Application.Abstractions;
using Apex.Application.Services;
using Apex.Domain.Entities;
using Serilog;

namespace Apex.Application.Commands.Meetings;

public class IngestMeetingsCommandHandler : ICommandHandler<IngestMeetingsCommand>
{
    private readonly IMeetingIngestionService _meetingIngestionService;

    public IngestMeetingsCommandHandler(IMeetingIngestionService meetingIngestionService)
    {
        _meetingIngestionService = meetingIngestionService;
    }

    public async Task HandleAsync(IngestMeetingsCommand command)
    {
        var result = await _meetingIngestionService.IngestMeetingsAsync(command);

        if (!result)
        {
            throw new InvalidOperationException($"Meeting ingestion failed..."); // add error message
        }
    }
}