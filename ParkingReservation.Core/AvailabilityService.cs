using ParkingReservation.Core.Exceptions;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Core
{
    public class AvailabilityService : IAvailabilityService
    {
        #region Private Fields

        private readonly int _totalCapacity;
        private readonly Dictionary<string, Reservation> _reservations;

        #endregion

        #region Constructors

        public AvailabilityService(int totalCapacity)
        {
            _totalCapacity = totalCapacity;
            _reservations = new Dictionary<string, Reservation>();
        }

        #endregion

        #region Public Methods

        public async Task<int> GetAvailability(DateRange dateRange)
        {
            if (dateRange.HasElapsed)
            {
                throw new ElapsedDateException("Start Date can not be in the past.");
            }

            if (dateRange.StartDateAfterEndDate)
            {
                throw new InvalidDatesException("Start Date must be after than the End Date.");
            }

            var matches = _reservations
                            .Select(kv => kv.Value)
                            .Count(res => dateRange.Overlaps(res.DateRange));

            return await Task.FromResult(_totalCapacity - matches);
        }

        public Reservation AddReservation(DateRange dateRange)
        {
            var res = new Reservation(dateRange);
            _reservations.Add(Guid.NewGuid().ToString(), res);
            return res;
        }

        #endregion
    }
}