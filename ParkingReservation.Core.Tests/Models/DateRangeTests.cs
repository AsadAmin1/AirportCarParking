using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests.Models
{
    public class DateRangeTests
    {
        [Test]
        public void Overlaps_WithNoOverlap_ReturnsFalse()
        {
            var expected = false;

            var dateRange1 = TestBookingDates.Jan1To9_1300_1300;
            var dateRange2 = TestBookingDates.Jan10To19_1300_1300;

            var actual = dateRange1.Overlaps(dateRange2);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Overlaps_WithOverlappingRange_ReturnsTrue()
        {
            var expected = true;

            var dateRange1 = TestBookingDates.Jan1To9_1300_1300;
            var dateRange2 = TestBookingDates.Jan8To15_1300_1300;

            var actual = dateRange1.Overlaps(dateRange2);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
