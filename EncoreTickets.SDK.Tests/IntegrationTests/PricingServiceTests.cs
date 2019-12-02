using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Pricing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    class PricingServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly PricingServiceApi service;

        public PricingServiceTests()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            var context = new ApiContext(Environments.QA, configuration["Pricing:Username"], configuration["Pricing:Password"]);
            service = new PricingServiceApi(context);
        }

        [Test]
        public void GetExchangeRates_Successful()
        {
            var rates = service.GetExchangeRates(null);

            Assert.IsNotEmpty(rates);
            foreach (var rate in rates)
            {
                Assert.NotNull(rate.baseCurrency);
                Assert.NotNull(rate.targetCurrency);
                Assert.True(rate.rate > 0);
                Assert.True(rate.encoreRate > 0);
                Assert.True(rate.protectionMargin >= 0);
            }
        }
    }
}
