using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ParkingReservation.Core.TestHelpers
{
    public class TestConfiguration
    {
        private static IConfiguration? _configuration;
        public static IConfiguration Get { get => _configuration ?? new ConfigurationBuilder().Build(); }

        public static void Setup()
        {
            var inMemorySettings = (IEnumerable<KeyValuePair<string,string?>>)new Dictionary<string, string> {
                {"PricingConfig:MinimumAllowablePrice", "10"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
    }
}