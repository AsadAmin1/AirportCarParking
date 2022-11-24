using ParkingReservation.Core.Interfaces;

namespace ParkingReservation.Core.Models
{
    public class Reservation
    {
        #region Public Properties

        public string Reference { get; }

        public DateRange DateRange { get; }
        public IBookable Item { get ; }

        #endregion

        #region Constructors

        public Reservation(DateRange dateRange, IBookable item)
        {
            Reference = Guid.NewGuid().ToString();
            DateRange = dateRange;
            Item = item;
        }

        #endregion
    }
}
