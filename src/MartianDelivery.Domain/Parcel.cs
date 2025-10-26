using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain;

public class Parcel
{
    private readonly IParcelStateMachine _stateMachine;

    public Parcel(IParcelStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        History =
        [
            new HistoryItem(nameof(State.Created),"Now")
        ];
    }
    
    public string Barcode { get; set; }
    //TODO Could Delete Status or replace with State?
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

    public void Launch()
    {
        TriggerAndSetHistory(Trigger.Launch);
    }

    public void Lose()
    {
        TriggerAndSetHistory(Trigger.Lose);
    }

    public void Land()
    {
        TriggerAndSetHistory(Trigger.Land);
    }

    public void Dispatch()
    {
        TriggerAndSetHistory(Trigger.Dispatch);
    }

    public void Deliver()
    {
        TriggerAndSetHistory(Trigger.Deliver);
    }
    
    private void TriggerAndSetHistory(Trigger trigger)
    {
        _stateMachine.Fire(trigger);
        var historyItem = new HistoryItem(_stateMachine.CurrentState().ToString(),"Now");
        History = History.Append(historyItem).ToArray();
    }
}

public class HistoryItem
{
    public HistoryItem(string status, string timestamp)
    {
        Status = status;
        Timestamp = timestamp;
    }
    public string Status { get; set; }
    public string Timestamp { get; set; }
}