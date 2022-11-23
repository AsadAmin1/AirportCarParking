namespace ParkingReservation.Core.Models
{
    public class DateRange
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public DateRange(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
