using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Core
{
    public class ParkingService : IParkingService
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IBookingService _bookingService;

        public ParkingService(IAvailabilityService availabilityService, IBookingService bookingService)
        {
            _availabilityService = availabilityService;
            _bookingService = bookingService;
        }

        public async Task<Reservation> AddReservationAsync(DateRange dateRange)
        {
            var totalCapacity = _bookingService.TotalCapacity;
            var reservations = _bookingService.Reservations;

            var availability = await _availabilityService.GetAvailabilityAsync(dateRange, totalCapacity, reservations);
            if (availability.Spaces > 0)
            {
                var reservation = await _bookingService.AddReservationAsync(dateRange);
                if (reservation != null)
                {
                    return reservation;
                }
            }
            throw new NoAvailabilityException("Unfortunately there is no availability for the request date range.");
        }

        public async Task<Availability> GetAvailabilityAsync(DateRange dateRange)
        {
            var totalCapacity = _bookingService.TotalCapacity;
            var reservations = _bookingService.Reservations;

            return await _availabilityService.GetAvailabilityAsync(dateRange, totalCapacity, reservations);
        }
    }
}
