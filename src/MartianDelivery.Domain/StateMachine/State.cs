namespace MartianDelivery.Domain.StateMachine;

public enum State
{
    Created,
    OnRocketToMars,
    LandedOnMars,
    OutForMartianDelivery,
    Delivered,
    Lost
}