using MartianDelivery.Models;
using MartianDelivery.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace MartianDelivery.Controllers;

[ApiController]
[Route("parcel")]
public class GetParcelController : ControllerBase
{
    private readonly IParcelRepository _parcelRepository;
    private readonly ILogger<GetParcelController> _logger;

    public GetParcelController(
        IParcelRepository parcelRepository,
        ILogger<GetParcelController> logger)
    {
        _parcelRepository = parcelRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route("{barcode}")]
    public GetResponse Get(string barcode)
    {
        var found = _parcelRepository.TryGetParcel(barcode, out var parcel);
        
        if (found && parcel != null)
        {
            _logger.LogInformation("Found Parcel for barcode,{barcode}", barcode);
            var getResponse = new GetResponse
            {
                Barcode = parcel.Barcode,
                Status = parcel.CurrentState.ToString(),
                LaunchDate = parcel.LaunchDate,
                EstimatedArrivalDate = parcel.EstimatedArrivalDate,
                Origin = parcel.Origin,
                Destination = parcel.Destination,
                Sender = parcel.Sender,
                Recipient = parcel.Recipient,
                Contents = parcel.Contents,
                History = parcel.History.Select(h => new HistoryItemResponse
                {
                    Status = h.Status,
                    Timestamp = h.Timestamp
                }).ToArray()
            };
            return getResponse;
        }

        throw new NotImplementedException();
    }
}