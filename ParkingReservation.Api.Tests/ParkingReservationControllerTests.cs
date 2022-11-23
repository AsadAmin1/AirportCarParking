using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ParkingReservation.Api.v1.Controllers;
using Microsoft.AspNetCore.Http;
using ParkingReservation.Core;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Api.Tests
{
    public class ParkingReservationControllerTests
    {
        private readonly int totalCapacity = 10;

        private ILogger<ParkingReservationController> _mockLogger;
        private AvailabilityService _availabilityService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = Mock.Of<ILogger<ParkingReservationController>>();
            _availabilityService = new AvailabilityService(totalCapacity);
        }

        [Test]
        public async Task GetAvailability_WithNoBookings_Returns10Spaces()
        {
            var startTime = new DateTime(2023, 1, 2, 13, 0, 0);
            var endTime = new DateTime(2023, 1, 9, 13, 0, 0);

            var dateRange = new DateRange(startTime, endTime);

            var controller = new ParkingReservationController(_mockLogger, _availabilityService);
            var actionResult = await controller.GetAvailability(dateRange);

            var contentResult = actionResult as OkObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult, !Is.Null);
                if (contentResult != null)
                {
                    Assert.That(contentResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                    Assert.That(contentResult.Value, Is.TypeOf<int>());
                    Assert.That(contentResult.Value, Is.EqualTo(totalCapacity));
                }
            });
        }
    }
}