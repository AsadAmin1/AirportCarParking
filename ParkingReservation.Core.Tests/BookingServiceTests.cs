using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;
using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests
{
    public class BookingServiceTests
    {
        private readonly int totalCapacity = 10;
        private readonly List<IBookable> bookableItems = TestBookableItems.Items;

        [Test]
        public async Task AddReservation_WithAvailableDates_ReturnsReservation()
        {
            var dateRange = TestBookingDates.Jan1To8_1300_1300;
            var expectedSpaces = 1;
            var expectedPrice = 168m;

            var price = 1;
            var sut = new BookingService(bookableItems);
            var actual = await sut.AddReservationAsync(dateRange, price);

            Assert.Multiple(() =>
            {
                Assert.That(sut.Reservations.Count, Is.EqualTo(expectedSpaces));
                Assert.That(actual.DateRange.StartTime, Is.EqualTo(dateRange.StartTime));
                Assert.That(actual.DateRange.EndTime, Is.EqualTo(dateRange.EndTime));
                Assert.That(actual.Reference, !Is.EqualTo(String.Empty));
                Assert.That(actual.Item, !Is.Null);
                Assert.That(actual.Price, Is.EqualTo(expectedPrice));
            });
        }

        [Test]
        public void GetAvailability_WithElapsedDate_ThrowsElapsedDateException()
        {
            var expected = "Start Date can not be in the past.";
            
            var dateRange = TestBookingDates.ElapsedDate;
            var sut = new AvailabilityService();
            var ex = Assert.ThrowsAsync<ElapsedDateException>(async () =>
            {
                await sut.GetAvailabilityAsync(dateRange, 10, new List<Reservation>());
            });

            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        [Test]
        public void GetAvailability_WithInvalidDates_ThrowsInvalidDatesException()
        {
            var expected = "Start Date must be after than the End Date.";

            var dateRange = TestBookingDates.InvalidDates;
            var sut = new AvailabilityService();
            var ex = Assert.ThrowsAsync<InvalidDatesException>(async () =>
            {
                await sut.GetAvailabilityAsync(dateRange, 10, new List<Reservation>());
            });

            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetAvailability_WithExistingMatchingBooking_Returns9Spaces()
        {
            var expected = totalCapacity - 1;

            var dateRange = TestBookingDates.Jan1To8_1300_1300;
            var sut = new AvailabilityService();
            var actual = await sut.GetAvailabilityAsync(dateRange, 10, new List<Reservation> { new(dateRange, new CarParkingSpot("1")) });

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetAvailability_WithExistingNotMatchingBooking_Returns10Spaces()
        {
            var expected = totalCapacity;

            var firstWeek = TestBookingDates.Jan1To8_1300_1300;
            var secondWeek = TestBookingDates.Jan9To16_1300_1300;
            
            var sut = new AvailabilityService();
            var actual = await sut.GetAvailabilityAsync(secondWeek, 10, new List<Reservation> { new(firstWeek, new CarParkingSpot("1")) });

            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}