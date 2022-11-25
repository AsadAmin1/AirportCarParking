using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Interfaces
{
    public interface IPricingService
    {
        decimal GetPrice(DateRange dateRange);
    }
}