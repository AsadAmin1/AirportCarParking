using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core.Models
{
    public class Reservation
    {
        #region Public Properties

        public string Reference { get; }

        private DateRange _dateRange;
        public DateRange DateRange { get => _dateRange; }

        private decimal _price;
        public decimal Price { get => _price; }

        private IBookable _item;
        public IBookable Item { get => _item; }

        #endregion

        #region Constructors

        public Reservation(DateRange dateRange, IBookable item, string reference = "", decimal price = 0)
        {
            _dateRange = dateRange;
            _price = price;
            _item = item;
            Reference = reference;
        }

        public void Amend(DateRange dateRange, IBookable item, decimal price)
        {
            _dateRange = dateRange;
            _price = price;
            _item = item;
        }

        #endregion
    }
}
