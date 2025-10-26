using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain;

public class Parcel
{
    private readonly IParcelStateMachine _stateMachine;

    public Parcel(IParcelStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    
    public string Barcode { get; set; }
    public string Status { get; set; }
    public string LaunchDate { get; set; }
    public int EtaDays { get; set; }
    public string EstimatedArrivalDate { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string Sender { get; set; }
    public string Recipient { get; set; }
    public string Contents { get; set; }
    public HistoryItem[] History { get; set; }

    public State CurrentState => _stateMachine.CurrentState();
}

public class HistoryItem
{
    public string Status { get; set; }
    public string Timestamp { get; set; }
}