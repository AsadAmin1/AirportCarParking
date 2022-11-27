using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core.Interfaces
{
    public interface IPriceRule
    {
        decimal ApplyRule(DateRange dateRange);
    }
}