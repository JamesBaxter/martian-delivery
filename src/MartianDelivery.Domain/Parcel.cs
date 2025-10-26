namespace MartianDelivery.Domain;

public class Parcel
{
    public string Barcode { get; set; }
    public string Status { get; set; }
    public string LaunchDate { get; set; }
    public int EtaDays { get; set; }
    public string EstimatedArrivalDate { get; set; }
    public string Origin { get; set; } = "Starport Thames Estuary";
    public string Destination { get; set; } = "New London";
    public string Sender { get; set; }
    public string Recipient { get; set; }
    public string Contents { get; set; }
    public HistoryItem[] History { get; set; }
}

public class HistoryItem
{
    public string Status { get; set; }
    public string Timestamp { get; set; }
}