using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Interfaces
{
    public interface IBookingService
    {
        int TotalCapacity { get; }
        List<Reservation> Reservations { get; }
        Task<Reservation> AddReservationAsync(DateRange dateRange, decimal price);
    }
}