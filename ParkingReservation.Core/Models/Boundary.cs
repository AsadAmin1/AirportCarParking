namespace ParkingReservation.Core.Models
{
    public class Boundary
    {
        public int Month { get; }
        public int Date { get;  }

        public Boundary(int date, int month)
        {
            Date = date;
            Month = month;
        }
    }
}
