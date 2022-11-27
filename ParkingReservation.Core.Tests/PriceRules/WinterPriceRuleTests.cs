using ParkingReservation.Core.Extensions;
using ParkingReservation.Core.Models;
using ParkingReservation.Core.Models.DatePeriods;
using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests.PriceRules
{
    public class WinterPriceRuleTests
    {
        [Test]
        public void ApplyPrice_With7DaysInSummer_Returns0()
        {
            var expected = 0m;

            var dateRange = TestBookingDates.SummerDates.FirstWeek1PMtoPM;

            var pr = new WinterPriceRule();
            var actual = pr.ApplyRule(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ApplyPrice_With7DaysInWinter_Returns168()
        {
            var expected = 168m;

            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var pr = new WinterPriceRule();
            var actual = pr.ApplyRule(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }


        [Test]
        public void ApplyPrice_With1DayBeforeWinterSeasonStarts_Returns0()
        {
            var expected = 0m;

            var season = new WinterSeason()
                .ToDateRangeSeasonStartForYear(2023)
                .AddDays(-1);

            var pr = new WinterPriceRule();
            var actual = pr.ApplyRule(season);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ApplyPrice_With1DayAfterWinterSeasonEnds_Returns0()
        {
            var expected = 0m;

            var season = new WinterSeason()
                .ToDateRangeSeasonEndForYear(2023)
                .AddDays(1);

            var pr = new WinterPriceRule();
            var actual = pr.ApplyRule(season);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
