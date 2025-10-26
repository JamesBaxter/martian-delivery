using AwesomeAssertions;
using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain.UnitTests.StateMachine;

public class CreatedStateTests
{
    [Fact]
    public void Created_Launch_Valid()
    {
        // Arrange
         var sut = new ParcelStateMachine(State.Created);
        
        // Act
        sut.Fire(Trigger.Launch);

        // Assert
        sut.CurrentState().Should().Be(State.OnRocketToMars);
    }
    
    [Theory]
    [InlineData(Trigger.Lose)]
    [InlineData(Trigger.Land)]
    [InlineData(Trigger.Dispatch)]
    [InlineData(Trigger.Deliver)]
    public void Created_Triggers_Not_Valid(Trigger trigger)
    {
        // Arrange
        var sut = new ParcelStateMachine(State.Created);
        
        // Act Assert
        var act = () => sut.Fire(trigger);
        act.Should().Throw<InvalidOperationException>();
    }
}