using AutoFixture;
using AwesomeAssertions;
using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain.UnitTests;

public class ParcelTests
{
    private readonly Fixture _fixture = new();

    public ParcelTests()
    {
        _fixture.Register<IParcelStateMachine>(() => new ParcelStateMachine(State.Created));
    }
    
    [Fact]
    public void Parcel_Constructor_HistorySet()
    {
        // Arrange
        var now = _fixture.Create<DateTimeOffset>();
        var parcel = new Parcel(new ParcelStateMachine(State.Created), now);
        
        // Act Assert
        parcel.CurrentState.Should().Be(State.Created);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", now)
        ]);
    }
    
    [Fact]
    public void Parcel_Launched_HistorySet()
    {
        // Arrange
        var now = _fixture.Create<DateTimeOffset>();
        var parcel = new Parcel(new ParcelStateMachine(State.Created), now);
        
        // Act
        parcel.Launch(now);
        
        // Assert
        parcel.CurrentState.Should().Be(State.OnRocketToMars);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", now),
            new HistoryItem("OnRocketToMars", now),
        ]);
    }
    
    [Fact]
    public void Parcel_LostOnRocket_HistorySet()
    {
        // Arrange
        var now = _fixture.Create<DateTimeOffset>();
        var launchTime = now.AddDays(1);
        var loseTime = now.AddDays(2);
        var parcel = new Parcel(new ParcelStateMachine(State.Created), now);
        
        // Act
        parcel.Launch(launchTime);
        parcel.Lose(loseTime);
        
        // Assert
        parcel.CurrentState.Should().Be(State.Lost);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", now),
            new HistoryItem("OnRocketToMars", launchTime),
            new HistoryItem("Lost", loseTime)
        ]);
    }
    
    [Fact]
    public void Parcel_LostOutForDelivery_HistorySet()
    {
        // Arrange
        var now = _fixture.Create<DateTimeOffset>();
        var launchTime = now.AddDays(1);
        var landTime = now.AddDays(2);
        var dispatchTime = now.AddDays(3);
        var loseTime = now.AddDays(4);
        var parcel = new Parcel(new ParcelStateMachine(State.Created), now);
        
        // Act
        parcel.Launch(launchTime);
        parcel.Land(landTime);
        parcel.Dispatch(dispatchTime);
        parcel.Lose(loseTime);
        
        // Assert
        parcel.CurrentState.Should().Be(State.Lost);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", now),
            new HistoryItem("OnRocketToMars", launchTime),
            new HistoryItem("LandedOnMars", landTime),
            new HistoryItem("OutForMartianDelivery", dispatchTime),
            new HistoryItem("Lost", loseTime)
        ]);
    }
    
    [Fact]
    public void Parcel_Landed_HistorySet()
    {
        // Arrange
        var now = _fixture.Create<DateTimeOffset>();
        var launchTime = now.AddDays(1);
        var landTime = now.AddDays(2);
        var parcel = new Parcel(new ParcelStateMachine(State.Created), now);
        
        // Act
        parcel.Launch(launchTime);
        parcel.Land(landTime);
        
        // Assert
        parcel.CurrentState.Should().Be(State.LandedOnMars);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", now),
            new HistoryItem("OnRocketToMars", launchTime),
            new HistoryItem("LandedOnMars", landTime)
        ]);
    }
    
    [Fact]
    public void Parcel_Dispatched_HistorySet()
    {
        // Arrange
        var now = _fixture.Create<DateTimeOffset>();
        var launchTime = now.AddDays(1);
        var landTime = now.AddDays(2);
        var dispatchTime = now.AddDays(3);
        var parcel = new Parcel(new ParcelStateMachine(State.Created), now);
        
        // Act
        parcel.Launch(launchTime);
        parcel.Land(landTime);
        parcel.Dispatch(dispatchTime);
        
        // Assert
        parcel.CurrentState.Should().Be(State.OutForMartianDelivery);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", now),
            new HistoryItem("OnRocketToMars", launchTime),
            new HistoryItem("LandedOnMars", landTime),
            new HistoryItem("OutForMartianDelivery", dispatchTime)
        ]);
    }
    
    [Fact]
    public void Parcel_Delivered_HistorySet()
    {
        // Arrange
        var now = _fixture.Create<DateTimeOffset>();
        var launchTime = now.AddDays(1);
        var landTime = now.AddDays(2);
        var dispatchTime = now.AddDays(3);
        var deliverTime = now.AddDays(4);
        var parcel = new Parcel(new ParcelStateMachine(State.Created), now);
        
        // Act
        parcel.Launch(launchTime);
        parcel.Land(landTime);
        parcel.Dispatch(dispatchTime);
        parcel.Deliver(deliverTime);
        
        // Assert
        parcel.CurrentState.Should().Be(State.Delivered);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", now),
            new HistoryItem("OnRocketToMars", launchTime),
            new HistoryItem("LandedOnMars", landTime),
            new HistoryItem("OutForMartianDelivery", dispatchTime),
            new HistoryItem("Delivered", deliverTime)
        ]);
    }
}