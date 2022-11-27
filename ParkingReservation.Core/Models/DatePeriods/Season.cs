namespace ParkingReservation.Core.Models.DatePeriods
{
    public class Season
    {
        public Boundary Start { get; }
        public Boundary End { get; }

        public Season(Boundary start, Boundary end)
        {
            Start = start;
            End = end;
        }
    }
}
