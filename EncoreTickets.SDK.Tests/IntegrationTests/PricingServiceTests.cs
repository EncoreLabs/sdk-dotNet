using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Pricing;
using EncoreTickets.SDK.Tests.Helpers;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    class PricingServiceTests
    {
        private PricingServiceApi service;

        [SetUp]
        public void SetupState()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            var context = new ApiContext(Environments.QA, configuration["Pricing:Username"], configuration["Pricing:Password"]);
            service = new PricingServiceApi(context, true);
        }

        [Test]
        public void GetExchangeRates_Successful()
        {
            var rates = service.GetExchangeRates(null);

            Assert.IsNotEmpty(rates);
            foreach (var rate in rates)
            {
                Assert.NotNull(rate.BaseCurrency);
                Assert.NotNull(rate.TargetCurrency);
                Assert.True(rate.Rate > 0);
                Assert.True(rate.EncoreRate > 0);
                Assert.True(rate.ProtectionMargin >= 0);
            }
        }
    }
}
