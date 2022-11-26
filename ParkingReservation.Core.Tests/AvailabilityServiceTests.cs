using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;
using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests
{
    public class AvailabilityServiceTests
    {
        private readonly int totalCapacity = 10;
        private List<IBookable> _bookableItems;

        [SetUp]
        public void SetUp()
        {
            _bookableItems = new List<IBookable>();
            PopulateBookableItemsList(1, 10);
        }

        [Test]
        public async Task GetAvailability_WithNoBookings_Returns10Spaces()
        {
            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var sut = new AvailabilityService();
            var actual = await sut.GetAvailabilityAsync(dateRange, 10, new List<Reservation>());

            Assert.That(actual, Is.EqualTo(totalCapacity));
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

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var sut = new AvailabilityService();
            var actual = await sut.GetAvailabilityAsync(dateRange, 10, new List<Reservation> { new(dateRange, new CarParkingSpot("1")) });

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetAvailability_WithExistingNotMatchingBooking_Returns10Spaces()
        {
            var expected = totalCapacity;

            var firstWeek = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var secondWeek = TestBookingDates.WinterDates.SecondWeek1PMto1PM;
            
            var sut = new AvailabilityService();
            var actual = await sut.GetAvailabilityAsync(secondWeek, 10, new List<Reservation> { new(firstWeek, new CarParkingSpot("1")) });

            Assert.That(expected, Is.EqualTo(actual));
        }

        private void PopulateBookableItemsList(int start, int end)
        {
            Enumerable.Range(start, end).ToList()
                .ForEach(i => _bookableItems.Add(new CarParkingSpot(i.ToString())));
        }
    }
}