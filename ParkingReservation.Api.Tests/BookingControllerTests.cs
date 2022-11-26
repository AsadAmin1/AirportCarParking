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
        public async Task CreateReservation_WithExistingMatchingBooking_Returns9Spaces()
        {
            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
;
            var expectedStatusCode = StatusCodes.Status201Created;

            var domainDateRange = dateRange.MapToDateRange();
            var controller = new BookingController(_mockLogger, _parkingService);
            var actionResult = await controller.CreateReservationAsync(domainDateRange);

            var contentResult = actionResult as CreatedResult;
            var reservationResponse = contentResult?.Value as ReservationResponse;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatusCode));
                Assert.That(reservationResponse?.Reference, !Is.EqualTo(string.Empty));
            });
        }

        [Test]
        public async Task CancelBooking_WithExistingBooking_ReturnsOk()
        {
            var expectedStatusCode = StatusCodes.Status200OK;

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var reservationResponse = await _parkingService.AddReservationAsync(dateRange);

            var controller = new BookingController(_mockLogger, _parkingService);
            var actionResult = await controller.CancelReservationAsync(reservationResponse.Reference);

            var contentResult = actionResult as OkResult;

            Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        [Test]
        public async Task CancelBooking_WithNonExistingBooking_ReturnsNotFound()
        {         
            var expectedStatusCode = StatusCodes.Status404NotFound;

            var bookingReference = "abc";

            var controller = new BookingController(_mockLogger, _parkingService);
            var actionResult = await controller.CancelReservationAsync(bookingReference);

            var cancelContentResult = actionResult as NotFoundObjectResult;
            Assert.That(cancelContentResult?.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        [Test]
        public async Task AmendBooking_WithExistingBookingWithAvailableDates_ReturnsAmendedReservation()
        {
            var expectedStatusCode = StatusCodes.Status200OK;

            var dateRange1 = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var dateRange2 = TestBookingDates.WinterDates.SecondWeek1PMto1PM;

            var initialDates = dateRange1;
            var newDates = dateRange2.MapToDateRange();

            var reservation = await _parkingService.AddReservationAsync(initialDates);
            var initialBookingReference = reservation?.Reference;

            var amendReservationRequest = new AmendReservationRequest
            {
                BookingReference = initialBookingReference,
                DateRange = newDates,
            };
            var controller = new BookingController(_mockLogger, _parkingService);
            var actionResult = await controller.AmendReservationAsync(amendReservationRequest);
            
            var contentResult = actionResult as OkObjectResult;
            var reservationResponse = contentResult?.Value as ReservationResponse;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatusCode));
                Assert.That(reservationResponse?.Reference, Is.EqualTo(initialBookingReference));
                Assert.That(reservationResponse?.DateRange.StartTime, Is.EqualTo(newDates.StartTime));
                Assert.That(reservationResponse?.DateRange.EndTime, Is.EqualTo(newDates.EndTime));
            });
        }

        [Test]
        public async Task AmendBooking_WithExistingBookingNoAvailabiltity_ReturnsNoAvailability()
        {
            var expectedMessage = "Unfortunately there is no availability for the request date range.";
            var expectedStatus = StatusCodes.Status404NotFound;

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
            var actionResult = await bookingController.AmendReservationAsync(amendReservationRequest);

            var contentResult = actionResult as NotFoundObjectResult;
            var availabilityResponse = contentResult?.Value as ReservationResponse;

            Assert.Multiple(() =>
            {
                Assert.That(contentResult?.StatusCode, Is.EqualTo(expectedStatus));
                Assert.That(availabilityResponse?.Error?.Description, Is.EqualTo(expectedMessage));
            });
        }

        private static async Task CreateMultipleReservations(Core.Models.DateRange initialDates, IParkingService parkingService)
        {
            var tasks = new List<Task>();
            Enumerable.Range(1,9).ToList()
                .ForEach(_ => tasks.Add(parkingService.AddReservationAsync(initialDates)));

            await Task.WhenAll(tasks);
        }
    }
}