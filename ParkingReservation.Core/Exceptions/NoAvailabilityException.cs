namespace ParkingReservation.Core.Exceptions
{
    [Serializable]
    public class NoAvailabilityException : Exception
    {
        public NoAvailabilityException(string? message) : base(message)
        {
        }
    }
}