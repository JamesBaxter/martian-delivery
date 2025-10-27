using MartianDelivery.Domain.StateMachine;

namespace MartianDelivery.Domain;

public class Parcel
{
    private readonly IParcelStateMachine _stateMachine;

    public Parcel(IParcelStateMachine stateMachine, DateTimeOffset timeStamp)
    {
        _stateMachine = stateMachine;
        History =
        [
            new HistoryItem(nameof(State.Created), timeStamp)
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

    public void Launch(DateTimeOffset timeStamp)
    {
        TriggerAndSetHistory(Trigger.Launch, timeStamp);
    }

    public void Lose(DateTimeOffset timeStamp)
    {
        TriggerAndSetHistory(Trigger.Lose, timeStamp);
    }

    public void Land(DateTimeOffset timeStamp)
    {
        TriggerAndSetHistory(Trigger.Land, timeStamp);
    }

    public void Dispatch(DateTimeOffset timeStamp)
    {
        TriggerAndSetHistory(Trigger.Dispatch, timeStamp);
    }

    public void Deliver(DateTimeOffset timeStamp)
    {
        TriggerAndSetHistory(Trigger.Deliver, timeStamp);
    }
    
    private void TriggerAndSetHistory(Trigger trigger, DateTimeOffset timeStamp)
    {
        _stateMachine.Fire(trigger);
        var historyItem = new HistoryItem(_stateMachine.CurrentState().ToString(), timeStamp);
        History = History.Append(historyItem).ToArray();
    }
}

public class HistoryItem
{
    public HistoryItem(string status, DateTimeOffset timestamp)
    {
        Status = status;
        Timestamp = timestamp;
    }
    public string Status { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}