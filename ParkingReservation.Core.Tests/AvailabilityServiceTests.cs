using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Tests
{
    public class AvailabilityServiceTests
    {
        private readonly int totalCapacity = 10;

        [Test]
        public void GetAvailability_WithNoBookings_Returns10Spaces()
        {
            var startTime = new DateTime(2023, 1, 2, 13, 0, 0);
            var endTime = new DateTime(2023, 1, 9, 13, 0, 0);

            var dateRange = new DateRange(startTime, endTime);

            var sut = new AvailabilityService(totalCapacity);
            var actual = sut.GetAvailability(dateRange);

            Assert.That(totalCapacity, Is.EqualTo(actual));
        }
    }
}