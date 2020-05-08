using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Pricing;
using EncoreTickets.SDK.Pricing.Models;
using EncoreTickets.SDK.Pricing.Models.RequestModels;
using EncoreTickets.SDK.Tests.Helpers;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    internal class PricingServiceTests
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

            AssertRatesAreValid(rates);
        }

        [Test]
        public void GetExchangeRates_CorrectlySorts()
        {
            var parameters = new ExchangeRatesParameters
            {
                Direction = Direction.Asc
            };

            var rates = service.GetExchangeRates(parameters);

            AssertRatesAreValid(rates);
            CollectionAssert.AreEqual(rates.OrderBy(r => r.DatetimeOfSourcing).Select(r => r.DatetimeOfSourcing).ToList(),
                rates.Select(r => r.DatetimeOfSourcing).ToList());
        }

        [Test]
        public void Authentication_Successful()
        {
            service.AuthenticationService.Authenticate();

            Assert.True(service.AuthenticationService.IsThereAuthentication());
        }

        [Test]
        public void Authentication_IfBadCredentials_Exception401()
        {
            var apiContext = new ApiContext(Environments.QA, "admin", "invalid_password");
            service = new PricingServiceApi(apiContext);

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.AuthenticationService.Authenticate();
            });

            Assert.AreEqual(HttpStatusCode.Unauthorized, exception.ResponseCode);
        }

        private void AssertRatesAreValid(IEnumerable<ExchangeRate> rates)
        {
            var rateList = rates.ToList();
            Assert.IsNotEmpty(rateList);
            foreach (var rate in rateList)
            {
                Assert.True(rate.Id > 0);
                Assert.NotNull(rate.BaseCurrency);
                Assert.NotNull(rate.TargetCurrency);
                Assert.True(rate.Rate > 0);
                Assert.True(rate.EncoreRate > 0);
                Assert.True(rate.ProtectionMargin >= 0);
            }
        }
    }
}
