using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests.PriceRules
{
    public class SummerPriceRuleTests
    {
        [Test]
        public void ApplyPrice_With7DaysInWinter_Returns168()
        {
            var expected = 0m;
            var dateRange = TestBookingDates.Jan1To8_1300_1300;

            var pr = new SummerPriceRule();
            var actual = pr.ApplyRule(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ApplyPrice_With7DaysInSummer_Returns0()
        {
            var expected = 336m;
            var dateRange = TestBookingDates.Jun1To8_1300_1300;

            var pr = new SummerPriceRule();
            var actual = pr.ApplyRule(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
