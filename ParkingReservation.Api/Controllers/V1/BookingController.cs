using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using System.Threading.Tasks;
using ParkingReservation.Api.ApiModels;
using ParkingReservation.Api.Extensions;

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

        #region

        #region Constructors

        public BookingController(ILogger<BookingController> logger, IParkingService parkingService) { 
            _logger = logger;
            _parkingService = parkingService;
        }

        #endregion

        #endregion Public Methods

        [HttpPost]
        public async Task<IActionResult> CreateReservationAsync([FromBody]DateRange dateRange)
        {
            var domainDateRange = new Core.Models.DateRange(dateRange.StartTime, dateRange.EndTime);

            try { 
                var reservationResult = await _parkingService.AddReservationAsync(domainDateRange);
                if (reservationResult != null)
                {
                    var reservation = reservationResult.MapToReservationResponse();
                    return new CreatedResult($"parkingreservation/bookings/{reservation.Reference}", reservation);
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
