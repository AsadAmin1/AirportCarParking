using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using System.Threading.Tasks;
using ParkingReservation.Api.ApiModels;
using ParkingReservation.Api.Extensions;
using System.Reflection;

namespace ParkingReservation.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class BookingController : ControllerBase
    {
        #region Private Fields

        private readonly ILogger<BookingController> _logger;
        private readonly IParkingService _parkingService;
        private readonly string _messageFormat = $"{0}-{1}";

        #endregion

        #region Constructors

        public BookingController(ILogger<BookingController> logger, IParkingService parkingService) { 
            _logger = logger;
            _parkingService = parkingService;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        public async Task<IActionResult> CreateReservationAsync([FromBody]DateRange dateRange)
        {
            using (_logger.BeginScope(_messageFormat, GetType().Name, MethodBase.GetCurrentMethod().Name))
            {
                var domainDateRange = new Core.Models.DateRange(dateRange.StartTime, dateRange.EndTime);

                try
                {
                    var reservationResult = await _parkingService.AddReservationAsync(domainDateRange);
                    if (reservationResult != null)
                    {
                        var reservation = reservationResult.MapToReservationResponse();
                        return new CreatedResult($"parkingreservation/bookings/{reservation.Reference}", reservation);
                    }

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                catch (Exception ex)
                {
                    if (ex is NoAvailabilityException)
                    {
                        var response = new ReservationResponse { Error = ErrorDetails.New(ex.Message) };
                        return new NotFoundObjectResult(response);
                    }

                    return new StatusCodeResult(StatusCodes.Status424FailedDependency);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> CancelReservationAsync([FromQuery] string bookingReference)
        {
            using (_logger.BeginScope(_messageFormat, GetType().Name, MethodBase.GetCurrentMethod().Name))
            {
                try
                {
                    await _parkingService.CancelReservationAsync(bookingReference);
                    return Ok();
                }
                catch (Exception ex)
                {
                    if (ex is BookingNotFoundException)
                    {
                        var response = new ReservationResponse { Error = ErrorDetails.New(ex.Message) };
                        return new NotFoundObjectResult(response);
                    }

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPatch]
        public async Task<IActionResult> AmendReservationAsync([FromBody] AmendReservationRequest amendReservationRequest)
        {
            using (_logger.BeginScope(_messageFormat, GetType().Name, MethodBase.GetCurrentMethod().Name))
            {
                try
                {
                    var reservation = await _parkingService.AmendReservationAsync(amendReservationRequest.MapToDomainModel());
                    return Ok(reservation.MapToReservationResponse());
                }
                catch (Exception ex)
                {
                    if (ex is NoAvailabilityException) {
                        var response = new ReservationResponse { Error = ErrorDetails.New(ex.Message) };
                        return new NotFoundObjectResult(response);
                    }

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        #endregion
    }
}
