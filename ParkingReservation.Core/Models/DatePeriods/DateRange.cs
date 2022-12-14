namespace ParkingReservation.Core.Models.DatePeriods
{
    public class DateRange
    {
        #region Public Properties

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool HasElapsed { get => StartTime < DateTime.Now; }
        public bool StartDateAfterEndDate { get => EndTime <= StartTime; }

        #endregion

        #region Constructors

        public DateRange(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        #endregion

        #region Public Methods

        public bool Overlaps(DateRange dateRange)
            => StartTime < dateRange.EndTime && dateRange.StartTime < EndTime;

        public DateRange AddDays(int days)
        {
            return new DateRange(StartTime.AddDays(days), EndTime.AddDays(days));
        }

        #endregion
    }
}
