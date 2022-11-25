using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.TestHelpers;
using ParkingReservation.Core.Exceptions;

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

            var dateRange = TestBookingDates.Jan1To8_1300_1300;

            var sut = new BookingService(bookableItems);
            var actual = await sut.AddReservationAsync(dateRange, expectedPrice);

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

            var dateRange = TestBookingDates.Jan1To8_1300_1300;

            var sut = new BookingService(bookableItems);
            var reservation = await sut.AddReservationAsync(dateRange, expectedPrice);

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

            var dateRange = TestBookingDates.Jan1To8_1300_1300;

            var sut = new BookingService(bookableItems);
            var reservation = await sut.AddReservationAsync(dateRange, expectedPrice);

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
    }
}