using AutoMapper;
using AutoMapper.Internal;
using ParkingReservation.Api.ApiModels;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Api.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapRequests();
            MapResponses();
            MapModels();
        }

        public void MapRequests()
        {
            CreateMap<AmendReservationRequest, AmendReservation>();
        }

        public void MapResponses()
        {
            CreateMap<Availability, AvailabilityResponse>();
            CreateMap<Reservation, ReservationResponse>();
        }

        public void MapModels()
        {
            CreateMap<Core.Models.DatePeriods.DateRange, ApiModels.DateRange>()
                .ReverseMap();
        }

    }
}
