using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Core
{
    public class ParkingService : IParkingService
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IBookingService _bookingService;
        private readonly IPricingService _pricingService;

        public ParkingService(IAvailabilityService availabilityService, IBookingService bookingService, IPricingService pricingService)
        {
            _availabilityService = availabilityService;
            _bookingService = bookingService;
            _pricingService = pricingService;
        }

        public async Task<Availability> GetAvailabilityAsync(DateRange dateRange)
        {
            var totalCapacity = _bookingService.TotalCapacity;
            var reservations = _bookingService.Reservations;

            var price = _pricingService.GetPrice(dateRange);
            var spaces = await _availabilityService.GetAvailabilityAsync(dateRange, totalCapacity, reservations);

            return new Availability(spaces, price);
        }

        public async Task<Reservation> AddReservationAsync(DateRange dateRange)
        {
            var totalCapacity = _bookingService.TotalCapacity;
            var reservations = _bookingService.Reservations;

            var spaces = await _availabilityService.GetAvailabilityAsync(dateRange, totalCapacity, reservations);
            if (spaces > 0)
            {
                var price = _pricingService.GetPrice(dateRange);

                var reservation = await _bookingService.CreateReservationAsync(dateRange, price);
                if (reservation != null)
                {
                    return reservation;
                }
            }
            throw new NoAvailabilityException("Unfortunately there is no availability for the request date range.");
        }

        public async Task<bool> CancelReservationAsync(string bookingReference)
        {
            return await _bookingService.CancelReservationAsync(bookingReference);
        }

        public async Task<Reservation> AmendReservationAsync(AmendReservationRequest amendReservationRequest)
        {
            var dateRange = amendReservationRequest.DateRange;
            var totalCapacity = _bookingService.TotalCapacity;
            var reservations = _bookingService.Reservations;

            var spaces = await _availabilityService.GetAvailabilityAsync(dateRange, totalCapacity, reservations);
            if (spaces > 0)
            {
                var price = _pricingService.GetPrice(dateRange);

                return  await _bookingService.AmendReservationAsync(amendReservationRequest.BookingReference, dateRange, price);
            }

            throw new NoAvailabilityException("Unfortunately there is no availability for the request date range.");
        }
    }
}
