using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests.PriceRules
{
    public class WinterPriceRuleTests
    {
        [Test]
        public void ApplyPrice_With7DaysInWinter_Returns168()
        {
            var expected = 168m;
            var dateRange = TestBookingDates.Jan1To8_1300_1300;

            var pr = new WinterPriceRule();
            var actual = pr.ApplyRule(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ApplyPrice_With7DaysInSummer_Returns0()
        {
            var expected = 168m;
            var dateRange = TestBookingDates.Jan1To8_1300_1300;

            var pr = new WinterPriceRule();
            var actual = pr.ApplyRule(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
