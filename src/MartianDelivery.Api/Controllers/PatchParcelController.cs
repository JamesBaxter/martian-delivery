using MartianDelivery.Domain;
using MartianDelivery.Models;
using MartianDelivery.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace MartianDelivery.Controllers;

public class PatchParcelController : ControllerBase
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private ILogger<PatchParcelController> _logger;
    private readonly IParcelRepository _parcelRepository;

    public PatchParcelController(
        IDateTimeProvider dateTimeProvider,
        IParcelRepository parcelRepository,
        ILogger<PatchParcelController> logger)
    {
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        _parcelRepository = parcelRepository;
    }

    [HttpPatch]
    [Route("{barcode}")]
    public IActionResult Patch(
        [FromRoute] string barcode,
        [FromBody] PatchRequest patchRequest)
    {
        _logger.LogInformation("Received Create request for Barcode: {barcode}", barcode);
        var found = _parcelRepository.TryGetParcel(barcode, out var parcel);

        if (found && parcel != null)
        {
            // This should be extracted, ran out of time
            // Swallowing exceptions done for speed, far from ideal
            switch (patchRequest.NewStatus)
            {
                case NewStatus.OnRocketToMars:
                    try
                    {
                        parcel.Launch(_dateTimeProvider.OffsetNow);
                    }
                    catch (InvalidOperationException)
                    {
                        return BadRequest(
                            $"State transition from: {parcel.CurrentState} to: {patchRequest.NewStatus} now allowed.");
                    }

                    break;
                case NewStatus.LandedOnMars:
                    try
                    {
                        parcel.Land(_dateTimeProvider.OffsetNow);
                    }
                    catch (InvalidOperationException)
                    {
                        return BadRequest(
                            $"State transition from: {parcel.CurrentState} to: {patchRequest.NewStatus} now allowed.");
                    }

                    break;
                case NewStatus.Lost:
                    try
                    {
                        parcel.Lose(_dateTimeProvider.OffsetNow);
                    }
                    catch (InvalidOperationException)
                    {
                        return BadRequest(
                            $"State transition from: {parcel.CurrentState} to: {patchRequest.NewStatus} now allowed.");
                    }

                    break;
                case NewStatus.OutForMartianDelivery:
                    try
                    {
                        parcel.Dispatch(_dateTimeProvider.OffsetNow);
                    }
                    catch (InvalidOperationException)
                    {
                        return BadRequest(
                            $"State transition from: {parcel.CurrentState} to: {patchRequest.NewStatus} now allowed.");
                    }

                    break;
                case NewStatus.Delivered:
                    try
                    {
                        parcel.Deliver(_dateTimeProvider.OffsetNow);
                    }
                    catch (InvalidOperationException)
                    {
                        return BadRequest(
                            $"State transition from: {parcel.CurrentState} to: {patchRequest.NewStatus} now allowed.");
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Ok();
        }

        throw new NotImplementedException();
    }
}