using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Tests.PriceRules;

namespace ParkingReservation.Core.TestHelpers
{
    public class TestPricingRules
    {
        public static List<IPriceRule> PriceRules
        {
            get => new()
            {
                new SummerPriceRule(),
                new WinterPriceRule()
            };
        }
    }
}
