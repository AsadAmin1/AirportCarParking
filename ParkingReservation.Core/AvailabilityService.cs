using ParkingReservation.Core.Models;

namespace ParkingReservation.Core
{
    public class AvailabilityService
    {
        private readonly int _totalCapacity;

        public AvailabilityService(int totalCapacity)
        {
            _totalCapacity = totalCapacity;
        }

        public int GetAvailability(DateRange _)
        {
            return _totalCapacity;
        }
    }
}