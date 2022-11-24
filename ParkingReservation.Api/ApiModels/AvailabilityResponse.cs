using ParkingReservation.Api.Interfaces;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Api.ApiModels
{
    public class AvailabilityResponse : IErrorDetails
    {
        public int Spaces { get; set; }
        public ErrorDetails Error { get; set; }
    }
}
