using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingReservation.Api.Models;
using ParkingReservation.Core.Interfaces;
using System.Threading.Tasks;

namespace ParkingReservation.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class ParkingReservationController : ControllerBase
    {
        #region Private Fields

        private readonly ILogger<ParkingReservationController> _logger;
        private readonly IAvailabilityService _availabilityService;

        #region

        #region Constructors

        public ParkingReservationController(ILogger<ParkingReservationController> logger, IAvailabilityService availabilityService)
        {
            _logger = logger;
            _availabilityService = availabilityService;
        }

        #endregion

        #endregion Public Methods

        [HttpGet]
        public async Task<IActionResult> GetAvailability([FromQuery]DateRange dateRange)
        {
            var domainDateRange = new Core.Models.DateRange(dateRange.StartTime, dateRange.EndTime);
            return Ok(await _availabilityService.GetAvailability(domainDateRange));
        }

        #endregion
    }
}
