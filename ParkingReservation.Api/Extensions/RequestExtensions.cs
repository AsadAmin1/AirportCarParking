using ParkingReservation.Api.ApiModels;

namespace ParkingReservation.Api.Extensions
{
    public static class RequestExtensions
    {
        public static Core.Models.AmendReservationRequest MapToDomainModel(this AmendReservationRequest amendReservationRequest)
        {
            return new Core.Models.AmendReservationRequest(
                amendReservationRequest.BookingReference, 
                amendReservationRequest.DateRange.MapToDateRange()
            );
        }
    }
}
