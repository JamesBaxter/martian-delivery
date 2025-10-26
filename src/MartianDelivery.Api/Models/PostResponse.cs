namespace MartianDelivery.Models;

public record PostResponse
{
    public required string Barcode { get; set; }
    public required string Status { get; set; }
    public required string LaunchDate { get; set; }
    public required int EtaDays { get; set; }
    public required string EstimatedArrivalDate { get; set; }
    public required string Origin { get; set; }
    public required string Destination { get; set; }
    public required string Sender { get; set; }
    public required string Recipient { get; set; }
    public required string Contents { get; set; }
}