namespace ParkingReservation.Core.Exceptions
{
    [Serializable]
    public class InvalidDatesException : Exception
    {
        #region Constructors

        public InvalidDatesException(string? message) : base(message)
        {}

        #endregion
    }
}