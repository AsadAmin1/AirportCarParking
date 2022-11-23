namespace ParkingReservation.Core
{
    [Serializable]
    public class ElapsedDateException: Exception
    {
        #region Constructors

        public ElapsedDateException(string? message): base(message)
        {}

        #endregion
    }
}