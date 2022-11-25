using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.TestHelpers
{
    public static class TestBookingDates
    {
        private static readonly DateTime startTime1 = new(2024, 1, 2, 13, 0, 0);
        private static readonly DateTime startTime2 = new(2024, 1, 8, 13, 0, 0);
        private static readonly DateTime startTime3 = new(2024, 1, 9, 13, 0, 0);
        private static readonly DateTime startTime4 = new(2024, 6, 1, 13, 0, 0);

        public static DateRange Jan1To8_1300_1300 { get => new(startTime1, startTime1.AddDays(7)); }
        public static DateRange Jan8To15_1300_1300 { get => new(startTime2, startTime2.AddDays(7)); }
        public static DateRange Jan9To16_1300_1300 { get => new(startTime3, startTime3.AddDays(7)); }
        public static DateRange Jun1To8_1300_1300 { get => new(startTime4, startTime4.AddDays(7)); }
        public static DateRange ElapsedDate { get => new(new(2022, 1, 1, 0, 0, 0), new(2022, 1, 2, 0, 0, 0)); }
        public static DateRange InvalidDates {  get => new(new(2023, 1, 31, 13, 0, 0), new(2023, 1, 29, 13, 0, 0)); }
    }
}