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

public class PostParcelControllerTests
{
    private readonly ILogger<GetParcelController> _loggerMock = Substitute.For<ILogger<GetParcelController>>();
    private readonly IParcelRepository _parcelRepositoryMock = Substitute.For<IParcelRepository>();
    private readonly IParcelCreateCommandMapper _parcelCreateCreateCommandMapper = Substitute.For<IParcelCreateCommandMapper>();
    private readonly IParcelFactory _parcelFactoryMock = Substitute.For<IParcelFactory>();
    private readonly Fixture _fixture = new();
    
    private readonly PostParcelController _sut;
    
    public PostParcelControllerTests()
    {
        _fixture.Register<IParcelStateMachine>(() => new ParcelStateMachine(State.Created));
        _sut = new PostParcelController(
            _parcelRepositoryMock,
            _parcelCreateCreateCommandMapper,
            _parcelFactoryMock,
            _loggerMock);
    }
    
    [Fact]
    public void Post_Calls_CommandMapper_Factory_And_ParcelRepository_ReturnsCreatedParcel()
    {
        // Arrange
        var postRequest = _fixture.Create<PostRequest>();
        var createParcelCommand = _fixture.Create<CreateParcelCommand>();
        
        _parcelCreateCreateCommandMapper
            .Map(Arg.Is(postRequest))
            .Returns(createParcelCommand);

        var parcel = _fixture.Create<Parcel>();
        _parcelFactoryMock
            .Create(Arg.Is(createParcelCommand.Barcode),
                    Arg.Is(createParcelCommand.Contents),
                    Arg.Is(createParcelCommand.Recipient),
                    Arg.Is(createParcelCommand.Sender))
            .Returns(parcel);
        
        SetupRepositoryResponse(parcel);
        
        // Act
        var result = _sut.Post(postRequest);
        
        // Assert
        _parcelCreateCreateCommandMapper.Received().Map(postRequest);
        _parcelFactoryMock.Received()
            .Create(Arg.Is(createParcelCommand.Barcode),
                Arg.Is(createParcelCommand.Contents),
                Arg.Is(createParcelCommand.Recipient),
                Arg.Is(createParcelCommand.Sender));
        _parcelRepositoryMock.Received().TryAddParcel(Arg.Is(parcel));
        result.Should().BeEquivalentTo(new PostResponse
        {
            Barcode = parcel.Barcode,
            Status = parcel.Status,
            LaunchDate = parcel.LaunchDate,
            EtaDays = parcel.EtaDays,
            EstimatedArrivalDate = parcel.EstimatedArrivalDate,
            Origin = parcel.Origin,
            Destination = parcel.Destination,
            Sender = parcel.Sender,
            Recipient = parcel.Recipient,
            Contents = parcel.Contents
        });
    }

    private void SetupRepositoryResponse(Parcel parcel)
    {
        _parcelRepositoryMock.TryAddParcel(Arg.Is(parcel))
            .Returns(true);
    }
}