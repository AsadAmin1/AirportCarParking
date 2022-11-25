using ParkingReservation.Core.Interfaces;

namespace ParkingReservation.Core.Models
{
    public class Reservation
    {
        #region Public Properties

        public string Reference { get; }

        public DateRange DateRange { get; }
        public decimal Price { get; }
        public IBookable Item { get ; }

        #endregion

        #region Constructors

        public Reservation(DateRange dateRange, IBookable item, string reference = "", decimal price = 0)
        {
            Reference = reference;
            DateRange = dateRange;
            Price = price;
            Item = item;
        }

        #endregion
    }
}
