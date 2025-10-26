using AwesomeAssertions;
using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain.UnitTests.StateMachine;

public class OnRocketToMarsStateTests
{
    [Theory]
    [InlineData(Trigger.Lose, State.Lost)]
    [InlineData(Trigger.Land, State.LandedOnMars)]
    public void OnRocketToMars_Valid_States(Trigger trigger, State expectedState)
    {
        // Arrange
        var sut = new ParcelStateMachine(State.OnRocketToMars);
        
        // Act
        sut.Fire(trigger);

        // Assert
        sut.CurrentState().Should().Be(expectedState);
    }
    
    [Theory]
    [InlineData(Trigger.Launch)]
    [InlineData(Trigger.Dispatch)]
    [InlineData(Trigger.Deliver)]
    public void OnRocketToMars_Triggers_Not_Valid(Trigger trigger)
    {
        // Arrange
        var sut = new ParcelStateMachine(State.OnRocketToMars);
        
        // Act Assert
        var act = () => sut.Fire(trigger);
        act.Should().Throw<InvalidOperationException>();
    }
}