using ParkingReservation.Core.Models;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Exceptions;

namespace ParkingReservation.Core
{
    public class BookingService : IBookingService
    {
        #region Private Fields

        private readonly Dictionary<string, Reservation> _reservations;
        private readonly List<IBookable> _bookableItems;

        #endregion

        #region Properties

        public int TotalCapacity { get => _bookableItems.Count; }
        public List<Reservation> Reservations { get => _reservations.Values.ToList(); }

        #endregion

        #region Constructors

        public BookingService(List<IBookable> bookableItems)
        {
            _reservations = new Dictionary<string, Reservation>();
            _bookableItems = bookableItems;
        }

        #endregion

        #region Public Methods

        public async Task<Reservation> AddReservationAsync(DateRange dateRange, decimal price)
        {
            var bookingsInDateRange = Reservations
                            .Where(b => b.DateRange.Overlaps(dateRange))
                            .Select(i => i.Item)
                            .ToList();

            if (bookingsInDateRange.Count == TotalCapacity)
            {
                throw new NoAvailabilityException("Unfortunately there is no availability for the request date range.");
            }

            var item = _bookableItems.Except(bookingsInDateRange).First();
            
            var reference = Guid.NewGuid().ToString();
            var res = new Reservation(dateRange, item, price);
            _reservations.Add(reference, res);

            return await Task.FromResult(res);
        }

        #endregion
    }
}