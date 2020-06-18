using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Pricing;
using EncoreTickets.SDK.Pricing.Models;
using EncoreTickets.SDK.Pricing.Models.RequestModels;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using EncoreTickets.SDK.Utilities.Serializers;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Pricing
{
    internal class PricingServiceApiTests : PricingServiceApi
    {
        private const string NotFoundResponseContent = @"{
            ""request"": ""<irrelevant_json>"",
            ""response"": """",
            ""context"": {
                ""errors"": [
                    {
                        ""message"": ""no results are found""
                    }
                ]
            }
        }";

        private const string SuccessfulResponseTemplate = @"{{
            ""request"": {{
                ""body"": ""<irrelevant_json>"",
                ""query"": ""<irrelevant_json>"",
                ""urlParams"": ""<irrelevant_json>""
            }},
            ""response"": {0},
            ""context"": """"
        }}";

        private const string AffiliateIdHeader = "affiliateId";
        private const string DisplayCurrencyHeader = "x-display-currency";

        private ApiServiceMockerWithAuthentication mocker;
        private BaseJsonSerializer serializer;

        public override IAuthenticationService AuthenticationService => mocker.AuthenticationServiceMock.Object;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mocker.RestClientBuilderMock.Object);

        public PricingServiceApiTests()
            : base(new ApiContext(Environments.Sandbox))
        {
            serializer = new DefaultJsonSerializer();
        }

        [SetUp]
        public void CreateMockers()
        {
            mocker = new ApiServiceMockerWithAuthentication();
            AutomaticAuthentication = true;
            ApiContextTestHelper.ResetContextToDefault(Context);
        }

        [TestCase(null, null, null, null, true)]
        [TestCase(Direction.Asc, 10, 2, "datetimeOfSourcing", false)]
        public void GetExchangeRates_CorrectlyCalled(Direction? direction, int? limit, int? page, string sort, bool nullParameters)
        {
            var parameters = nullParameters
                ? null
                : new ExchangeRatesParameters
                {
                    Direction = direction,
                    Limit = limit,
                    Page = page,
                    Sort = sort,
                };

            TestActionCorrectlyExecuted<ResponseForPage<ExchangeRate>>(
                () => GetExchangeRates(parameters),
                $"v{ApiVersion}/admin/exchange_rates",
                Method.GET,
                expectedQueryParameters: nullParameters ? null : new Dictionary<string, object>
                {
                    { "direction", parameters.Direction.ToString() },
                    { "limit", parameters.Limit },
                    { "page", parameters.Page },
                    { "sort", parameters.Sort },
                },
                shouldAuthenticate: true);
        }

        [TestCaseSource(typeof(PricingServiceApiTestSource), nameof(GetExchangeRates_ReturnsExchangeRates))]
        public void GetExchangeRates_ReturnsExchangeRates(ResponseForPage<ExchangeRate> expectedResult)
        {
            TestSuccessfulAction(() => GetExchangeRates(null), expectedResult);
        }

        [Test]
        public void GetPriceBands_CorrectlyCalled()
        {
            const string productId = "id";
            const int quantity = 2;
            var date = DateTime.Now;
            Context.Affiliate = "test-affiliate";
            Context.DisplayCurrency = "USD";

            TestActionCorrectlyExecuted<List<PriceBand>, IList<PriceBand>>(
                () => GetPriceBands(productId, quantity, date),
                $"v{ApiVersion}/pricing/products/{productId}/quantity/{quantity}/bands",
                Method.GET,
                expectedQueryParameters: new Dictionary<string, object>
                    { { "date", date.ToEncoreDate() }, { "time", date.ToEncoreTime() } },
                expectedHeaders: new Dictionary<string, object>
                    { { AffiliateIdHeader, Context.Affiliate }, { DisplayCurrencyHeader, Context.DisplayCurrency } });
        }

        [TestCaseSource(typeof(PricingServiceApiTestSource), nameof(GetPriceBands_ReturnsPriceBands))]
        public void GetPriceBands_ReturnsPriceBands(IList<PriceBand> expectedResult)
        {
            TestSuccessfulAction(() => GetPriceBands("product_id", 2, DateTime.Now), expectedResult);
        }

        [Test]
        public void GetPriceBands_NullProductId_ArgumentException()
        {
            Assert.Catch<ArgumentException>(() => { GetPriceBands(null, 2, DateTime.Now); });
        }

        [Test]
        public void GetDailyPriceRanges_CorrectlyCalled()
        {
            const string productId = "id";
            const int quantity = 2;
            var fromDate = DateTime.Now;
            var toDate = fromDate.AddMonths(1);
            Context.Affiliate = "test-affiliate";

            TestActionCorrectlyExecuted<List<DailyPriceRange>, IList<DailyPriceRange>>(
                () => GetDailyPriceRanges(productId, quantity, fromDate, toDate),
                $"v{ApiVersion}/pricing/days/products/{productId}/quantity/{quantity}" +
                    $"/from/{fromDate.ToEncoreDate()}/to/{toDate.ToEncoreDate()}",
                Method.GET,
                expectedHeaders: new Dictionary<string, object> { { AffiliateIdHeader, Context.Affiliate } });
        }

        [TestCaseSource(typeof(PricingServiceApiTestSource), nameof(GetDailyPriceRanges_ReturnsPriceRanges))]
        public void GetDailyPriceRanges_ReturnsPriceRanges(IList<DailyPriceRange> expectedResult)
        {
            TestSuccessfulAction(
                () => GetDailyPriceRanges("product_id", 2, DateTime.Now, DateTime.Now.AddMonths(1)),
                expectedResult);
        }

        [Test]
        public void GetDailyPriceRanges_NullProductId_ArgumentException()
        {
            Assert.Catch<ArgumentException>(() => { GetDailyPriceRanges(null, 2, DateTime.Now, DateTime.Now.AddMonths(1)); });
        }

        [Test]
        public void GetDailyPriceRanges_NotFoundProduct_ThrowsApiException()
        {
            TestApiExceptionOnFailedAction<IList<DailyPriceRange>>(
                () => GetDailyPriceRanges("invalid_product", 1, DateTime.Now, DateTime.Now.AddDays(1)),
                NotFoundResponseContent,
                HttpStatusCode.NotFound);
        }

        [Test]
        public void GetMonthlyPriceRanges_CorrectlyCalled()
        {
            const string productId = "id";
            const int quantity = 2;
            var fromDate = DateTime.Now;
            var toDate = fromDate.AddMonths(1);
            Context.Affiliate = "test-affiliate";

            TestActionCorrectlyExecuted<List<MonthlyPriceRange>, IList<MonthlyPriceRange>>(
                () => GetMonthlyPriceRanges(productId, quantity, fromDate, toDate),
                $"v{ApiVersion}/pricing/months/products/{productId}/quantity/{quantity}" +
                    $"/from/{fromDate.ToEncoreDate()}/to/{toDate.ToEncoreDate()}",
                Method.GET,
                expectedHeaders: new Dictionary<string, object> { { AffiliateIdHeader, Context.Affiliate } });
        }

        [TestCaseSource(typeof(PricingServiceApiTestSource), nameof(GetMonthlyPriceRanges_ReturnsPriceRanges))]
        public void GetMonthlyPriceRanges_ReturnsPriceRanges(IList<MonthlyPriceRange> expectedResult)
        {
            TestSuccessfulAction(
                () => GetMonthlyPriceRanges("product_id", 2, DateTime.Now, DateTime.Now.AddMonths(1)),
                expectedResult);
        }

        [Test]
        public void GetMonthlyPriceRanges_NullProductId_ArgumentException()
        {
            Assert.Catch<ArgumentException>(() => { GetMonthlyPriceRanges(null, 2, DateTime.Now, DateTime.Now.AddMonths(1)); });
        }

        [Test]
        public void GetMontlhyPriceRanges_NotFoundProduct_ThrowsApiException()
        {
            TestApiExceptionOnFailedAction<IList<MonthlyPriceRange>>(
                () => GetMonthlyPriceRanges("invalid_product", 1, DateTime.Now, DateTime.Now.AddDays(1)),
                NotFoundResponseContent,
                HttpStatusCode.NotFound);
        }

        [Test]
        public void GetPriceRuleSummaries_CorrectlyCalled()
        {
            TestActionCorrectlyExecuted<List<PriceRuleSummary>, IList<PriceRuleSummary>>(
                () => GetPriceRuleSummaries(),
                $"v{ApiVersion}/admin/pricing/rules",
                Method.GET,
                shouldAuthenticate: true);
        }

        [TestCaseSource(typeof(PricingServiceApiTestSource), nameof(GetPriceRuleSummaries_ReturnsPriceRuleSummaries))]
        public void GetPriceRuleSummaries_ReturnsPriceRuleSummaries(IList<PriceRuleSummary> expectedResult)
        {
            TestSuccessfulAction(GetPriceRuleSummaries, expectedResult);
        }

        [Test]
        public void GetPriceRule_CorrectlyCalled()
        {
            const int id = 1;

            TestActionCorrectlyExecuted<PriceRule>(
                () => GetPriceRule(id),
                $"v{ApiVersion}/admin/pricing/rules/{id}",
                Method.GET,
                shouldAuthenticate: true);
        }

        [TestCaseSource(typeof(PricingServiceApiTestSource), nameof(GetPriceRule_ReturnsPriceRule))]
        public void GetPriceRule_ReturnsPriceRule(PriceRule expectedResult)
        {
            TestSuccessfulAction(() => GetPriceRule(1), expectedResult);
        }

        [Test]
        public void GetPriceRule_NotFoundRule_ThrowsApiException()
        {
            TestApiExceptionOnFailedAction<PriceRule>(
                () => GetPriceRule(-1),
                NotFoundResponseContent,
                HttpStatusCode.NotFound);
        }

        [Test]
        public void GetPartnerGroups_CorrectlyCalled()
        {
            TestActionCorrectlyExecuted<List<PartnerGroup>, IList<PartnerGroup>>(
                () => GetPartnerGroups(),
                $"v{ApiVersion}/admin/groups",
                Method.GET,
                shouldAuthenticate: true);
        }

        [TestCaseSource(typeof(PricingServiceApiTestSource), nameof(GetPartnerGroups_ReturnsPartnerGroups))]
        public void GetPartnerGroups_ReturnsPartnerGroups(IList<PartnerGroup> expectedResult)
        {
            TestSuccessfulAction(GetPartnerGroups, expectedResult);
        }

        [Test]
        public void GetPartnersInGroup_CorrectlyCalled()
        {
            const int groupId = 1;

            TestActionCorrectlyExecuted<List<Partner>, IList<Partner>>(
                () => GetPartnersInGroup(groupId),
                $"v{ApiVersion}/admin/groups/{groupId}/partners",
                Method.GET,
                shouldAuthenticate: true);
        }

        [TestCaseSource(typeof(PricingServiceApiTestSource), nameof(GetPartnersInGroup_ReturnsPartners))]
        public void GetPartnersInGroup_ReturnsPartners(IList<Partner> expectedResult)
        {
            TestSuccessfulAction(() => GetPartnersInGroup(1), expectedResult);
        }

        [Test]
        public void GetPartnersInGroup_NotFoundGroup_ThrowsApiException()
        {
            TestApiExceptionOnFailedAction<IList<Partner>>(
                () => GetPartnersInGroup(-1),
                NotFoundResponseContent,
                HttpStatusCode.NotFound);
        }

        [Test]
        public void GetPartner_CorrectlyCalled()
        {
            const int id = 1;

            TestActionCorrectlyExecuted<Partner>(
                () => GetPartner(id),
                $"v{ApiVersion}/admin/partners/{id}",
                Method.GET,
                shouldAuthenticate: true);
        }

        [TestCaseSource(typeof(PricingServiceApiTestSource), nameof(GetPartner_ReturnsPartner))]
        public void GetPartner_ReturnsPartner(Partner expectedResult)
        {
            TestSuccessfulAction(() => GetPartner(1), expectedResult);
        }

        [Test]
        public void GetPartner_NotFoundPartner_ThrowsApiException()
        {
            TestApiExceptionOnFailedAction<Partner>(
                () => GetPartner(-1),
                NotFoundResponseContent,
                HttpStatusCode.NotFound);
        }

        private void TestActionCorrectlyExecuted<T>(
            Action action,
            string endpoint,
            Method method,
            Dictionary<string, object> expectedQueryParameters = null,
            Dictionary<string, object> expectedHeaders = null,
            bool shouldAuthenticate = false)
            where T : class, new()
        {
            TestActionCorrectlyExecuted<T, T>(
                action,
                endpoint,
                method,
                expectedQueryParameters,
                expectedHeaders,
                shouldAuthenticate);
        }

        private void TestActionCorrectlyExecuted<TSetup, TResult>(
            Action action,
            string endpoint,
            Method method,
            Dictionary<string, object> expectedQueryParameters = null,
            Dictionary<string, object> expectedHeaders = null,
            bool shouldAuthenticate = false)
            where TSetup : class, TResult, new()
            where TResult : class
        {
            mocker.SetupAnyExecution<TSetup>();

            try
            {
                action();
            }
            catch (Exception)
            {
                // ignored
            }

            if (shouldAuthenticate)
            {
                ShouldAuthenticate();
            }
            else
            {
                ShouldNotAuthenticate();
            }

            mocker.VerifyExecution<ApiResponse<TResult>>(
                BaseUrl,
                endpoint,
                method,
                expectedQueryParameters: expectedQueryParameters,
                expectedHeaders: expectedHeaders);
        }

        private void TestSuccessfulAction<T>(Func<T> action, T expectedResult)
            where T : class
        {
            var serializedExpectedResult = serializer.Serialize(expectedResult);
            var responseContent = string.Format(SuccessfulResponseTemplate, serializedExpectedResult);
            mocker.SetupSuccessfulExecution<ApiResponse<T>>(responseContent);

            var result = action();

            AssertExtension.AreObjectsValuesEqual(expectedResult, result);
        }

        private void TestApiExceptionOnFailedAction<T>(
            Action action,
            string responseContent,
            HttpStatusCode code)
            where T : class
        {
            mocker.SetupFailedExecution<ApiResponse<T>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                action();
            });

            Assert.AreEqual(code, exception.ResponseCode);
        }

        private void ShouldAuthenticate()
        {
            mocker.VerifyAuthenticateExecution(Times.Once());
        }

        private void ShouldNotAuthenticate()
        {
            mocker.VerifyAuthenticateExecution(Times.Never());
        }
    }

    internal static class PricingServiceApiTestSource
    {
        #region ExchangeRates

        public static IEnumerable<TestCaseData> GetExchangeRates_ReturnsExchangeRates { get; } = new[]
        {
            new TestCaseData(
                new ResponseForPage<ExchangeRate>()
                {
                    PageCount = 1,
                    CurrentPage = 1,
                    ItemsPerPage = 10,
                    TotalItemCount = 10,
                    Items = new List<ExchangeRate>
                    {
                        new ExchangeRate
                        {
                            BaseCurrency = "GBP",
                            CreatedAt = DateTimeOffset.Now,
                            DatetimeOfSourcing = DateTimeOffset.Now,
                            EncoreRate = 10.5m,
                            Id = 1,
                            ProtectionMargin = 12,
                            Rate = 1.47m,
                            Sourced = 1,
                            TargetCurrency = "USD",
                            UpdatedAt = DateTimeOffset.Now,
                        },
                        new ExchangeRate
                        {
                            BaseCurrency = "GBP",
                            CreatedAt = DateTimeOffset.Now,
                            DatetimeOfSourcing = DateTimeOffset.Now,
                            EncoreRate = 12.7m,
                            Id = 2,
                            ProtectionMargin = 12,
                            Rate = 158m,
                            Sourced = 1,
                            TargetCurrency = "JPY",
                            UpdatedAt = DateTimeOffset.Now,
                        },
                    },
                }),
        };

        #endregion

        #region PriceBands

        public static IEnumerable<TestCaseData> GetPriceBands_ReturnsPriceBands { get; } = new[]
        {
            new TestCaseData(
                new List<PriceBand>
                {
                    new PriceBand
                    {
                        CreatedAt = DateTimeOffset.Now,
                        Date = DateTimeOffset.Now.AddMonths(1),
                        DisplayCurrency = "USD",
                        FaceValue = new List<Price>
                        {
                            new Price
                            {
                                Currency = "GBP",
                                DecimalPlaces = 2,
                                Value = 100,
                            },
                            new Price
                            {
                                Currency = "USD",
                                DecimalPlaces = 2,
                                Value = 147,
                            },
                        },
                        SalePrice = new List<Price>
                        {
                            new Price
                            {
                                Currency = "GBP",
                                DecimalPlaces = 2,
                                Value = 200,
                            },
                            new Price
                            {
                                Currency = "USD",
                                DecimalPlaces = 2,
                                Value = 294,
                            },
                        },
                    },
                }),
        };

        #endregion

        #region PriceRanges

        private static readonly IEnumerable<PriceRange> TestPriceRanges = new List<PriceRange>
        {
            new PriceRange
            {
                CreatedAt = DateTimeOffset.Now,
                DisplayCurrency = "GBP",
                IncludesBookingFee = true,
                MinPrice = new List<Price>
                {
                    new Price
                    {
                        Currency = "GBP",
                        DecimalPlaces = 2,
                        Value = 100,
                    },
                },
                MaxPrice = new List<Price>
                {
                    new Price
                    {
                        Currency = "GBP",
                        DecimalPlaces = 2,
                        Value = 200,
                    },
                },
            },
        };

        private static List<DailyPriceRange> CreateTestDailyRanges()
        {
            return TestPriceRanges.Select(r =>
            {
                var dailyRange = r.CopyObjectToChildClass<PriceRange, DailyPriceRange>();
                dailyRange.Date = DateTimeOffset.Now;
                return dailyRange;
            }).ToList();
        }

        private static List<MonthlyPriceRange> CreateTestMonthlyRanges()
        {
            return TestPriceRanges.Select(r =>
            {
                var monthlyRange = r.CopyObjectToChildClass<PriceRange, MonthlyPriceRange>();
                monthlyRange.Date = new YearMonthDate
                {
                    Month = DateTimeOffset.Now.Month,
                    Year = DateTimeOffset.Now.Year,
                };
                return monthlyRange;
            }).ToList();
        }

        public static IEnumerable<TestCaseData> GetDailyPriceRanges_ReturnsPriceRanges { get; } = new[]
        {
            new TestCaseData(CreateTestDailyRanges()),
        };

        public static IEnumerable<TestCaseData> GetMonthlyPriceRanges_ReturnsPriceRanges { get; } = new[]
        {
            new TestCaseData(CreateTestMonthlyRanges()),
        };

        #endregion

        #region PriceRules

        public static IEnumerable<TestCaseData> GetPriceRuleSummaries_ReturnsPriceRuleSummaries { get; } = new[]
        {
            new TestCaseData(
                new List<PriceRuleSummary>
                {
                    new PriceRuleSummary
                    {
                        Active = 1,
                        Id = 1,
                        Name = "rule1",
                    },
                    new PriceRuleSummary
                    {
                        Active = 1,
                        Id = 2,
                        Name = "rule2",
                    },
                }),
        };

        public static IEnumerable<TestCaseData> GetPriceRule_ReturnsPriceRule { get; } = new[]
        {
            new TestCaseData(
                new PriceRule
                {
                    Active = 1,
                    CreatedAt = DateTimeOffset.Now,
                    Id = 1,
                    UpdatedAt = DateTimeOffset.Now,
                    Name = "r1",
                    Type = 2,
                    Weight = 3,
                    Modifiers = new List<PriceModifier>
                    {
                        new PriceModifier
                        {
                            AdjustmentType = "percent",
                            AdjustmentValue = 10,
                            CreatedAt = DateTimeOffset.Now,
                            UpdatedAt = DateTimeOffset.Now,
                            Id = 1,
                            Mode = 1,
                            RoundingPrecision = 2,
                            RoundingType = "up",
                            Weight = 4,
                        },
                    },
                    Qualifiers = new List<PriceQualifier>
                    {
                        new PriceQualifier
                        {
                            CreatedAt = DateTimeOffset.Now,
                            UpdatedAt = DateTimeOffset.Now,
                            Id = 1,
                            PublishedAt = DateTimeOffset.Now,
                            Type = "1",
                            Properties = new List<PriceProperty>
                            {
                                new PriceProperty
                                {
                                    CreatedAt = DateTimeOffset.Now,
                                    UpdatedAt = DateTimeOffset.Now,
                                    Id = 1,
                                    Name = "p1",
                                    Type = "1",
                                    Value = "100",
                                },
                            },
                        },
                    },
                }),
        };

        #endregion

        #region PartnersAndGroups

        private static readonly List<PartnerGroup> PartnerGroups = new List<PartnerGroup>
        {
            new PartnerGroup
            {
                Id = 1,
                Name = "group1",
            },
            new PartnerGroup
            {
                Id = 2,
                Name = "group2",
            },
        };

        private static readonly List<Partner> Partners = new List<Partner>
        {
            new Partner
            {
                CurrencyCode = "GBP",
                DefaultDisplayCurrencyCode = "USD",
                Description = "description",
                Id = 1,
                Name = "partner1",
                OfficeId = "32",
                PartnerGroup = PartnerGroups[0],
            },
            new Partner
            {
                CurrencyCode = "GBP",
                DefaultDisplayCurrencyCode = "GBP",
                Description = "description",
                Id = 2,
                Name = "partner2",
                OfficeId = "66",
                PartnerGroup = PartnerGroups[0],
            },
        };

        public static IEnumerable<TestCaseData> GetPartnerGroups_ReturnsPartnerGroups { get; } = new[]
        {
            new TestCaseData(PartnerGroups),
        };

        public static IEnumerable<TestCaseData> GetPartnersInGroup_ReturnsPartners { get; } = new[]
        {
            new TestCaseData(Partners),
        };

        public static IEnumerable<TestCaseData> GetPartner_ReturnsPartner { get; } = new[]
        {
            new TestCaseData(Partners[0]),
        };

        #endregion
    }
}