using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Core
{
    public class PricingService : IPricingService
    {
        #region Private Fields

        private readonly List<IPriceRule> _priceRules;

        #endregion

        #region Constructors

        public PricingService(List<IPriceRule> priceRules)
        {
            _priceRules = priceRules;
        }

        #endregion

        #region Public Methods

        public decimal GetPrice(DateRange dateRange)
        {
            return _priceRules.Sum(r => r.ApplyRule(dateRange));
        }

        #endregion
    }
}