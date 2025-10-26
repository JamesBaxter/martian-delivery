using MartianDelivery.Models;

namespace MartianDelivery;

public interface IParcelCreateCommandMapper
{
    public CreateParcelCommand Map(PostRequest postRequest);
}

public class ParcelCreateCommandMapper : IParcelCreateCommandMapper
{
    public CreateParcelCommand Map(PostRequest postRequest)
    {
        DeliveryService commandService = postRequest.DeliveryService switch
        {
            Service.Standard => DeliveryService.Standard,
            Service.Express => DeliveryService.Express,
            _ => throw new ArgumentOutOfRangeException()
        };

        return new CreateParcelCommand
        {
            Barcode = postRequest.Barcode,
            Sender = postRequest.Sender,
            Recipient = postRequest.Recipient,
            Contents = postRequest.Contents,
            Service = commandService
        };
    }
}

public record CreateParcelCommand
{
    public required string Barcode { get; set; }
    public required string Sender { get; set; }
    public required string Recipient { get; set; }
    public required string Contents { get; set; }
    public required DeliveryService Service { get; set; }
}

public enum DeliveryService
{
    Standard,
    Express
}