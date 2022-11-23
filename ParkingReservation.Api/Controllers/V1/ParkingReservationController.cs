using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingReservation.Core;
using ParkingReservation.Core.Models;
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
        private readonly AvailabilityService _availabilityService;

        #region

        #region Constructors

        public ParkingReservationController(ILogger<ParkingReservationController> logger, AvailabilityService availabilityService)
        {
            _logger = logger;
            _availabilityService = availabilityService;
        }

        #endregion

        #endregion Public Methods

        [HttpGet]
        [ApiVersion("1.0")]
        public async Task<IActionResult> GetAvailability(DateRange dateRange)
        {
            return Ok(await _availabilityService.GetAvailability(dateRange));
        }

        #endregion
    }
}
