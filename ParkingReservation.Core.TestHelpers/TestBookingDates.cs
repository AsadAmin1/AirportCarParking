using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.TestHelpers
{
    public static class TestBookingDates
    {
        public static class WinterDates
        {
            private static readonly DateTime time1Start = new(2024, 1, 1, 13, 0, 0);
            public static DateRange FirstWeek1PMto1PM { get => new(time1Start, time1Start.AddDays(7)); }

            
            private static readonly DateTime time2Start = new(2024, 1, 8, 13, 0, 0);
            public static DateRange SecondWeek1PMto1PM { get => new(time2Start, time2Start.AddDays(7)); }

            
            private static readonly DateTime timeXStart = new(2024, 1, 7, 13, 0, 0);
            public static DateRange FirstWeekOverlap { get => new(timeXStart, timeXStart.AddDays(7)); }
        }

        public static class SummerDates
        {
            private static readonly DateTime time1Start = new(2024, 6, 1, 13, 0, 0);
            public static DateRange FirstWeek1PMtoPM { get => new(time1Start, time1Start.AddDays(7)); }
        }

        public static DateRange ElapsedDate { get => new(new(2022, 1, 1, 0, 0, 0), new(2022, 1, 2, 0, 0, 0)); }
        public static DateRange InvalidDates {  get => new(new(2023, 1, 31, 13, 0, 0), new(2023, 1, 29, 13, 0, 0)); }
    }
}