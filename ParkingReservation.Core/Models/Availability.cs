namespace ParkingReservation.Core.Models
{
    public class Availability
    {
        #region Public Properties
        public int Spaces { get; }

        #endregion

        #region Constructors

        public Availability(int spaces)
        {
            Spaces = spaces;
        }

        #endregion
    }
}
