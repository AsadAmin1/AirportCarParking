using ParkingReservation.Core.Models;
using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core.Interfaces
{
    public interface IAvailabilityService
    {
        Task<int> GetAvailabilityAsync(DateRange dateRange, int totalCapacity, List<Reservation> reservations);
    }
}