using Apex.Application.Abstractions;
using Apex.Application.Client;
using Apex.Application.Mappings;
using Apex.Application.Services;
using Apex.Domain.Entities;
using Apex.Domain.Exceptions;
using Apex.Domain.Repositories;
using Serilog;

namespace Apex.Application.Commands.Meetings;

public class IngestMeetingsCommandHandler : ICommandHandler<IngestMeetingsCommand>
{
    
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IMeetingRepository _meetingRepository;

    public IngestMeetingsCommandHandler(IMeetingRepository meetingRepository, IOpenF1ApiClient apiClient)
    {
        _meetingRepository = meetingRepository;
        _apiClient = apiClient;
    }

    public async Task HandleAsync(IngestMeetingsCommand command)
    {
        Log.Information("Starting meeting list ingestion from OpenF1 API");

        try 
        {
            var meetings = await _apiClient.GetMeetingsAsync();
            
            if (!meetings!.Any())
            {
                throw new MeetingsNotFoundInApiException();
            }
            
            var existingDbMeetings = await _meetingRepository.GetAllAsync();

            var meetingsEntities = meetings!.Select(m => m.ToEntity()).ToList();

            foreach (var meeting in meetingsEntities)
            {
                var meetingExists = existingDbMeetings.Any(m => m.Key == meeting.Key);
                if (meetingExists)
                {
                    Log.Information("Meeting already exists in database {MeetingKey}", meeting.Key);
                    continue;
                }
                
                await _meetingRepository.AddAsync(meeting);
            }

            Log.Information("Completed meeting list ingestion from OpenF1 API");
        }
        catch (Exception ex)
        {
            Log.Error("Ingesting meetings failed: {ErrorMessage}", ex.Message);
        }
    }
}