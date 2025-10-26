namespace MartianDelivery.Models;

public record PostRequest
{
    public required string Barcode { get; set; }
    public required string Sender { get; set; }
    public required string Recipient { get; set; }
    public required Service DeliveryService { get; set; }
    public required string Contents { get; set; }
}

public enum Service
{
    Standard,
    Express
}