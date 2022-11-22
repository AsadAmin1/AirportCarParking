using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
        [ApiVersion("1.0")]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(Ok("Hello World"));
        }
    }
}
