using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Interfaces
{
    public interface IParkingService
    {
        Task<Reservation> AddReservationAsync(DateRange dateRange);
        Task<Availability> GetAvailabilityAsync(DateRange dateRange);
    }
}