namespace MartianDelivery.Models;

public record GetResponse
{
    public required string Barcode { get; set; }
    public required string Status { get; set; }
    public required string LaunchDate { get; set; }
    public required string EstimatedArrivalDate { get; set; }
    public required string Origin { get; set; }
    public required string Destination { get; set; }
    public required string Sender { get; set; }
    public required string Recipient { get; set; }
    public required string Contents { get; set; }
    public required HistoryItemResponse[] History { get; set; }
}