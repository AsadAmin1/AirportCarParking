namespace ParkingReservation.Core.Models
{
    public class Availability
    {
        #region Public Properties
        public int Spaces { get; }
        public decimal Price { get; }

        #endregion

        #region Constructors

        public Availability(int spaces, decimal price)
        {
            Spaces = spaces;
            Price = price;
        }

        #endregion
    }
}
