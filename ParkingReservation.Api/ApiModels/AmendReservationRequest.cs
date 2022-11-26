namespace ParkingReservation.Api.ApiModels
{
    public class AmendReservationRequest
    {
        public string BookingReference { get; set; }
        public DateRange DateRange { get; set; }
    }
}
