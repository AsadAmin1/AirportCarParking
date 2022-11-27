using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core.Models
{
    public class AmendReservation
    {
        public string BookingReference { get; set; }
        public DateRange DateRange { get; set; }

        public AmendReservation(string bookingReference, DateRange dateRange)
        {
            BookingReference = bookingReference;
            DateRange = dateRange;
        }
    }
}
