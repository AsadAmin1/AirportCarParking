using ParkingReservation.Api.Models;
using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models.DatePeriods;

namespace ParkingReservation.Core
{
    public class PricingService : IPricingService
    {
        #region Private Fields

        private readonly List<IPriceRule> _priceRules;
        private readonly PricingConfig pricingConfig;

        #endregion

        #region Constructors

        public PricingService(List<IPriceRule> priceRules, PricingConfig pricingConfig)
        {
            _priceRules = priceRules;
            this.pricingConfig = pricingConfig;
        }

        #endregion

        #region Public Methods

        public decimal GetPrice(DateRange dateRange)
        {
            var price = _priceRules.Sum(r => r.ApplyRule(dateRange));
            return price > pricingConfig.MinimumAllowablePrice
                ? price
                : pricingConfig.MinimumAllowablePrice;
        }

        #endregion
    }
}