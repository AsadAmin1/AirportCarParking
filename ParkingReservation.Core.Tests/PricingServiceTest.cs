using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.TestHelpers;
using ParkingReservation.Core.Tests.PriceRules;

namespace ParkingReservation.Core.Tests
{
    public class PricingServiceTest
    {
        private readonly List<IPriceRule> _priceRules = new()
        {
            new SummerPriceRule(),
            new WinterPriceRule()
        };

        [Test]
        public void GetPrice_WithNoPricing_Returns0()
        {
            var expected = 0;
            var dateRange = TestBookingDates.WinterDates.SecondWeek1PMto1PM;

            var pricing = new PricingService(new List<IPriceRule>());
            var actual = pricing.GetPrice(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetPrice_WithWinterFor1Week_Returns168()
        {
            var expected = 168;
            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var pricing = new PricingService(_priceRules);
            var actual = pricing.GetPrice(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetPrice_WithSummerFor1Week_Returns168()
        {
            var expected = 336;
            var dateRange = TestBookingDates.SummerDates.FirstWeek1PMtoPM;

            var pricing = new PricingService(_priceRules);
            var actual = pricing.GetPrice(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
