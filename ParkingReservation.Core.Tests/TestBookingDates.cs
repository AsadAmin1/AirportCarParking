using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Tests
{
    public static class TestBookingDates
    {
        private static readonly DateTime startTime = new(2023, 1, 2, 13, 0, 0);
        private static readonly DateTime endTime = startTime.AddDays(7);

        public static DateRange Jan1To9_1300_1300 { get => new(startTime, endTime); }
        public static DateRange ElapsedDate { get => new(startTime.AddYears(-2), endTime); }
        public static DateRange InvalidDates {  get => new(endTime, startTime); }
    }
}