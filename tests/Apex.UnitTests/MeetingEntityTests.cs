using Apex.Domain.Entities;
using Shouldly;

namespace Apex.UnitTests;

public class MeetingEntityTests
{
    [Fact]
    public void Meeting_WhenCreated_ShouldHaveCorrectProperties()
    {
        // arrange, act
        var meeting = new Meeting
        {
            Key = 1234,
            Name = "Poland Grand Prix",
            OfficialName = "FORMULA 1 GRAND PRIX POLAND 2077",
            Location = "Poland",
            CountryName = "Poland",
            CircuitKey = 1,
            CircuitShortName = "POL"
        };

        // assert
        meeting.Key.ShouldBe(1234);
        meeting.Name.ShouldBe("Poland Grand Prix");
        meeting.OfficialName.ShouldBe("FORMULA 1 GRAND PRIX POLAND 2077");
        meeting.Location.ShouldBe("Poland");
        meeting.CountryName.ShouldBe("Poland");
        meeting.CircuitKey.ShouldBe(1);
        meeting.CircuitShortName.ShouldBe("POL");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Meeting_WithInvalidKey_ShouldStillAllowCreation(int invalidKey) // to modify
    {
        // arrange, act
        var meeting = new Meeting { Key = invalidKey };

        // assert
        meeting.Key.ShouldBe(invalidKey);
    }


    [Fact]
    public void Meeting_DefaultValues_ShouldBeCorrect()
    {
        // arrange, act
        var meeting = new Meeting();

        // assert
        meeting.Key.ShouldBe(0);
        meeting.Name.ShouldBeNull();
        meeting.OfficialName.ShouldBeNull();
        meeting.Location.ShouldBeNull();
        meeting.CountryName.ShouldBeNull();
        meeting.CircuitKey.ShouldBe(0);
        meeting.CircuitShortName.ShouldBeNull();
    }
}