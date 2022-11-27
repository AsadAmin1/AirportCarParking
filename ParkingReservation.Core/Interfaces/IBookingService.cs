using ParkingReservation.Core.Models;
using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core.Interfaces
{
    public interface IBookingService
    {
        int TotalCapacity { get; }

        List<Reservation> Reservations { get; }

        Task<Reservation> CreateReservationAsync(DateRange dateRange, decimal price);

        Task<bool> CancelReservationAsync(string bookingReference);

        Task<Reservation> GetReservationAsync(string bookingReference);

        Task<Reservation> AmendReservationAsync(string bookingReference, DateRange dateRange, decimal price);
    }
}