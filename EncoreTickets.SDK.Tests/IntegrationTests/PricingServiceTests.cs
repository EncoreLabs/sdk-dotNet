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
                Affiliate = configuration["Pricing:AffiliateId"],
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
                Direction = Direction.Asc,
            };

            var rates = service.GetExchangeRates(parameters);

            AssertRatesAreValid(rates);
            CollectionAssert.AreEqual(
                rates.OrderBy(r => r.DatetimeOfSourcing).Select(r => r.DatetimeOfSourcing).ToList(),
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

            Assert.IsNotEmpty(priceBands);
            foreach (var priceBand in priceBands)
            {
                AssertPriceBandIsValid(priceBand, date);
            }
        }

        [Test]
        public void GetPriceBands_WithDisplayCurrency_Successful()
        {
            var date = DateTime.Now.AddMonths(3);
            const string displayCurrency = "USD";
            service.Context.DisplayCurrency = displayCurrency;

            var priceBands = service.GetPriceBands("1018", 2, date.Date);

            Assert.IsNotEmpty(priceBands);
            foreach (var priceBand in priceBands)
            {
                AssertPriceBandIsValid(priceBand, date);
                Assert.True(priceBand.SalePrice.Any(p => p.Currency == displayCurrency));
                Assert.True(priceBand.FaceValue.Any(p => p.Currency == displayCurrency));
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
        public void GetDailyPriceRanges_Successful()
        {
            var dateFrom = DateTime.Now.AddMonths(3);
            var dateTo = DateTime.Now.AddMonths(4);

            var priceRanges = service.GetDailyPriceRanges("1018", 2, dateFrom, dateTo);

            Assert.IsNotEmpty(priceRanges);
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

            Assert.IsNotEmpty(priceRanges);
            foreach (var priceRange in priceRanges)
            {
                Assert.NotNull(priceRange.MinPrice.FirstOrDefault());
                Assert.NotNull(priceRange.MaxPrice.FirstOrDefault());
                var resultDate = new DateTime(priceRange.Date.Year, priceRange.Date.Month, 1);
                Assert.GreaterOrEqual(dateTo.Date, resultDate);
                Assert.Less(dateFrom.Date, resultDate.AddMonths(1));
            }
        }

        [Test]
        public void GetPriceRuleSummaries_Successful()
        {
            var priceRules = service.GetPriceRuleSummaries();

            Assert.IsNotEmpty(priceRules);
            foreach (var priceRule in priceRules)
            {
                Assert.Less(0, priceRule.Id);
                Assert.NotNull(priceRule.Name);
            }
        }

        [Test]
        public void GetPriceRule_Successful()
        {
            const int id = 1;

            var priceRule = service.GetPriceRule(id);

            Assert.AreEqual(id, priceRule.Id);
            Assert.NotNull(priceRule.Name);
            Assert.IsNotEmpty(priceRule.Modifiers);
            Assert.IsNotEmpty(priceRule.Qualifiers);
        }

        [Test]
        public void GetPriceRule_InvalidRuleId_Exception404()
        {
            var exception = Assert.Catch<ApiException>(() =>
            {
                service.GetPriceRule(0);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
        }

        [Test]
        public void GetPartnerGroups_Successful()
        {
            var partnerGroups = service.GetPartnerGroups();

            Assert.IsNotEmpty(partnerGroups);
            foreach (var partnerGroup in partnerGroups)
            {
                Assert.Less(0, partnerGroup.Id);
                Assert.NotNull(partnerGroup.Name);
            }
        }

        [Test]
        public void GetPartnersInGroup_Successful()
        {
            const int id = 3;

            var partners = service.GetPartnersInGroup(id);

            Assert.IsNotEmpty(partners);
            foreach (var partner in partners)
            {
                AssertPartnerIsValid(partner);
            }
        }

        [Test]
        public void GetPartnersInGroup_InvalidGroupId_Exception404()
        {
            var exception = Assert.Catch<ApiException>(() =>
            {
                service.GetPartnersInGroup(0);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
        }

        [Test]
        public void GetPartner_Successful()
        {
            const int id = 35;

            var partner = service.GetPartner(id);

            AssertPartnerIsValid(partner);
            Assert.AreEqual(id, partner.Id);
            Assert.IsNotEmpty(partner.PartnerGroup.Partners);
        }

        [Test]
        public void GetPartner_InvalidPartnerId_Exception404()
        {
            var exception = Assert.Catch<ApiException>(() =>
            {
                service.GetPartner(0);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
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

        private void AssertPriceBandIsValid(PriceBand priceBand, DateTime requestedDate)
        {
            Assert.NotNull(priceBand.SalePrice.FirstOrDefault());
            Assert.NotNull(priceBand.FaceValue.FirstOrDefault());
            Assert.AreEqual(requestedDate.Date, priceBand.Date?.Date);
        }

        private void AssertPartnerIsValid(Partner partner)
        {
            Assert.Less(0, partner.Id);
            Assert.NotNull(partner.Name);
            Assert.NotNull(partner.OfficeId);
            Assert.NotNull(partner.CurrencyCode);
            Assert.NotNull(partner.DefaultDisplayCurrencyCode);
            Assert.NotNull(partner.Description);
        }
    }
}
