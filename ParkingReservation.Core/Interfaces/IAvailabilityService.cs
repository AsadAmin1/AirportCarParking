using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Interfaces
{
    public interface IAvailabilityService
    {
        Task<Availability> GetAvailabilityAsync(DateRange dateRange, int totalCapacity, List<Reservation> reservations);
    }
}