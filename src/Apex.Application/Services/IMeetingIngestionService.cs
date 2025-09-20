using Apex.Application.Commands.Meetings;

namespace Apex.Application.Services;

public interface IMeetingIngestionService
{
    Task<bool> IngestMeetingsAsync(IngestMeetingsCommand request);
}