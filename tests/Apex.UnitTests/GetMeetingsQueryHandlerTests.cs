using Apex.Application.Queries.Meetings;
using Apex.Domain.Entities;
using Apex.Domain.Repositories;
using Shouldly;

namespace Apex.UnitTests;

public class GetMeetingsQueryHandlerTests
{
    private readonly GetMeetingsQueryHandler _handler;
    private readonly IMeetingRepository _meetingRepository;

    public GetMeetingsQueryHandlerTests()
    {
        _meetingRepository = Substitute.For<IMeetingRepository>();
        _handler = new GetMeetingsQueryHandler(_meetingRepository);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllMeetingsFromRepository()
    {
        // arrange
        var query = new GetMeetingsQuery();
        var expectedMeetings = new[]
        {
            new Meeting { Key = 1, Name = "Australian Grand Prix" },
            new Meeting { Key = 2, Name = "Bahrain Grand Prix" }
        };

        _meetingRepository.GetAllAsync().Returns(expectedMeetings);

        // act
        var result = await _handler.HandleAsync(query);

        // assert
        result.ShouldNotBeNull();
        result.ShouldBe(expectedMeetings);
        await _meetingRepository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task HandleAsync_WhenRepositoryReturnsNull_ShouldReturnNull()
    {
        // arrange
        var query = new GetMeetingsQuery();
        _meetingRepository.GetAllAsync().Returns((IEnumerable<Meeting>?)null);

        // act
        var result = await _handler.HandleAsync(query);

        // assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task HandleAsync_WhenRepositoryReturnsEmptyCollection_ShouldReturnEmptyCollection()
    {
        // arrange
        var query = new GetMeetingsQuery();
        var emptyMeetings = Array.Empty<Meeting>();

        _meetingRepository.GetAllAsync().Returns(emptyMeetings);

        // act
        var result = await _handler.HandleAsync(query);

        // assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }
}