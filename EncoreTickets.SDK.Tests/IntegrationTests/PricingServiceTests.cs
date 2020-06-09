using System;
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
            var context = new ApiContext(Environments.QA, configuration["Pricing:Username"], configuration["Pricing:Password"])
            {
                Affiliate = "boxoffice"
            };
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

        [Test]
        public void GetPriceBands_Successful()
        {
            var date = DateTime.Now.AddMonths(3);

            var priceBands = service.GetPriceBands("1018", 2, date.Date);

            Assert.NotNull(priceBands);
            foreach (var priceBand in priceBands)
            {
                Assert.NotNull(priceBand.SalePrice.FirstOrDefault());
                Assert.NotNull(priceBand.FaceValue.FirstOrDefault());
                Assert.AreEqual(date.Date, priceBand.Date?.Date);
            }
        }

        [Test]
        public void GetPriceBands_IncorrectTimeSpecified_Exception404()
        {
            var date = DateTime.Now.AddMonths(3).Date + new TimeSpan(13, 37, 0);

            var exception = Assert.Catch<ApiException>(() =>
            { 
                service.GetPriceBands("1018", 2, date);
            });
            
            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
        }

        [Test]
        public void GetPriceBands_NullProductId_ArgumentException()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                service.GetPriceBands(null, 2, DateTime.Now);
            });
        }

        [Test]
        public void GetDailyPriceRanges_Successful()
        {
            var dateFrom = DateTime.Now.AddMonths(3);
            var dateTo = DateTime.Now.AddMonths(4);

            var priceRanges = service.GetDailyPriceRanges("1018", 2, dateFrom, dateTo);

            Assert.NotNull(priceRanges);
            foreach (var priceRange in priceRanges)
            {
                Assert.NotNull(priceRange.MinPrice.FirstOrDefault());
                Assert.NotNull(priceRange.MaxPrice.FirstOrDefault());
                Assert.GreaterOrEqual(dateTo.Date, priceRange.Date?.Date);
                Assert.LessOrEqual(dateFrom.Date, priceRange.Date?.Date);
            }
        }

        [Test]
        public void GetMonthlyPriceRanges_Successful()
        {
            var dateFrom = DateTime.Now.AddMonths(3);
            var dateTo = DateTime.Now.AddMonths(4);

            var priceRanges = service.GetMonthlyPriceRanges("1018", 2, dateFrom, dateTo);

            Assert.NotNull(priceRanges);
            foreach (var priceRange in priceRanges)
            {
                Assert.NotNull(priceRange.MinPrice.FirstOrDefault());
                Assert.NotNull(priceRange.MaxPrice.FirstOrDefault());
                var resultDate = new DateTime(priceRange.Date.Year, priceRange.Date.Month, 1);
                Assert.GreaterOrEqual(dateTo.Date, resultDate);
                Assert.Less(dateFrom.Date, resultDate.AddMonths(1));
            }
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
