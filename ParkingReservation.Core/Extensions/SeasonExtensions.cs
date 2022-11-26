using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.Extensions
{
    public static class SeasonExtensions
    {
        public static DateRange ToDateRangeWithOfSummerSeasonStartForYear(this SummerSeason season, int year)
        {
            var startDate = new DateTime(year, season.Start.Month, season.Start.Date, 0, 0, 0);
            var endDate = new DateTime(year, season.Start.Month, season.Start.Date, 23, 59, 59).AddDays(7);

            return new DateRange(startDate, endDate);
        }

        public static DateRange ToDateRangeSeasonStartForYear(this SummerSeason season, int year)
        {
            var startDate = new DateTime(year, season.Start.Month, season.Start.Date);
            var endDate = new DateTime(year, season.Start.Month, season.Start.Date).AppendTimeEndingOfDay();

            return new DateRange(startDate, endDate);
        }

        public static DateRange ToDateRangeSeasonEndForYear(this SummerSeason season, int year)
        {
            var startDate = new DateTime(year, season.End.Month, season.End.Date);
            var endDate = new DateTime(year, season.End.Month, season.End.Date).AppendTimeEndingOfDay();

            return new DateRange(startDate, endDate);
        }


        public static DateRange ToDateRangeSeasonStartForYear(this WinterSeason season, int year)
        {
            var startDate = new DateTime(year, season.Start.Month, season.Start.Date);
            var endDate = new DateTime(year, season.Start.Month, season.Start.Date).AppendTimeEndingOfDay();

            return new DateRange(startDate, endDate);
        }

        public static DateRange ToDateRangeSeasonEndForYear(this WinterSeason season, int year)
        {
            var startDate = new DateTime(year, season.End.Month, season.End.Date);
            var endDate = new DateTime(year + 1, season.End.Month, season.End.Date).AppendTimeEndingOfDay();

            return new DateRange(startDate, endDate);
        }

        public static DateTime AppendTimeEndingOfDay(this DateTime dateTime)
        {
            return dateTime.AddDays(1).AddMilliseconds(-1);
        }
    }
}
