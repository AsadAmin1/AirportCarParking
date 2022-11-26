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
using System.Reflection;

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
        private readonly string _messageFormat = "{0}-{1}";

        #endregion

        #region Constructors

        public AvailabilityController(ILogger<AvailabilityController> logger, IParkingService parkingService)
        {
            _logger = logger;
            _parkingService = parkingService;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public async Task<IActionResult> GetAvailabilityAsync([FromQuery]DateRange dateRange)
        {
            using (_logger.BeginScope(_messageFormat, GetType().Name, MethodBase.GetCurrentMethod().Name))
            {
                var domainDateRange = new Core.Models.DateRange(dateRange.StartTime, dateRange.EndTime);
                try
                {
                    var availability = await _parkingService.GetAvailabilityAsync(domainDateRange);
                    return Ok(availability.AsAvailabilityResponse());
                }
                catch (Exception ex)
                {
                    if (ex is ElapsedDateException || ex is InvalidDatesException || ex is NoAvailabilityException)
                    {
                        return BadRequest(new AvailabilityResponse { Error = ErrorDetails.New(ex.Message) });
                    }

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        #endregion
    }
}
