using ParkingReservation.Api.ApiModels;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Api.Extensions
{
    // TODO: Use AutoMapper
    public static class ResponseExtensions
    {
        public static AvailabilityResponse AsAvailabilityResponse(this Availability availability, ErrorDetails error = null)
        {
            return new AvailabilityResponse {
                Spaces = availability.Spaces,
                Price = availability.Price,
                Error = error,
            };
        }

        public static AvailabilityResponse MapToAvailabilityResponse(Availability availability, ErrorDetails error = null)
        {
            return new AvailabilityResponse
            {
                Spaces = availability.Spaces,
                Error = error
            };
        }

        public static ReservationResponse MapToReservationResponse(this Reservation reservation, ErrorDetails error = null)
        {
            return new ReservationResponse
            {
                DateRange = new ApiModels.DateRange { StartTime = reservation.DateRange.StartTime, EndTime = reservation.DateRange.EndTime },
                Reference = reservation.Reference,
                Price = reservation.Price,
                Item = reservation.Item,
                Error = error,
            };
        }

        public static ApiModels.DateRange MapToDateRange(this Core.Models.DateRange dateRange)
        {
            return new ApiModels.DateRange
            {
                StartTime = dateRange.StartTime,
                EndTime = dateRange.EndTime,
            };
        }

        public static Core.Models.DateRange MapToDateRange(this ApiModels.DateRange dateRange)
        {
            return new Core.Models.DateRange(
                dateRange.StartTime, 
                dateRange.EndTime
            );
        }
    }
}
