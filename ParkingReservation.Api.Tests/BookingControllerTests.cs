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
    public class BookingControllerTests
    {
        private ILogger<BookingController> _mockLogger;
        private IParkingService _parkingService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = Mock.Of<ILogger<BookingController>>();
            var availabilityService = new AvailabilityService();
            var bookingService = new BookingService(TestBookableItems.Items);
            var pricingService = new PricingService(TestPricingRules.PriceRules);
            _parkingService = new ParkingService(availabilityService, bookingService, pricingService);
        }

        [Test]
        public async Task GetAvailability_WithExistingMatchingBooking_Returns9Spaces()
        {
            var dateRange = TestBookingDates.Jan1To8_1300_1300;
;
            var expectedStatusCode = StatusCodes.Status201Created;

            var domainDateRange = dateRange.MapToDateRange();
            var controller = new BookingController(_mockLogger, _parkingService);
            var actionResult = await controller.CreateReservationAsync(domainDateRange);

            var contentResult = actionResult as CreatedResult;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatusCode));

                var content = contentResult?.Value as ReservationResponse;
                Assert.That(content?.Reference, !Is.EqualTo(string.Empty));
            });
        }

        [Test]
        public async Task CancelBooking_WithExistingBooking_Cancels()
        {
            var expectedStatusCode = StatusCodes.Status200OK;

            var dateRange = TestBookingDates.Jan1To8_1300_1300;

            var domainDateRange = dateRange.MapToDateRange();
            var controller = new BookingController(_mockLogger, _parkingService);
            var actionResult = await controller.CreateReservationAsync(domainDateRange);

            var contentResult = actionResult as CreatedResult;

            var reservation = contentResult?.Value as ReservationResponse;
            Assert.That(reservation, !Is.Null);

            actionResult = await controller.CancelReservationAsync(reservation.Reference);

            var cancelContentResult = actionResult as OkResult;
            Assert.That(cancelContentResult?.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        [Test]
        public async Task CancelBooking_WithNonExistingBooking_ReturnsException()
        {
            var bookingReference = "abc";
           
            var expectedStatusCode = StatusCodes.Status404NotFound;

            var controller = new BookingController(_mockLogger, _parkingService);

            var actionResult = await controller.CancelReservationAsync(bookingReference);

            var cancelContentResult = actionResult as NotFoundObjectResult;
            Assert.That(cancelContentResult?.StatusCode, Is.EqualTo(expectedStatusCode));
        }
    }
}