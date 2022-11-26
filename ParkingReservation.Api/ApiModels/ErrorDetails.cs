using Newtonsoft.Json;
using System;

namespace ParkingReservation.Api.ApiModels
{
    [Serializable]
    public class ErrorDetails
    {
        #region Public Properties

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Code { get; set; }
        public string Description { get; set;  }

        #endregion

        public static ErrorDetails New(string description, int? code = null)
        {
            return new ErrorDetails { Code = code, Description = description };
        }
    }
}
