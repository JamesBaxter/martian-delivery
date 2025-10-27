using MartianDelivery.Domain;
using MartianDelivery.Models;
using MartianDelivery.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace MartianDelivery.Controllers;

[ApiController]
[Route("parcel")]
public class PostParcelController : ControllerBase
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IParcelCreateCommandMapper _parcelCreateCommandMapper;
    private readonly IParcelFactory _parcelFactory;
    private readonly IParcelRepository _parcelRepository;
    private readonly ILogger<PostParcelController> _logger;

    public PostParcelController(
        IDateTimeProvider dateTimeProvider,
        IParcelRepository parcelRepository,
        IParcelCreateCommandMapper parcelCreateCommandMapper,
        IParcelFactory parcelFactory,
        ILogger<PostParcelController> logger)
    {
        _dateTimeProvider = dateTimeProvider;
        _parcelRepository = parcelRepository;
        _parcelCreateCommandMapper = parcelCreateCommandMapper;
        _logger = logger;
        _parcelFactory = parcelFactory;
    }

    [HttpPost]
    public IActionResult Post([FromBody] PostRequest postRequest)
    {
        _logger.LogInformation("Received Create request for Barcode: {barcode}", postRequest.Barcode);
        
        var createParcelCommand = _parcelCreateCommandMapper.Map(postRequest);
        var parcel = _parcelFactory.Create(
            barcode: createParcelCommand.Barcode,
            contents: createParcelCommand.Contents,
            recipient: createParcelCommand.Recipient,
            sender: createParcelCommand.Sender,
            timestamp: _dateTimeProvider.OffsetNow
        );

        _parcelRepository.TryAddParcel(parcel);

        return Ok(CreateResponse(parcel));
    }

    private static PostResponse CreateResponse(Parcel parcel)
    {
        return new PostResponse
        {
            Barcode = parcel.Barcode,
            Status = parcel.Status,
            LaunchDate = parcel.LaunchDate,
            EtaDays = parcel.EtaDays,
            EstimatedArrivalDate = parcel.EstimatedArrivalDate,
            Origin = parcel.Origin,
            Destination = parcel.Destination,
            Sender = parcel.Sender,
            Recipient = parcel.Recipient,
            Contents = parcel.Contents
        };
    }
}