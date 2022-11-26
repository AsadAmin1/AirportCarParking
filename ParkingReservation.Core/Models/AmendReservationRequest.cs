namespace ParkingReservation.Core.Models
{
    public class AmendReservationRequest
    {
        public string BookingReference { get; set; }
        public DateRange DateRange { get; set; }

        public AmendReservationRequest(string bookingReference, DateRange dateRange)
        {
            BookingReference = bookingReference;
            DateRange = dateRange;
        }
    }
}
