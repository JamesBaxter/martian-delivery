using System.Text.Json.Serialization;

namespace MartianDelivery.Models;

public class PatchRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter<NewStatus>))]
    public NewStatus NewStatus { get; set; }
}

public enum NewStatus
{
    OnRocketToMars,
    LandedOnMars,
    Lost,
    OutForMartianDelivery,
    Delivered
}