using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Tests.PriceRules
{
    public class BasePriceRule : IPriceRule
    {
        protected decimal PricePerHour { get; }

        public BasePriceRule(decimal pricePerHour)
        {
            PricePerHour = pricePerHour;
        }

        public virtual decimal ApplyRule(DateRange dateRange)
        {
            return (decimal)(dateRange.EndTime - dateRange.StartTime).TotalHours * PricePerHour;
        }
    }
}
