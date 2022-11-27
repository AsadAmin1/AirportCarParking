using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core.Interfaces
{
    public interface IPricingService
    {
        decimal GetPrice(DateRange dateRange);
    }
}