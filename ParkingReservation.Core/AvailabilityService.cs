using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;
using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core
{
    public class AvailabilityService : IAvailabilityService
    {
        #region Public Methods

        public async Task<int> GetAvailabilityAsync(DateRange dateRange, int totalCapacity, List<Reservation> reservations)
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

            if (availableSpaces > 0)
            {
                return await Task.FromResult(availableSpaces);
            }

            throw new NoAvailabilityException("Unfortunately there is no availability for the request date range.");
        }

        #endregion
    }
}