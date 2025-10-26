using AwesomeAssertions;
using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain.UnitTests.StateMachine;

public class LandedOnMarsStateTests
{
    [Fact]
    public void LandedOnMars_Dispatch_Valid()
    {
        // Arrange
        var sut = new ParcelStateMachine(State.LandedOnMars);
        
        // Act
        sut.Fire(Trigger.Dispatch);

        // Assert
        sut.CurrentState().Should().Be(State.OutForMartianDelivery);
    }
    
    [Theory]
    [InlineData(Trigger.Launch)]
    [InlineData(Trigger.Lose)]
    [InlineData(Trigger.Deliver)]
    public void LandedOnMars_Triggers_Not_Valid(Trigger trigger)
    {
        // Arrange
        var sut = new ParcelStateMachine(State.LandedOnMars);
        
        // Act Assert
        var act = () => sut.Fire(trigger);
        act.Should().Throw<InvalidOperationException>();
    }
}