using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests.Models
{
    public class DateRangeTests
    {
        [Test]
        public void Overlaps_WithNoOverlap_ReturnsFalse()
        {
            var expected = false;

            var dateRange1 = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var dateRange2 = TestBookingDates.WinterDates.SecondWeek1PMto1PM;

            var actual = dateRange1.Overlaps(dateRange2);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Overlaps_WithOverlappingRange_ReturnsTrue()
        {
            var expected = true;

            var dateRange1 = TestBookingDates.WinterDates.FirstWeek1PMto1PM;
            var dateRange2 = TestBookingDates.WinterDates.FirstWeekOverlap;

            var actual = dateRange1.Overlaps(dateRange2);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
