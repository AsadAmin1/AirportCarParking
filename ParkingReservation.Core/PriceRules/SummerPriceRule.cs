using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Tests.PriceRules
{
    public class SummerPriceRule : BasePriceRule 
    {
        #region Private Fields

        private readonly Season _season;

        #endregion

        #region Constructors

        public SummerPriceRule() : base(2m)
        {
            _season = new SummerSeason();
        }

        #endregion

        #region IPriceRule

        public override decimal ApplyRule(DateRange dateRange)
        {
            var seasonStart = new DateTime(dateRange.StartTime.Year, _season.Start.Month, _season.Start.Date, 0, 0, 0);
            var seasonEnd = new DateTime(dateRange.StartTime.Year, _season.End.Month, _season.End.Date, 23,59,59);

            if (dateRange.StartTime >= seasonStart && dateRange.EndTime <= seasonEnd)
            {
                return base.ApplyRule(dateRange);
            }

            return 0;
        }

        #endregion
    }
}
