using Apex.Application.Client;
using Apex.Application.Commands.Laps;
using Apex.Application.DTO;
using Apex.Domain.Configuration;
using Apex.Domain.Entities;
using Apex.Domain.Exceptions;
using Apex.Domain.Repositories;
using Microsoft.Extensions.Options;
using Shouldly;

namespace Apex.UnitTests;

public class IngestLapsCommandHandlerTests
{
    private readonly IngestLapsCommandHandler _handler;
    private readonly ILapRepository _lapRepository;
    private readonly IOpenF1ApiClient _apiClient;
    private readonly IOptions<IngestionOptions> _ingestionOptions;

    public IngestLapsCommandHandlerTests()
    {
        _lapRepository = Substitute.For<ILapRepository>();
        _apiClient = Substitute.For<IOpenF1ApiClient>();
        _ingestionOptions = Options.Create(new IngestionOptions());
        _handler = new IngestLapsCommandHandler(_lapRepository, _apiClient, _ingestionOptions);
    }

    [Fact]
    public async Task HandleAsync_WhenApiReturnsNull_ShouldThrowLapsNotFoundForDriverInSessionException()
    {
        // arrange
        var command = new IngestLapsCommand(1234, 1234, 1234, 1234);
        _apiClient.GetLapsAsync(1234, 1234).Returns(new List<LapDto>());

        // act
        var exception = await Should.ThrowAsync<LapsNotFoundForDriverInSessionInApiException>(
        () => _handler.HandleAsync(command));
        
        // assert
        exception.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task HandleAsync_WhenLapsAlreadyExist_ShouldNotAddDuplicate()
    {
        // arrange
        var command = new IngestLapsCommand(1234, 1234, 1234, 1234);
        var laps = new List<LapDto>() { new() { SessionKey = 1234 } };

        _apiClient.GetLapsAsync(1234, 1234).Returns(laps);
        _lapRepository.GetDriverLapsInSessionCountAsync(1234, 1234).Returns(10);

        // act
        await _handler.HandleAsync(command);

        // assert
        await _lapRepository.DidNotReceive().AddDriverLapsAsync(Arg.Any<List<Lap>>());
    }

    [Fact]
    public async Task HandleAsync_WhenNewLaps_ShouldAddToRepository()
    {
        // arrange
        var sessionId = 1;
        var driverId = 2;
        
        var command = new IngestLapsCommand(1234, sessionId, 1234, driverId);
        var newLaps = new List<LapDto>() { new() { SessionKey = 1, LapDuration = 1234 }, new() { SessionKey = 1, LapDuration = 1235 } };
    
        _apiClient.GetLapsAsync(1234, 1234).Returns(newLaps);
        _lapRepository.GetDriverLapsInSessionCountAsync(1234, 1234).Returns(0);
    
        // act
        await _handler.HandleAsync(command);
    
        // assert
        await _lapRepository.Received(1).AddDriverLapsAsync(Arg.Is<List<Lap>>(
            laps => laps.All(l => l.SessionId == sessionId && l.DriverId == driverId))
        );
    }

}