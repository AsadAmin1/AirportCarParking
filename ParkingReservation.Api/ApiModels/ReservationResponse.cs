using ParkingReservation.Api.Interfaces;
using ParkingReservation.Core.Interfaces;

namespace ParkingReservation.Api.ApiModels
{
    public class ReservationResponse : IErrorDetails
    {
        #region Public Properties

        public DateRange DateRange { get; set; }
        public decimal Price { get; set; }
        public ErrorDetails Error { get; set; }
        public string Reference { get; internal set; }
        public IBookable Item { get; internal set; }

        #endregion
    }
}
