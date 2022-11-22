using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ParkingReservation.Api.v1.Controllers;
using Microsoft.AspNetCore.Http;

namespace ParkingReservation.Api.Tests
{
    public class ParkingReservationControllerTests
    {
        private ILogger<ParkingReservationController> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockLogger = Mock.Of<ILogger<ParkingReservationController>>();
        }

        [Test]
        public async Task Get_Barebone_ReturnsHelloWorld()
        {
            var expected = "Hello World";

            var controller = new ParkingReservationController(_mockLogger);
            var actionResult = await controller.Get();

            var contentResult = actionResult as OkObjectResult;

            Assert.That(contentResult, !Is.Null);
            
            Assert.That(contentResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(contentResult.Value, Is.TypeOf<string>());
            Assert.That(contentResult.Value, Is.EqualTo(expected));
        }
    }
}