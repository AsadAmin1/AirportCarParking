using Newtonsoft.Json;
using System;

namespace ParkingReservation.Api.ApiModels
{
    [Serializable]
    public class ErrorDetails
    {
        #region Public Properties

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Code { get; }
        public string Description { get; }

        #endregion

        #region Constructors

        public ErrorDetails(string description, int? code = null)
        {
            Description = description;
            Code = code;
        }

        #endregion
    }
}
