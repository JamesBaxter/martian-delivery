using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain;

public interface IParcelFactory
{
    public Parcel Create(string barcode,
        string contents,
        string recipient,
        string sender,
        DateTimeOffset timestamp);
}

public class ParcelFactory : IParcelFactory
{
    // Missing Date and ETA Calculation
    // I would inject some calculators in here they would be passed the Service level Standard or express and return appropriate dates 
    public Parcel Create(string barcode,
        string contents,
        string recipient,
        string sender,
        DateTimeOffset timestamp)
    {
        var parcelStateMachine = new ParcelStateMachine(State.Created);
        return new Parcel(parcelStateMachine, timestamp)
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
            Status = nameof(State.Created),
            History =
            [
                new HistoryItem(nameof(State.Created), timestamp)
            ]
        };
    }
}