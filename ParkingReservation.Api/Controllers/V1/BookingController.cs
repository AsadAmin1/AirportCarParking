using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using System.Threading.Tasks;
using ParkingReservation.Api.ApiModels;

namespace ParkingReservation.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class BookingController : ControllerBase
    {
        #region Private Fields

        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;

        #region

        #region Constructors

        public BookingController(ILogger<BookingController> logger, IBookingService bookingService) { 
            _logger = logger;
            _bookingService = bookingService;
        }

        #endregion

        #endregion Public Methods

        [HttpPost]
        public async Task<IActionResult> CreateReservationAsync([FromBody]DateRange dateRange)
        {
            var domainDateRange = new Core.Models.DateRange(dateRange.StartTime, dateRange.EndTime);

            try { 
                var reservation = await _bookingService.AddReservationAsync(domainDateRange);
                if (reservation != null)
                {
                    var apiRes = new ReservationResponse
                    {
                        DateRange = new DateRange{
                            StartTime = reservation.DateRange.StartTime,
                            EndTime = reservation.DateRange.EndTime
                        },
                        Reference = reservation.Reference,
                        Item = reservation.Item
                    };

                    return new CreatedResult($"parkingreservation/bookings/{reservation.Reference}", apiRes);
                }

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch(Exception ex){
                if (ex is NoAvailabilityException)
                {
                    return new StatusCodeResult(StatusCodes.Status424FailedDependency);
                }

                return new StatusCodeResult(StatusCodes.Status424FailedDependency);
            }
        }

        #endregion
    }
}
