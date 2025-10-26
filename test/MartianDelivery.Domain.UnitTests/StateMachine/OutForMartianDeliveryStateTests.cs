using AwesomeAssertions;
using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain.UnitTests.StateMachine;

public class OutForMartianDeliveryStateTests
{
    [Theory]
    [InlineData(Trigger.Lose, State.Lost)]
    [InlineData(Trigger.Deliver, State.Delivered)]
    public void OutForMartianDelivery_Valid_States(Trigger trigger, State expectedState)
    {
        // Arrange
        var sut = new ParcelStateMachine(State.OutForMartianDelivery);
        
        // Act
        sut.Fire(trigger);

        // Assert
        sut.CurrentState().Should().Be(expectedState);
    }
    
    [Theory]
    [InlineData(Trigger.Launch)]
    [InlineData(Trigger.Land)]
    [InlineData(Trigger.Dispatch)]
    public void OutForMartianDelivery_Triggers_Not_Valid(Trigger trigger)
    {
        // Arrange
        var sut = new ParcelStateMachine(State.OutForMartianDelivery);
        
        // Act Assert
        var act = () => sut.Fire(trigger);
        act.Should().Throw<InvalidOperationException>();
    }
}