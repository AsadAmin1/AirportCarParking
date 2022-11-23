namespace ParkingReservation.Core.Models
{
    public class Reservation
    {
        #region Public Properties

        public DateRange DateRange { get; }

        #endregion

        #region Constructors

        public Reservation(DateRange dateRange)
        {
            DateRange = dateRange;
        }

        #endregion
    }
}
