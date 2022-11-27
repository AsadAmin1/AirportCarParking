using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.TestHelpers;
using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core.Tests
{
    public class BookingServiceTests
    {
        private readonly List<IBookable> bookableItems = TestBookableItems.Items;

        [Test]
        public async Task AddReservation_WithAvailableDates_ReturnsReservation()
        {
            var expectedSpaces = 1;
            var expectedPrice = 168m;

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var sut = new BookingService(bookableItems);
            var actual = await sut.CreateReservationAsync(dateRange, expectedPrice);

            Assert.Multiple(() =>
            {
                Assert.That(sut.Reservations, Has.Count.EqualTo(expectedSpaces));
                Assert.That(actual.DateRange.StartTime, Is.EqualTo(dateRange.StartTime));
                Assert.That(actual.DateRange.EndTime, Is.EqualTo(dateRange.EndTime));
                Assert.That(actual.Reference, !Is.EqualTo(String.Empty));
                Assert.That(actual.Item, !Is.Null);
                Assert.That(actual.Price, Is.EqualTo(expectedPrice));
            });
        }

        [Test]
        public async Task CancelReservation_WithReference_ReturnsTrue()
        {
            var expectedReservations = 0;
            var expectedPrice = 1m;
            var expectedResponse = true;

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var sut = new BookingService(bookableItems);
            var reservation = await sut.CreateReservationAsync(dateRange, expectedPrice);

            var actual = await sut.CancelReservationAsync(reservation.Reference);

            Assert.Multiple(() =>
            {
                Assert.That(sut.Reservations, Has.Count.EqualTo(expectedReservations));
                Assert.That(actual, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public async Task CancelReservation_WithUnknownReference_Throws()
        {
            var expectedReservations = 1;
            var expectedPrice = 1m;
            var expectedResponse = false;

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var sut = new BookingService(bookableItems);
            var reservation = await sut.CreateReservationAsync(dateRange, expectedPrice);

            bool actual = false;
            var ex = Assert.ThrowsAsync<BookingNotFoundException>(async () =>
            {
                actual = await sut.CancelReservationAsync("abc");
            });

            Assert.Multiple(() =>
            {
                Assert.That(ex.Message, Is.EqualTo("Booking not found."));
                Assert.That(sut.Reservations, Has.Count.EqualTo(expectedReservations));
                Assert.That(actual, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public async Task AmendBooking_WithExistingBookingWithAvailableDates_ReturnsTrue()
        {
            var dateRange1 = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var dateRange2 = TestBookingDates.WinterDates.SecondWeek1PMto1PM;

            var initialDates = dateRange1;
            var initialPrice = 150m;

            var newDates = initialDates;
            var newPrice = 200m;

            var bookingService = new BookingService(bookableItems);
            var reservation = await bookingService.CreateReservationAsync(initialDates, initialPrice);
            var initialBookingReference = reservation.Reference;

            var amendedReservation = await bookingService.AmendReservationAsync(reservation.Reference, newDates, newPrice);

            Assert.Multiple(() =>
            {
                Assert.That(reservation.Reference, Is.EqualTo(initialBookingReference));
                Assert.That(amendedReservation.DateRange.StartTime, Is.EqualTo(newDates.StartTime));
            });
        }

        [Test]
        public async Task AmendBooking_WithExistingBookingNoAvailabiltity_ThrowsException()
        {
            var expectedMessage = "Unfortunately there is no availability for the request date range.";

            var dateRange1 = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var dateRange2 = TestBookingDates.WinterDates.SecondWeek1PMto1PM;

            var initialDates = dateRange1;
            var initialPrice = 150m;

            var newDates = initialDates;
            var newPrice = 200m;

            var bookingService = new BookingService(bookableItems);
            var reservation = await bookingService.CreateReservationAsync(initialDates, initialPrice);
            var initialBookingReference = reservation.Reference;

            await CreateMultipleReservations(initialDates, initialPrice, bookingService);

            var ex = Assert.ThrowsAsync<NoAvailabilityException>(async () =>
            {
                await bookingService.AmendReservationAsync(reservation.Reference, newDates, newPrice);
            });

            Assert.Multiple(() =>
            {
                Assert.That(ex.Message, Is.EqualTo(expectedMessage));
            });
        }

        private static async Task CreateMultipleReservations(DateRange initialDates, decimal price, BookingService bookingService)
        {
            var tasks = new List<Task>();
            for (int i = 1; i < 10; i++)
            {
                tasks.Add(bookingService.CreateReservationAsync(initialDates, price));
            }
            await Task.WhenAll(tasks);
        }
    }
}