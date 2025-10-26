using Stateless;

namespace MartianDelivery.Domain.StateMachine;

public interface IParcelStateMachine
{
    public void Fire(Trigger trigger);
    public State CurrentState();
}

public class ParcelStateMachine: IParcelStateMachine
{
    private readonly StateMachine<State, Trigger> _stateMachine;

    public ParcelStateMachine(State initialState)
    {
        _stateMachine = new StateMachine<State, Trigger>(initialState);

        _stateMachine.Configure(State.Created)
            .Permit(Trigger.Launch, State.OnRocketToMars);

        _stateMachine.Configure(State.OnRocketToMars)
            .Permit(Trigger.Land, State.LandedOnMars)
            .Permit(Trigger.Lose, State.Lost);

        _stateMachine.Configure(State.LandedOnMars)
            .Permit(Trigger.Dispatch, State.OutForMartianDelivery);

        _stateMachine.Configure(State.OutForMartianDelivery)
            .Permit(Trigger.Deliver, State.Delivered)
            .Permit(Trigger.Lose, State.Lost);
    }

    public void Fire(Trigger trigger)
    {
        _stateMachine.Fire(trigger);
    }

    public State CurrentState() => _stateMachine.State;
}