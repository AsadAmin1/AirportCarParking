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
using ParkingReservation.Core.Exceptions;

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
            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
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
        public async Task CancelBooking_WithExistingBooking_ReturnsOk()
        {
            var expectedStatusCode = StatusCodes.Status200OK;

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

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
        public async Task CancelBooking_WithNonExistingBooking_ReturnsNotFound()
        {
            var bookingReference = "abc";
           
            var expectedStatusCode = StatusCodes.Status404NotFound;

            var controller = new BookingController(_mockLogger, _parkingService);

            var actionResult = await controller.CancelReservationAsync(bookingReference);

            var cancelContentResult = actionResult as NotFoundObjectResult;
            Assert.That(cancelContentResult?.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        [Test]
        public async Task AmendBooking_WithExistingBookingWithAvailableDates_ReturnsTrue()
        {
            var expectedStatusCode = StatusCodes.Status200OK;

            var dateRange1 = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var dateRange2 = TestBookingDates.WinterDates.SecondWeek1PMto1PM;

            var initialDates = dateRange1.MapToDateRange();
            var newDates = dateRange2.MapToDateRange();

            var controller = new BookingController(_mockLogger, _parkingService);
            var actionResult = await controller.CreateReservationAsync(initialDates);

            var contentResult = actionResult as CreatedResult;

            var reservation = contentResult?.Value as ReservationResponse;
            Assert.That(reservation, !Is.Null);

            var initialBookingReference = reservation.Reference;

            var amendReservationRequest = new AmendReservationRequest
            {
                BookingReference = initialBookingReference,
                DateRange = newDates,
            };

            var amendActionResult = await controller.AmendReservationAsync(amendReservationRequest);
            
            var amendContentResult = amendActionResult as OkObjectResult;
            Assert.That(amendContentResult, !Is.Null);

            var amendedReservation = amendContentResult.Value as ReservationResponse;

            Assert.Multiple(() =>
            {
                Assert.That(amendedReservation, !Is.Null);

                Assert.That(amendContentResult.StatusCode, Is.EqualTo(expectedStatusCode));
                Assert.That(reservation.Reference, Is.EqualTo(initialBookingReference));
                Assert.That(amendedReservation?.DateRange.StartTime, Is.EqualTo(newDates.StartTime));
            });
        }

        [Test]
        public async Task AmendBooking_WithExistingBookingNoAvailabiltity_ThrowsException()
        {
            var expectedMessage = "Unfortunately there is no availability for the request date range.";

            var dateRange1 = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var dateRange2 = TestBookingDates.WinterDates.SecondWeek1PMto1PM;

            var initialDates = dateRange1;

            var newDates = initialDates;

            var initialReservation = await _parkingService.AddReservationAsync(initialDates);

            await CreateMultipleReservations(newDates, _parkingService);

            var bookingController = new BookingController(_mockLogger, _parkingService);

            var amendReservationRequest = new AmendReservationRequest
            {
                BookingReference = initialReservation.Reference,
                DateRange = newDates.MapToDateRange(),
            };

            var ex = Assert.ThrowsAsync<NoAvailabilityException>(async () =>
            {
                await bookingController.AmendReservationAsync(amendReservationRequest);
            });

            Assert.Multiple(() =>
            {
                Assert.That(ex.Message, Is.EqualTo(expectedMessage));
            });
        }

        private static async Task CreateMultipleReservations(Core.Models.DateRange initialDates, IParkingService parkingService)
        {
            var tasks = new List<Task>();
            for (int i = 1; i < 10; i++)
            {
                tasks.Add(parkingService.AddReservationAsync(initialDates));
            }
            await Task.WhenAll(tasks);
        }
    }
}