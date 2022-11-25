using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Interfaces
{
    public interface IAvailabilityService
    {
        Task<int> GetAvailabilityAsync(DateRange dateRange, int totalCapacity, List<Reservation> reservations);
    }
}