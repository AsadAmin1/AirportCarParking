using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Core
{
    public class AvailabilityService : IAvailabilityService
    {
        #region Public Methods

        public async Task<Availability> GetAvailabilityAsync(DateRange dateRange, int totalCapacity, List<Reservation> reservations)
        {
            if (dateRange.HasElapsed)
            {
                throw new ElapsedDateException("Start Date can not be in the past.");
            }

            if (dateRange.StartDateAfterEndDate)
            {
                throw new InvalidDatesException("Start Date must be after than the End Date.");
            }

            var spacesTaken = reservations.Count(res => dateRange.Overlaps(res.DateRange));

            var availableSpaces = totalCapacity - spacesTaken;

            return await Task.FromResult(new Availability(availableSpaces));
        }

        #endregion
    }
}