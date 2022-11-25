namespace ParkingReservation.Core.Models
{
    public class SummerSeason : Season
    {
        public SummerSeason() : base(new Boundary(1, 4), new Boundary(30, 9))
        {
        }
    }
}
