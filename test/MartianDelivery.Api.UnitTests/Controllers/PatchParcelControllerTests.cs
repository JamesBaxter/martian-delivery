using AutoFixture;
using AwesomeAssertions;
using MartianDelivery.Controllers;
using MartianDelivery.Domain;
using MartianDelivery.Domain.StateMachine;
using MartianDelivery.Models;
using MartianDelivery.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MartianDelivery.Api.UnitTests.Controllers;

public class PatchParcelControllerTests
{
    private readonly ILogger<PatchParcelController> _loggerMock = Substitute.For<ILogger<PatchParcelController>>();
    private readonly IParcelRepository _parcelRepositoryMock = Substitute.For<IParcelRepository>();
    private readonly IDateTimeProvider _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
    private readonly Fixture _fixture = new();
    
    [Theory]
    [InlineData(State.Created, NewStatus.OnRocketToMars, State.OnRocketToMars)]
    [InlineData(State.OnRocketToMars, NewStatus.LandedOnMars, State.LandedOnMars)]
    [InlineData(State.OnRocketToMars, NewStatus.Lost,State.Lost)]
    [InlineData(State.LandedOnMars, NewStatus.OutForMartianDelivery,State.OutForMartianDelivery)]
    [InlineData(State.OutForMartianDelivery, NewStatus.Lost,State.Lost)]
    [InlineData(State.OutForMartianDelivery, NewStatus.Delivered,State.Delivered)]
    public void Patch_CorrectTriggerOnParcel(State initialState, NewStatus updateState, State expectedState)
    {
        // Arrange
        _fixture.Register<IParcelStateMachine>(() => new ParcelStateMachine(initialState));
        
        var sut = new PatchParcelController(
            _dateTimeProviderMock,
            _parcelRepositoryMock,
            _loggerMock);
        
        var now = _fixture.Create<DateTime>();
        _dateTimeProviderMock.OffsetNow.Returns(now);
        
        var barcode = _fixture.Create<string>();
        var patchRequest = _fixture.Build<PatchRequest>()
            .With(x => x.NewStatus, updateState)
            .Create();
        
        var parcel = _fixture.Build<Parcel>()
            .With(x => x.Barcode, barcode)
            .Create();
        
        SetupRepositoryResponse(barcode, parcel);
        
        // Act
        var result = sut.Patch(barcode, patchRequest);

        // Assert
        parcel.CurrentState.Should().Be(expectedState);
        
        // hacky assertion to check was called with correct time
        parcel.History.Should().Contain(x => x.Timestamp == now);
        var okResult = result as OkResult;
        okResult!.StatusCode.Should().Be(200);
    }
    
    //Other combinations could and should be tested
    [Fact]
    public void Patch_BadRequestOnBadTransition()
    {
        // Arrange
        _fixture.Register<IParcelStateMachine>(() => new ParcelStateMachine(State.Created));
        
        var sut = new PatchParcelController(
            _dateTimeProviderMock,
            _parcelRepositoryMock,
            _loggerMock);
        
        var now = _fixture.Create<DateTime>();
        _dateTimeProviderMock.OffsetNow.Returns(now);
        
        var barcode = _fixture.Create<string>();
        var patchRequest = _fixture.Build<PatchRequest>()
            .With(x => x.NewStatus, NewStatus.Delivered)
            .Create();
        
        var parcel = _fixture.Build<Parcel>()
            .With(x => x.Barcode, barcode)
            .Create();
        
        SetupRepositoryResponse(barcode, parcel);
        
        // Act
        var result = sut.Patch(barcode, patchRequest);

        // Assert
        var badRequest = result as BadRequestObjectResult;
        badRequest!.StatusCode.Should().Be(400);
        parcel.CurrentState.Should().Be(State.Created);
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