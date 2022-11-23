using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Core
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly int _totalCapacity;

        public AvailabilityService(int totalCapacity)
        {
            _totalCapacity = totalCapacity;
        }

        public async Task<int> GetAvailability(DateRange _)
        {
            return await Task.FromResult(_totalCapacity);
        }
    }
}