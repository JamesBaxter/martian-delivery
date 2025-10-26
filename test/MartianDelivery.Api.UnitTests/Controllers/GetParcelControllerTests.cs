using AutoFixture;
using AwesomeAssertions;
using MartianDelivery.Controllers;
using MartianDelivery.Domain;
using MartianDelivery.Domain.StateMachine;
using MartianDelivery.Models;
using MartianDelivery.Persistence;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MartianDelivery.Api.UnitTests.Controllers;

public class GetParcelControllerTests
{
    private readonly ILogger<GetParcelController> _loggerMock = Substitute.For<ILogger<GetParcelController>>();
    private readonly IParcelRepository _parcelRepositoryMock = Substitute.For<IParcelRepository>();
    private readonly Fixture _fixture = new();
    private readonly GetParcelController _sut;

    public GetParcelControllerTests()
    {
        _fixture.Register<IParcelStateMachine>(() => new ParcelStateMachine(State.Created));
        _sut = new GetParcelController(_parcelRepositoryMock, _loggerMock);
    }

    [Fact]
    public void Get_Calls_ParcelRepository_Returns()
    {
        // Arrange
        var barcode = _fixture.Create<string>();
        var parcel = _fixture.Build<Parcel>()
            .With(x => x.Barcode, barcode)
            .Create();

        SetupRepositoryResponse(barcode, parcel);

        // Act
        var result = _sut.Get(barcode);

        // Assert
        _parcelRepositoryMock.Received().TryGetParcel(Arg.Is(barcode), out _);
        result.Should().BeEquivalentTo(
            new GetResponse
            {
                Barcode = barcode,
                Status = parcel.CurrentState.ToString(),
                LaunchDate = parcel.LaunchDate,
                EstimatedArrivalDate = parcel.EstimatedArrivalDate,
                Origin = parcel.Origin,
                Destination = parcel.Destination,
                Sender = parcel.Sender,
                Recipient = parcel.Recipient,
                Contents = parcel.Contents,
                History = parcel.History.Select(x => new HistoryItemResponse
                {
                    Status = x.Status,
                    Timestamp = x.Timestamp
                }).ToArray()
            });
    }

    private void SetupRepositoryResponse(string barcode, Parcel parcel)
    {
        _parcelRepositoryMock.TryGetParcel(Arg.Is(barcode), out Arg.Any<Parcel?>())
            .Returns(r =>
            {
                r[1] = parcel;
                return true;
            });
    }
}