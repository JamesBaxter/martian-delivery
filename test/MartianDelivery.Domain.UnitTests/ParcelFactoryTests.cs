using AutoFixture;
using AwesomeAssertions;
using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain.UnitTests;

public class ParcelFactoryTests
{
    private readonly Fixture _fixture = new();

    private readonly ParcelFactory _sut = new();

    [Fact]
    public void Create_Creates_Valid_Domain()
    {
        // Arrange
        var barcode = _fixture.Create<string>();
        var contents = _fixture.Create<string>();
        var recipient = _fixture.Create<string>();
        var sender = _fixture.Create<string>();
        var now = _fixture.Create<DateTimeOffset>();

        // Act
        var result = _sut.Create(barcode, contents, recipient, sender, now);

        // Assert
        result.Should().BeEquivalentTo(new Parcel(new ParcelStateMachine(State.Created), now)
        {
            Barcode = barcode,
            Status = "Created",
            LaunchDate = "2025-09-03",
            EtaDays = 90,
            EstimatedArrivalDate = "2025-12-02",
            Origin = "Starport Thames Estuary",
            Destination = "New London",
            Sender = sender,
            Recipient = recipient,
            Contents = contents,
            History =
            [
                new HistoryItem(nameof(State.Created), now)
            ]
        });
        result.CurrentState.Should().Be(State.Created);
    }
}