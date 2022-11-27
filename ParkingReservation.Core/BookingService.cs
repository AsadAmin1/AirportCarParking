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

        public async Task<Reservation> CreateReservationAsync(DateRange dateRange, decimal price)
        {
            IBookable item = GetBookableItem(dateRange);

            var reference = Guid.NewGuid().ToString();
            var res = new Reservation(dateRange, item, reference, price);
            _reservations.Add(reference, res);

            return await Task.FromResult(res);
        }

        private IBookable GetBookableItem(DateRange dateRange)
        {
            var bookingsInDateRange = Reservations
                            .Where(b => b.DateRange.Overlaps(dateRange))
                            .Select(i => i.Item)
                            .ToList();

            if (bookingsInDateRange.Count == TotalCapacity)
            {
                throw new NoAvailabilityException("Unfortunately there is no availability for the request date range.");
            }

            return _bookableItems.Except(bookingsInDateRange).First();
        }

        public async Task<bool> CancelReservationAsync(string bookingReference)
        {
            var reservation = await GetReservationAsync(bookingReference);
            return await Task.FromResult(_reservations.Remove(bookingReference));
        }

        public async Task<Reservation> GetReservationAsync(string bookingReference)
        {
            if (_reservations.ContainsKey(bookingReference))
            {
                return await Task.FromResult(_reservations[bookingReference]);
            }

            throw new BookingNotFoundException("Booking not found.");
        }

        public async Task<Reservation> AmendReservationAsync(string bookingReference, DateRange dateRange, decimal price)
        {
            var reservation = await GetReservationAsync(bookingReference);
            IBookable item = GetBookableItem(dateRange);

            reservation.Amend(dateRange, item, price);

            return await Task.FromResult(reservation);
        }

        #endregion
    }
}