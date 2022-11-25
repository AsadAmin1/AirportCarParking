using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Tests.PriceRules
{
    public class WinterPriceRule : BasePriceRule
    {
        private readonly Season _season;

        public WinterPriceRule(): base(1m)
        {
            _season = new WinterSeason();
        }

        public override decimal ApplyRule(DateRange dateRange)
        {
            var seasonStart = new DateTime(dateRange.StartTime.Year, _season.Start.Month, _season.Start.Date, 0, 0, 0);

            if (dateRange.StartTime < seasonStart)
            {
                seasonStart = seasonStart.AddYears(-1);
            }

            var seasonEnd = new DateTime(seasonStart.Year + 1, _season.End.Month, _season.End.Date, 0, 0, 0);

            if (dateRange.StartTime > seasonStart && dateRange.StartTime < seasonEnd)
            {
                return base.ApplyRule(dateRange);
            }

            return 0;
        }
    }
}
