namespace ParkingReservation.Core.Models
{
    public class DateRange
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public DateRange(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
