using ParkingReservation.Core.Exceptions;

namespace ParkingReservation.Core.Tests
{
    public class AvailabilityServiceTests
    {
        private readonly int totalCapacity = 10;

        [Test]
        public async Task GetAvailability_WithNoBookings_Returns10Spaces()
        {
            var dateRange = TestBookingDates.Jan1To9_1300_1300;

            var sut = new AvailabilityService(totalCapacity);
            var actual = await sut.GetAvailability(dateRange);

            Assert.That(totalCapacity, Is.EqualTo(actual));
        }

        [Test]
        public void GetAvailability_WithElapsedDate_ThrowsElapsedDateException()
        {
            var expected = "Start Date can not be in the past.";
            
            var dateRange = TestBookingDates.ElapsedDate;
            var sut = new AvailabilityService(totalCapacity);
            var ex = Assert.ThrowsAsync<ElapsedDateException>(async () =>
            {
                await sut.GetAvailability(dateRange);
            });

            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        [Test]
        public void GetAvailability_WithInvalidDates_ThrowsInvalidDatesException()
        {
            var expected = "Start Date must be after than the End Date.";

            var dateRange = TestBookingDates.InvalidDates;
            var sut = new AvailabilityService(totalCapacity);
            var ex = Assert.ThrowsAsync<InvalidDatesException>(async () =>
            {
                await sut.GetAvailability(dateRange);
            });

            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        [Test]
        public async Task GetAvailability_WithExistingMatchingBooking_Returns9Spaces()
        {
            var expected = totalCapacity - 1;

            var dateRange = TestBookingDates.Jan1To9_1300_1300;
            var sut = new AvailabilityService(totalCapacity);
            sut.AddReservation(dateRange);

            var actual = await sut.GetAvailability(dateRange);

            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}