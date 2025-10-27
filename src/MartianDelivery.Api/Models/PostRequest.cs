using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MartianDelivery.Models;

public class PostRequest
{
    [RegularExpression("RMARS\\d{19}[A-Z]", ErrorMessage = "Barcode not in correct format.")]
    public required string Barcode { get; set; }
    public required string Sender { get; set; }
    public required string Recipient { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter<Service>))]
    public required Service DeliveryService { get; set; }
    public required string Contents { get; set; }
}

public enum Service
{
    Standard,
    Express
}