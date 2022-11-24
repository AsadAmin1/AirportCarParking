using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingReservation.Api.Extensions;
using ParkingReservation.Core;
using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using System.Threading.Tasks;
using ParkingReservation.Api.ApiModels;

namespace ParkingReservation.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        #region Private Fields

        private readonly ILogger<AvailabilityController> _logger;
        private readonly IParkingService _parkingService;

        #region

        #region Constructors

        public AvailabilityController(ILogger<AvailabilityController> logger, IParkingService parkingService)
        {
            _logger = logger;
            _parkingService = parkingService;
        }

        #endregion

        #endregion Public Methods

        [HttpGet]
        public async Task<IActionResult> GetAvailability([FromQuery]DateRange dateRange)
        {
            var domainDateRange = new Core.Models.DateRange(dateRange.StartTime, dateRange.EndTime);
            try
            {
                var availability = await _parkingService.GetAvailabilityAsync(domainDateRange);
                return Ok(availability.AsAvailabilityResponse());
            }catch(Exception ex) {
                if (ex is ElapsedDateException || ex is InvalidDatesException)
                {
                    return BadRequest(new AvailabilityResponse { Error = new ErrorDetails(ex.Message) });
                }

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] DateRange dateRange)
        {
            var domainDateRange = new Core.Models.DateRange(dateRange.StartTime, dateRange.EndTime);
            try
            {
                var availability = await _parkingService.GetAvailabilityAsync(domainDateRange);
                if (availability.Spaces > 0)
                {
                    var reservation = await _parkingService.AddReservationAsync(domainDateRange);
                    if (reservation != null)
                    {
                        return new CreatedResult($"parkingreservation/bookings/{reservation.Reference}", reservation.MapToReservationResponse());
                    }
                }
                return new StatusCodeResult(StatusCodes.Status424FailedDependency);
            }
            catch (Exception ex)
            {
                if (ex is ElapsedDateException || ex is InvalidDatesException)
                {
                    return BadRequest(new AvailabilityResponse { Error = new ErrorDetails(ex.Message) });
                }

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
