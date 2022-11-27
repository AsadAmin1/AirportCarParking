using Microsoft.Extensions.Configuration;
using ParkingReservation.Api.Exceptions;

namespace ParkingReservation.Api.Models
{
    public class PricingConfig
    {
        #region Private Fields

        private const string MinimumAllowablePriceKey = "MinimumAllowablePrice";

        #endregion

        #region Public Properties

        public decimal MinimumAllowablePrice { get; }

        #endregion

        #region Constructors

        public PricingConfig(IConfiguration config)
        {
            var sectionName = nameof(PricingConfig);
            var cfg = config.GetSection(sectionName)
                ?? throw new InvalidAppSettingsException(sectionName);

            var value = cfg[MinimumAllowablePriceKey]
                    ?? throw new InvalidAppSettingsException(sectionName);

            if (!decimal.TryParse(value, out decimal MinimumAllowablePrice))
            {
                MinimumAllowablePrice = 10;
            }
        }

        #endregion
    }
}
