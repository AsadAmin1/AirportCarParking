using System.Runtime.Serialization;

namespace ParkingReservation.Core.Exceptions
{
    [Serializable]
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException()
        {
        }

        public BookingNotFoundException(string? message) : base(message)
        {
        }
    }
}