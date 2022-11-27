using System;
using System.Runtime.Serialization;

namespace ParkingReservation.Api.Exceptions
{
    [Serializable]
    internal class InvalidAppSettingsException : Exception
    {
        public InvalidAppSettingsException()
        {
        }

        public InvalidAppSettingsException(string message) : base(message)
        {
        }

        public InvalidAppSettingsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidAppSettingsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}