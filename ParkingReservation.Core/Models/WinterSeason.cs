namespace ParkingReservation.Core.Models
{
    public class WinterSeason : Season
    {
        public WinterSeason() : base(new Boundary(1, 10), new Boundary(31, 3))
        {
        }
    }
}
