using ParkingReservation.Core.Extensions;
using ParkingReservation.Core.Models;
using ParkingReservation.Core.TestHelpers;

namespace ParkingReservation.Core.Tests.PriceRules
{
    public class SummerPriceRuleTests
    {
        [Test]
        public void ApplyPrice_With7DaysInWinter_Returns168()
        {
            var expected = 0m;
            var dateRange = TestBookingDates.WinterDates.FirstWeek1PMto1PM;

            var pr = new SummerPriceRule();
            var actual = pr.ApplyRule(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ApplyPrice_With7DaysInSummer_Returns0()
        {
            var expected = 336m;
            var dateRange = TestBookingDates.SummerDates.FirstWeek1PMtoPM;

            var pr = new SummerPriceRule();
            var actual = pr.ApplyRule(dateRange);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ApplyPrice_With1DaysStartingOnSummerSeason_Returns0()
        {
            var expected = 48m;

            var summerSeason = new SummerSeason().ToDateRangeSeasonStartForYear(2023);

            var pr = new SummerPriceRule();
            var actual = pr.ApplyRule(summerSeason);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ApplyPrice_With1DayBeforeSummerSeasonStarts_Returns0()
        {
            var expected = 0m;

            var summerSeason = new SummerSeason()
                .ToDateRangeSeasonStartForYear(2023)
                .AddDays(-1);

            var pr = new SummerPriceRule();
            var actual = pr.ApplyRule(summerSeason);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ApplyPrice_With1DayAfterSummerSeasonEnds_Returns0()
        {
            var expected = 0m;

            var summerSeason = new SummerSeason()
                .ToDateRangeSeasonEndForYear(2023)
                .AddDays(1);

            var pr = new SummerPriceRule();
            var actual = pr.ApplyRule(summerSeason);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
