using AwesomeAssertions;
using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain.UnitTests.StateMachine;

public class DeliveredStateTests
{
    [Theory]
    [InlineData(Trigger.Launch)]
    [InlineData(Trigger.Land)]
    [InlineData(Trigger.Dispatch)]
    [InlineData(Trigger.Deliver)]
    public void OutForMartianDelivery_Triggers_Not_Valid(Trigger trigger)
    {
        // Arrange
        var sut = new ParcelStateMachine(State.Delivered);
        
        // Act Assert
        var act = () => sut.Fire(trigger);
        act.Should().Throw<InvalidOperationException>();
    }
}