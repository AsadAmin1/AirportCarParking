using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Interfaces
{
    public interface IPriceRule
    {
        decimal ApplyRule(DateRange dateRange);
    }
}