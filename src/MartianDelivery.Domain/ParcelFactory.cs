using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain;

public interface IParcelFactory
{
    public Parcel Create(string barcode,
        string contents,
        string recipient,
        string sender);
}

public class ParcelFactory : IParcelFactory
{
    // TODO Dates and ETA and STatus and History
    public Parcel Create(string barcode,
        string contents,
        string recipient,
        string sender)
    {
        var parcelStateMachine = new ParcelStateMachine(State.Created);
        return new Parcel(parcelStateMachine)
        {
            
            Barcode = barcode,
            Contents = contents,
            Destination = "New London",
            EstimatedArrivalDate = "2025-12-02",
            EtaDays = 90,
            LaunchDate = "2025-09-03",
            Origin = "Starport Thames Estuary",
            Recipient = recipient,
            Sender = sender,
            Status = "Created",
            History =
            [
                new HistoryItem("HELLO","NOW")
            ]
        };
    }
}