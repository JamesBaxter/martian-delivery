namespace MartianDelivery.Domain;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTimeOffset OffsetNow { get; }
}

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTimeOffset OffsetNow => DateTimeOffset.Now;
}

