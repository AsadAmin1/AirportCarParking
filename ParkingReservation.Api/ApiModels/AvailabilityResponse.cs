using ParkingReservation.Api.Interfaces;

namespace ParkingReservation.Api.ApiModels
{
    public class AvailabilityResponse : IErrorDetails
    {
        public int Spaces { get; set; }
        public decimal Price { get; set; }
        public ErrorDetails Error { get; set; }
    }
}
