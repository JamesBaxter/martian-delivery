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
        var parcel = new Parcel(new ParcelStateMachine(State.Created));
        
        // Act Assert
        parcel.CurrentState.Should().Be(State.Created);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", "Now")
        ]);
    }
    
    [Fact]
    public void Parcel_Launched_HistorySet()
    {
        // Arrange
        var parcel = new Parcel(new ParcelStateMachine(State.Created));
        
        // Act
        parcel.Launch();
        
        // Assert
        parcel.CurrentState.Should().Be(State.OnRocketToMars);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", "Now"),
            new HistoryItem("OnRocketToMars", "Now"),
        ]);
    }
    
    [Fact]
    public void Parcel_LostOnRocket_HistorySet()
    {
        // Arrange
        var parcel = new Parcel(new ParcelStateMachine(State.Created));
        
        // Act
        parcel.Launch();
        parcel.Lose();
        
        // Assert
        parcel.CurrentState.Should().Be(State.Lost);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", "Now"),
            new HistoryItem("OnRocketToMars", "Now"),
            new HistoryItem("Lost", "Now")
        ]);
    }
    
    [Fact]
    public void Parcel_LostOutForDelivery_HistorySet()
    {
        // Arrange
        var parcel = new Parcel(new ParcelStateMachine(State.Created));
        
        // Act
        parcel.Launch();
        parcel.Land();
        parcel.Dispatch();
        parcel.Lose();
        
        // Assert
        parcel.CurrentState.Should().Be(State.Lost);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", "Now"),
            new HistoryItem("OnRocketToMars", "Now"),
            new HistoryItem("LandedOnMars", "Now"),
            new HistoryItem("OutForMartianDelivery", "Now"),
            new HistoryItem("Lost", "Now")
        ]);
    }
    
    [Fact]
    public void Parcel_Landed_HistorySet()
    {
        // Arrange
        var parcel = new Parcel(new ParcelStateMachine(State.Created));
        
        // Act
        parcel.Launch();
        parcel.Land();
        
        // Assert
        parcel.CurrentState.Should().Be(State.LandedOnMars);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", "Now"),
            new HistoryItem("OnRocketToMars", "Now"),
            new HistoryItem("LandedOnMars", "Now")
        ]);
    }
    
    [Fact]
    public void Parcel_Dispatched_HistorySet()
    {
        // Arrange
        var parcel = new Parcel(new ParcelStateMachine(State.Created));
        
        // Act
        parcel.Launch();
        parcel.Land();
        parcel.Dispatch();
        
        // Assert
        parcel.CurrentState.Should().Be(State.OutForMartianDelivery);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", "Now"),
            new HistoryItem("OnRocketToMars", "Now"),
            new HistoryItem("LandedOnMars", "Now"),
            new HistoryItem("OutForMartianDelivery", "Now")
        ]);
    }
    
    [Fact]
    public void Parcel_Delivered_HistorySet()
    {
        // Arrange
        var parcel = new Parcel(new ParcelStateMachine(State.Created));
        
        // Act
        parcel.Launch();
        parcel.Land();
        parcel.Dispatch();
        parcel.Deliver();
        
        // Assert
        parcel.CurrentState.Should().Be(State.Delivered);
        parcel.History.Should().BeEquivalentTo([
            new HistoryItem("Created", "Now"),
            new HistoryItem("OnRocketToMars", "Now"),
            new HistoryItem("LandedOnMars", "Now"),
            new HistoryItem("OutForMartianDelivery", "Now"),
            new HistoryItem("Delivered", "Now")
        ]);
    }
}