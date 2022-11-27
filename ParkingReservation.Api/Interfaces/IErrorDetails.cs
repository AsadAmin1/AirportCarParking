using ParkingReservation.Api.ApiModels;

namespace ParkingReservation.Api.Interfaces
{
    public interface IErrorDetails
    {
        ErrorDetails Error { get; set; }
    }
}
