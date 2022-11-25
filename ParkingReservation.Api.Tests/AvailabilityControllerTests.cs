using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ParkingReservation.Api.v1.Controllers;
using Microsoft.AspNetCore.Http;
using ParkingReservation.Core;
using ParkingReservation.Core.TestHelpers;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Api.ApiModels;
using ParkingReservation.Api.Extensions;

namespace ParkingReservation.Api.Tests
{
    public class AvailabilityControllerTests
    {
        private readonly int totalCapacity = 10;

        private ILogger<AvailabilityController> _mockLogger;
        private IParkingService _parkingService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = Mock.Of<ILogger<AvailabilityController>>();
            var availabilityService = new AvailabilityService();
            var bookingService = new BookingService(TestBookableItems.Items);
            var pricingService = new PricingService(TestPricingRules.PriceRules);
            _parkingService = new ParkingService(availabilityService, bookingService, pricingService);
        }

        [Test]
        public async Task GetAvailability_WithNoBookings_Returns10Spaces()
        {
            var dateRange = TestBookingDates.Jan1To8_1300_1300;
            
            var expected = totalCapacity;
            var expectedPrice = 168m;

            var domainDateRange = dateRange.MapToDateRange();
            var controller = new AvailabilityController(_mockLogger, _parkingService);
            var actionResult = await controller.GetAvailability(domainDateRange);

            var contentResult = actionResult as OkObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult, !Is.Null);
                if (contentResult != null)
                {
                    Assert.That(contentResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                    var content = contentResult.Value as AvailabilityResponse;
                    Assert.That(content?.Spaces, Is.EqualTo(expected));
                    Assert.That(content?.Price, Is.EqualTo(expectedPrice));
                }
            });
        }

        [Test]
        public async Task GetAvailability_WithInvalidDates_ThrowsInvalidDatesException()
        {
            var dateRange = TestBookingDates.InvalidDates;

            var expected = "Start Date must be after than the End Date.";
            var expectedStatusCode = StatusCodes.Status400BadRequest;

            var domainDateRange = dateRange.MapToDateRange();
            var controller = new AvailabilityController(_mockLogger, _parkingService);
            var actionResult = await controller.GetAvailability(domainDateRange);

            var contentResult = actionResult as BadRequestObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult, !Is.Null);
                Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatusCode));

                var content = contentResult?.Value as AvailabilityResponse;
                Assert.That(content?.Error?.Description, Is.EqualTo(expected));
            });
        }
    }
}