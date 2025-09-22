using Apex.Application.Client;
using Apex.Application.Commands.Laps;
using Apex.Application.Commands.Sessions;
using Apex.Application.DTO;
using Apex.Domain.Configuration;
using Apex.Domain.Entities;
using Apex.Domain.Exceptions;
using Apex.Domain.Repositories;
using Microsoft.Extensions.Options;
using Shouldly;

namespace Apex.UnitTests;

public class IngestSessionsCommandHandlerTests
{
    private readonly IngestSessionsCommandHandler _handler;
    private readonly ISessionRepository _sessionRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IOptions<IngestionOptions> _ingestionOptions;

    public IngestSessionsCommandHandlerTests()
    {
        _sessionRepository = Substitute.For<ISessionRepository>();
        _meetingRepository = Substitute.For<IMeetingRepository>();
        _apiClient = Substitute.For<IOpenF1ApiClient>();
        _ingestionOptions = Options.Create(new IngestionOptions());
        _handler = new IngestSessionsCommandHandler(_sessionRepository, _meetingRepository, _apiClient, _ingestionOptions);
    }

    [Fact]
    public async Task HandleAsync_WhenApiReturnsEmptyList_ShouldThrowSessionsNotFoundInApiException()
    {
        // arrange
        var meetingKey = 1234;
        var command = new IngestSessionsCommand(meetingKey);
        _apiClient.GetSessionsAsync(meetingKey).Returns(new List<SessionDto>());

        // act
        var exception = await Should.ThrowAsync<SessionsInMeetingNotFoundInApiException>(
        () => _handler.HandleAsync(command));
        
        // assert
        exception.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task HandleAsync_WhenSessionsAlreadyExist_ShouldNotAddDuplicate()
    {
        // arrange
        var command = new IngestSessionsCommand(1);
        var sessions = new List<SessionDto>() { new() { Session_Key = 1234 } };

        _apiClient.GetSessionsAsync(1).Returns(sessions);
        _sessionRepository.ExistsByKeyAsync(1234).Returns(true);

        // act
        await _handler.HandleAsync(command);

        // assert
        await _sessionRepository.DidNotReceive().AddAsync(Arg.Any<Session>());
    }

    [Fact]
    public async Task HandleAsync_WhenNewSessions_ShouldAddToRepository()
    {
        var command = new IngestSessionsCommand(1);
        var newSessions = new List<SessionDto>(){ new () { Session_Key = 1234 }, new () { Session_Key = 1235 } };
        var existingSessions = new List<Session>(){ new () { Key = 1234 }, new () { Key = 1235 } };

        _apiClient.GetSessionsAsync(1).Returns(newSessions);
        _sessionRepository.ExistsByKeyAsync(1234).Returns(false);
        _sessionRepository.ExistsByKeyAsync(1235).Returns(false);
        _meetingRepository.GetAllAsync().Returns(new List<Meeting>());

        await _handler.HandleAsync(command);
        
        await _sessionRepository.Received(2).AddAsync(Arg.Any<Session>());
    }
}