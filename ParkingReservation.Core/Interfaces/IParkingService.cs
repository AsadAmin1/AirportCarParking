using ParkingReservation.Core.Models;
using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core.Interfaces
{
    public interface IParkingService
    {
        Task<Reservation> AddReservationAsync(DateRange dateRange);
        
        Task<Availability> GetAvailabilityAsync(DateRange dateRange);

        Task<bool> CancelReservationAsync(string bookingReference);

        Task<Reservation> AmendReservationAsync(AmendReservation amendReservation);
    }
}