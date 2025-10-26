namespace MartianDelivery.Models;

public record HistoryItemResponse
{
    public required string Status { get; set; }
    public required string Timestamp { get; set; }
}