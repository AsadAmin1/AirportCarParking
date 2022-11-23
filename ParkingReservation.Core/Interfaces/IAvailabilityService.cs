using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Interfaces
{
    public interface IAvailabilityService
    {
        Task<int> GetAvailability(DateRange _);
    }
}