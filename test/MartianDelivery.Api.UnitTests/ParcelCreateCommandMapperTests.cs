using AutoFixture;
using AwesomeAssertions;
using MartianDelivery.Models;

namespace MartianDelivery.Api.UnitTests;

public class ParcelCreateCommandMapperTests
{
    private readonly Fixture _fixture = new();
    private readonly ParcelCreateCommandMapper _sut = new();
    
    [Fact]
    public void CreateParcelCommand_MapsProperties()
    {
        // Arrange
        var request = _fixture.Create<PostRequest>();
        
        // Act
        var result = _sut.Map(request);

        // Assert
        result.Should().BeEquivalentTo(new CreateParcelCommand
        {
            Barcode = result.Barcode,
            Sender = request.Sender,
            Contents = request.Contents,
            Recipient = request.Recipient,
            Service = result.Service == DeliveryService.Express ? DeliveryService.Express : DeliveryService.Standard
        });
    }
}