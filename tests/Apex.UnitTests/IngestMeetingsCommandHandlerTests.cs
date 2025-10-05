using Apex.Application.Commands.Meetings;
using Apex.Application.Client;
using Apex.Application.DTO;
using Apex.Domain.Entities;
using Apex.Domain.Exceptions;
using Apex.Domain.Repositories;
using Shouldly;

namespace Apex.UnitTests;

public class IngestMeetingsCommandHandlerTests
{
    private readonly IngestMeetingsCommandHandler _handler;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IOpenF1ApiClient _apiClient;

    public IngestMeetingsCommandHandlerTests()
    {
        _meetingRepository = Substitute.For<IMeetingRepository>();
        _apiClient = Substitute.For<IOpenF1ApiClient>();
        _handler = new IngestMeetingsCommandHandler(_meetingRepository, _apiClient);
    }

    [Fact]
    public async Task HandleAsync_WhenApiReturnsEmptyList_ShouldThrowMeetingsNotFoundInMeetingInApiException()
    {
        // arrange
        var command = new IngestMeetingsCommand();
        _apiClient.GetMeetingsAsync().Returns(new List<MeetingDto>());

        // act
        var exception = await Should.ThrowAsync<MeetingsNotFoundInApiException>(
            () => _handler.HandleAsync(command));
        
        // assert
        exception.ShouldNotBeNull();
    }

    [Fact]
    public async Task HandleAsync_WhenMeetingAlreadyExists_ShouldNotAddDuplicate()
    {
        // arrange
        var command = new IngestMeetingsCommand();
        var meetingDto = new MeetingDto 
        { 
            MeetingKey = 1234,
            MeetingName = "Poland Grand Prix",
            CountryName = "Poland"
        };
        
        var existingMeeting = new Meeting 
        { 
            Key = 1234,
            Name = "Poland Grand Prix",
        };

        _apiClient.GetMeetingsAsync().Returns(new List<MeetingDto>(){ meetingDto });
        _meetingRepository.GetAllAsync().Returns(new[] { existingMeeting });

        // act
        await _handler.HandleAsync(command);

        // assert
        await _meetingRepository.DidNotReceive().AddAsync(Arg.Any<Meeting>());
    }

    [Fact]
    public async Task HandleAsync_WhenNewMeeting_ShouldAddToRepository()
    {
        // arrange
        var command = new IngestMeetingsCommand();
        var meetingDto = new MeetingDto 
        { 
            MeetingKey = 1234,
            MeetingName = "New Grand Prix",
            CountryName = "New Country"
        };

        _apiClient.GetMeetingsAsync().Returns(new List<MeetingDto>{ meetingDto });
        _meetingRepository.GetAllAsync().Returns(Array.Empty<Meeting>());

        // act
        await _handler.HandleAsync(command);

        // assert
        await _meetingRepository.Received(1).AddAsync(Arg.Is<Meeting>(m => m.Key == 1234));
    }
    
    
}