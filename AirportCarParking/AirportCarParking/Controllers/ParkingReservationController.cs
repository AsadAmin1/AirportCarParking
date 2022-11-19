using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AirportCarParking.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingReservationController : ControllerBase
    {
        private readonly ILogger<ParkingReservationController> _logger;

        public ParkingReservationController(ILogger<ParkingReservationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello World";
        }
    }
}
