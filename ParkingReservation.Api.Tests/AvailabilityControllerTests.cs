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
using ParkingReservation.Api.Models;

namespace ParkingReservation.Api.Tests
{
    public class AvailabilityControllerTests
    {
        private readonly int totalCapacity = 10;

        private ILogger<AvailabilityController> _mockLogger;
        private IParkingService _parkingService;
        private PricingConfig _pricingConfig;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestConfiguration.Setup();
            _pricingConfig = new PricingConfig(TestConfiguration.Get);
        }

        [SetUp]
        public void Setup()
        {
            _mockLogger = Mock.Of<ILogger<AvailabilityController>>();
            var availabilityService = new AvailabilityService();
            var bookingService = new BookingService(TestBookableItems.Items);
            var pricingService = new PricingService(TestPricingRules.PriceRules, _pricingConfig);
            _parkingService = new ParkingService(availabilityService, bookingService, pricingService);
        }

        [Test]
        public async Task GetAvailability_WithNoBookings_Returns10Spaces()
        {
            var expected = totalCapacity;
            var expectedPrice = 168m;

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var domainDateRange = dateRange.MapToDateRange();
            var controller = new AvailabilityController(_mockLogger, _parkingService);
            var actionResult = await controller.GetAvailabilityAsync(domainDateRange);

            var contentResult = actionResult as OkObjectResult;
            var availabilityResponse = contentResult?.Value as AvailabilityResponse;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult?.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                Assert.That(availabilityResponse?.Spaces, Is.EqualTo(expected));
                Assert.That(availabilityResponse?.Price, Is.EqualTo(expectedPrice));
            });
        }

        [Test]
        public async Task GetAvailability_WithElapsedDate_ThrowsElapsedDateException()
        {
            var expectedMessage = "Start Date can not be in the past.";
            var expectedStatus = StatusCodes.Status400BadRequest;

            var dateRange = TestBookingDates.ElapsedDate;

            var controller = new AvailabilityController(_mockLogger, _parkingService);
            var actionResult = await controller.GetAvailabilityAsync(dateRange.MapToDateRange());

            var contentResult = actionResult as BadRequestObjectResult;
            var availabilityResponse = contentResult?.Value as AvailabilityResponse;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatus));
                Assert.That(availabilityResponse?.Error?.Description, Is.EqualTo(expectedMessage));
            });
        }

        [Test]
        public async Task GetAvailability_WithInvalidDates_ThrowsInvalidDatesException()
        {
            var expected = "Start Date must be after than the End Date.";
            var expectedStatusCode = StatusCodes.Status400BadRequest;
            
            var dateRange = TestBookingDates.InvalidDates;

            var domainDateRange = dateRange.MapToDateRange();
            var controller = new AvailabilityController(_mockLogger, _parkingService);
            var actionResult = await controller.GetAvailabilityAsync(domainDateRange);

            var contentResult = actionResult as BadRequestObjectResult;
            var availabilityResponse = contentResult?.Value as AvailabilityResponse;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatusCode));
                Assert.That(availabilityResponse?.Error?.Description, Is.EqualTo(expected));
            });
        }

        [Test]
        public async Task GetAvailability_WithExistingMatchingBooking_Returns9Spaces()
        {
            var expectedSpaces = 9;
            var expectedStatus = StatusCodes.Status200OK;
            var expectedDateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            await _parkingService.AddReservationAsync(dateRange);

            var controller = new AvailabilityController(_mockLogger, _parkingService);
            var actionResults = await controller.GetAvailabilityAsync(dateRange.MapToDateRange());

            var contentResult = actionResults as OkObjectResult;
            var availabilityResponse = contentResult?.Value as AvailabilityResponse;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatus));
                Assert.That(availabilityResponse?.Spaces, Is.EqualTo(expectedSpaces));
            });
        }

        [Test]
        public async Task GetAvailability_WithExistingNotMatchingBooking_Returns10Spaces()
        {
            var expected = 10;
            var expectedStatusCode = StatusCodes.Status200OK;

            var firstWeek = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var secondWeek = TestBookingDates.WinterDates.SecondWeek1PMto1PM;

            await _parkingService.AddReservationAsync(firstWeek);

            var controller = new AvailabilityController(_mockLogger, _parkingService);
            var actionResult = await controller.GetAvailabilityAsync(secondWeek.MapToDateRange());

            var contentResult = actionResult as OkObjectResult;
            var availabilityResponse = contentResult?.Value as AvailabilityResponse;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatusCode));
                Assert.That(availabilityResponse?.Spaces, Is.EqualTo(expected));
            });
        }
    }
}