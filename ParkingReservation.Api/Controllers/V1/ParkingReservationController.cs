using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ParkingReservation.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
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
