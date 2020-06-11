using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Inventory;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Inventory.Models.RequestModels;
using EncoreTickets.SDK.Inventory.Models.ResponseModels;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Inventory
{
    internal class InventoryServiceApiTests : InventoryServiceApi
    {
        private const string TestValidSearchText = "wicked";
        private const string TestValidProductId = "1587";
        private const string CorrelationIdHeader = "X-Correlation-Id";
        private const string AffiliateIdHeader = "affiliateId";
        private const string MarketHeader = "x-market";

        private ApiServiceMocker mockers;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public InventoryServiceApiTests() : base(ApiContextTestHelper.DefaultApiContext)
        {
        }

        [SetUp]
        public void CreateMockers()
        {
            mockers = new ApiServiceMocker();
            ApiContextTestHelper.ResetContextToDefault(Context);
        }

        #region SearchProducts

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void SearchProducts_IfTextIsNotSet_ThrowsArgumentException(string text)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                SearchProducts(text);
            });
        }

        [TestCase("w")]
        [TestCase("broadway")]
        [TestCase("singin in the rain")]
        public void SearchProducts_IfTextIsSet_CallsApiWithRightParameters(string text)
        {
            Context.Affiliate = "boxoffice";
            Context.Correlation = "30435ee1-c0ce-4664-85b9-cf5402f20e83";
            mockers.SetupAnyExecution<ProductSearchResponse>();

            try
            {
                SearchProducts(text);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ProductSearchResponse>(
                BaseUrl,
                "v4/search",
                Method.GET,
                expectedQueryParameters: new Dictionary<string, object> { { "query", text } },
                expectedHeaders: new Dictionary<string, object>
                {
                    { AffiliateIdHeader, Context.Affiliate },
                    { CorrelationIdHeader, Context.Correlation },
                });
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.SearchProducts_IfApiResponseSuccessful_ReturnsProducts))]
        public void SearchProducts_IfApiResponseSuccessful_ReturnsProducts(
            string responseContent,
            List<Product> expected)
        {
            mockers.SetupSuccessfulExecution<ProductSearchResponse>(responseContent);

            var actual = SearchProducts(TestValidSearchText);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.SearchProducts_IfApiResponseFailed_ThrowsApiException))]
        public void SearchProducts_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string message)
        {
            mockers.SetupFailedExecution<ProductSearchResponse>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = SearchProducts(TestValidSearchText);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(message, exception.Message);
        }

        #endregion

        #region GetAvailabilityRange

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void GetAvailabilityRange_IfProductIdIsNotSet_ThrowsArgumentException(string productId)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetAvailabilityRange(productId);
            });
        }

        [TestCase("1587")]
        [TestCase("-1587")]
        [TestCase("some_id")]
        public void GetAvailabilityRange_IfProductIdIsSet_CallsApiWithRightParameters(string productId)
        {
            Context.Affiliate = "boxoffice";
            Context.Correlation = "30435ee1-c0ce-4664-85b9-cf5402f20e83";
            mockers.SetupAnyExecution<ApiResponse<AvailabilityRange>>();

            try
            {
                GetAvailabilityRange(productId);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<AvailabilityRange>>(
                BaseUrl,
                $"v4/products/{productId}/availability-range",
                Method.GET,
                expectedHeaders: new Dictionary<string, object>
                {
                    { AffiliateIdHeader, Context.Affiliate },
                    { CorrelationIdHeader, Context.Correlation },
                });
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetAvailabilityRange_IfApiResponseSuccessful_ReturnsBookingRange))]
        public void GetAvailabilityRange_IfApiResponseSuccessful_ReturnsBookingRange(
            string responseContent,
            AvailabilityRange expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<AvailabilityRange>>(responseContent);

            var actual = GetAvailabilityRange(TestValidProductId);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetAvailabilityRange_IfApiResponseFailed_ThrowsApiException))]
        public void GetAvailabilityRange_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string message)
        {
            mockers.SetupFailedExecution<ApiResponse<AvailabilityRange>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetAvailabilityRange(TestValidProductId);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(message, exception.Message);
        }

        #endregion

        #region GetAvailabilities

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetAvailabilities_IfProductIdIsNotSet_ThrowsArgumentException(string productId)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetAvailabilities(productId, It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());
            });
        }

        [TestCase("1587", 2, "1/10/2020", "11/12/2020")]
        [TestCase("test_id", 1, "1/10/2020", "11/12/2020")]
        public void GetAvailabilities_IfProductIdIsSet_CallsApiWithRightParameters(string productId, int quantity, string fromAsStr, string toAsStr)
        {
            Context.Affiliate = "boxoffice";
            Context.Correlation = "30435ee1-c0ce-4664-85b9-cf5402f20e83";
            Context.Market = Market.Broadway;
            var from = TestHelper.ConvertTestArgumentToDateTime(fromAsStr);
            var to = TestHelper.ConvertTestArgumentToDateTime(toAsStr);
            mockers.SetupAnyExecution<ApiResponse<List<Availability>>>();

            try
            {
                GetAvailabilities(productId, quantity, from, to);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<List<Availability>>>(
                BaseUrl,
                $"v4/availability/products/{productId}/quantity/{quantity}/from/{from:yyyyMMdd}/to/{to:yyyyMMdd}",
                Method.GET,
                expectedHeaders: new Dictionary<string, object>
                {
                    { AffiliateIdHeader, Context.Affiliate },
                    { CorrelationIdHeader, Context.Correlation },
                    { MarketHeader, "Broadway" },
                });
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetAvailabilities_IfApiResponseSuccessful_ReturnsPerformances))]
        public void GetAvailabilities_IfApiResponseSuccessful_ReturnsPerformances(
            string responseContent,
            List<Availability> expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<List<Availability>>>(responseContent);

            var actual = GetAvailabilities(TestValidProductId, It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetAvailabilities_IfApiResponseFailed_ThrowsApiException))]
        public void GetAvailabilities_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string message)
        {
            mockers.SetupFailedExecution<ApiResponse<List<Availability>>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetAvailabilities(TestValidProductId, It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(message, exception.Message);
        }

        #endregion

        #region GetAggregateSeatAvailability

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetAggregateSeatAvailability_IfProductIdIsNotSet_ThrowsArgumentException(string productId)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetAggregateSeatAvailability(productId, It.IsAny<AggregateSeatAvailabilityParameters>());
            });
        }

        [Test]
        public void GetAggregateSeatAvailability_IfParametersAreNotSet_ThrowsArgumentException()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetAggregateSeatAvailability(TestValidProductId, It.IsAny<AggregateSeatAvailabilityParameters>());
            });
        }

        [TestCase("1587", 2, "1/10/2020 3:56:51 PM", null)]
        [TestCase("1587", 2, "1/10/2020 3:56:51 PM", Direction.Asc)]
        [TestCase("1587", 2, "1/10/2020 3:56:51 PM", Direction.Desc)]
        [TestCase("1587", 2, "1/10/2020 3:56:51 PM", (Direction) 100)]
        public void GetAggregateSeatAvailability_IfProductIdAndQuantityAndPerformanceAreSet_CallsApiWithRightParameters(
            string productId, int quantity, string dateAsStr, Direction? direction)
        {
            Context.Affiliate = "boxoffice";
            Context.Correlation = "30435ee1-c0ce-4664-85b9-cf5402f20e83";
            var parameters = new AggregateSeatAvailabilityParameters
            {
                PerformanceTime = TestHelper.ConvertTestArgumentToDateTime(dateAsStr),
                Direction = direction,
                Quantity = quantity
            };
            var queryParameters = new Dictionary<string, object>
            {
                {"quantity", quantity},
                {"date", parameters.PerformanceTime.ToString("yyyyMMdd")},
                {"time", parameters.PerformanceTime.ToString("HHmm")},
            };

            if (parameters.Direction != null && (parameters.Direction == Direction.Asc || parameters.Direction == Direction.Desc))
            {
                queryParameters.Add("direction", parameters.Direction == Direction.Asc ? "asc" : "desc");
            }

            mockers.SetupAnyExecution<ApiResponse<AggregateSeatAvailability>>();

            try
            {
                GetAggregateSeatAvailability(productId, parameters);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<AggregateSeatAvailability>>(
                BaseUrl,
                $"v{ApiVersion}/products/{productId}/areas",
                Method.GET,
                expectedQueryParameters: queryParameters,
                expectedHeaders: new Dictionary<string, object>
                {
                    {AffiliateIdHeader, Context.Affiliate},
                    {CorrelationIdHeader, Context.Correlation},
                });
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetAggregateSeatAvailability_IfApiResponseSuccessful_ReturnsAvailability))]
        public void GetAggregateSeatAvailability_IfApiResponseSuccessful_ReturnsAvailability(
            string responseContent,
            AggregateSeatAvailability expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<AggregateSeatAvailability>>(responseContent);

            var actual = GetAggregateSeatAvailability(TestValidProductId, new AggregateSeatAvailabilityParameters());

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetAggregateSeatAvailability_IfApiResponseFailed_ThrowsApiException))]
        public void GetAggregateSeatAvailability_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string message)
        {
            mockers.SetupFailedExecution<ApiResponse<AggregateSeatAvailability>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetAggregateSeatAvailability(TestValidProductId, new AggregateSeatAvailabilityParameters());
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(message, exception.Message);
        }

        #endregion

        #region GetSeatAvailability

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        [Obsolete]
        public void GetSeatAvailability_IfTextIsNotSet_ThrowsArgumentException(string productId)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetSeatAvailability(productId, It.IsAny<int>());
            });
        }

        [TestCase("1587", 2)]
        [Obsolete]
        public void GetSeatAvailability_IfProductIdAndQuantityAreSet_CallsApiWithRightParameters(string productId, int quantity)
        {
            Context.Affiliate = "boxoffice";
            Context.Correlation = "30435ee1-c0ce-4664-85b9-cf5402f20e83";
            mockers.SetupAnyExecution<ApiResponse<SeatAvailability>>();

            try
            {
                GetSeatAvailability(productId, quantity);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SeatAvailability>>(
                BaseUrl,
                $"v4/europa/availability/products/{productId}/quantity/{quantity}/seats",
                Method.GET,
                expectedQueryParameters: new Dictionary<string, object>(),
                expectedHeaders: new Dictionary<string, object>
                {
                    { AffiliateIdHeader, Context.Affiliate },
                    { CorrelationIdHeader, Context.Correlation },
                });
        }

        [TestCase("1587", 2, "1/10/2020 3:56:51 PM")]
        [Obsolete]
        public void GetSeatAvailability_IfProductIdAndQuantityAndPerformanceAreSet_CallsApiWithRightParameters(
            string productId, int quantity, string dateAsStr)
        {
            Context.Affiliate = "boxoffice";
            Context.Correlation = "30435ee1-c0ce-4664-85b9-cf5402f20e83";
            var performance = TestHelper.ConvertTestArgumentToDateTime(dateAsStr);
            mockers.SetupAnyExecution<ApiResponse<SeatAvailability>>();

            try
            {
                GetSeatAvailability(productId, quantity, performance);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SeatAvailability>>(
                BaseUrl,
                $"v4/europa/availability/products/{productId}/quantity/{quantity}/seats",
                Method.GET,
                expectedQueryParameters: new Dictionary<string, object>
                {
                    {"date", performance.ToString("yyyyMMdd")},
                    {"time", performance.ToString("HHmm")},
                },
                expectedHeaders: new Dictionary<string, object>
                {
                    {AffiliateIdHeader, Context.Affiliate},
                    {CorrelationIdHeader, Context.Correlation},
                });
        }

        [TestCase("1587", 2, "1/10/2020 3:56:51 PM", null, null, 0)]
        [TestCase("1587", 2, "1/10/2020 3:56:51 PM", Direction.Asc, null, 0)]
        [TestCase("1587", 2, "1/10/2020 3:56:51 PM", Direction.Desc, null, 0)]
        [TestCase("1587", 2, "", null, "id", 0)]
        [TestCase("1587", 2, "1/10/2020 3:56:51 PM", null, null, -1)]
        [TestCase("1587", 2, null, null, null, 10)]
        [TestCase("1587", 2, null, Direction.Asc, "id", 1)]
        [TestCase("1587", 2, "1/10/2020 3:56:51 PM", Direction.Asc, "id", 1)]
        [Obsolete]
        public void GetSeatAvailability_IfProductIdAndQuantityAndPerformanceAreSet_CallsApiWithRightParameters(
            string productId, int quantity, string dateAsStr, Direction? direction, string sort, int groupingLimit)
        {
            Context.Affiliate = "boxoffice";
            Context.Correlation = "30435ee1-c0ce-4664-85b9-cf5402f20e83";
            var parameters = new SeatAvailabilityParameters
            {
                PerformanceTime = string.IsNullOrWhiteSpace(dateAsStr)
                    ? null
                    : (DateTime?)TestHelper.ConvertTestArgumentToDateTime(dateAsStr),
                Direction = direction,
                Sort = sort,
                GroupingLimit = groupingLimit
            };
            var queryParameters = new Dictionary<string, object>();
            if (parameters.PerformanceTime != null)
            {
                queryParameters.Add("date", parameters.PerformanceTime.Value.ToString("yyyyMMdd"));
            }

            if (parameters.PerformanceTime != null)
            {
                queryParameters.Add("time", parameters.PerformanceTime.Value.ToString("HHmm"));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Sort))
            {
                queryParameters.Add("sort", parameters.Sort);
            }

            if (parameters.Direction != null)
            {
                queryParameters.Add("direction", parameters.Direction.ToString().ToLower());
            }

            if (parameters.GroupingLimit > 0)
            {
                queryParameters.Add("groupingLimit", parameters.GroupingLimit);
            }

            mockers.SetupAnyExecution<ApiResponse<SeatAvailability>>();

            try
            {
                GetSeatAvailability(productId, quantity, parameters);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SeatAvailability>>(
                BaseUrl,
                $"v4/europa/availability/products/{productId}/quantity/{quantity}/seats",
                Method.GET,
                expectedQueryParameters: queryParameters,
                expectedHeaders: new Dictionary<string, object>
                {
                    {AffiliateIdHeader, Context.Affiliate},
                    {CorrelationIdHeader, Context.Correlation},
                });
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetSeatAvailability_IfApiResponseSuccessful_ReturnsAvailability))]
        [Obsolete]
        public void GetSeatAvailabilityy_IfApiResponseSuccessful_ReturnsAvailability(
            string responseContent,
            SeatAvailability expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<SeatAvailability>>(responseContent);

            var actual = GetSeatAvailability(TestValidProductId, It.IsAny<int>());

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetSeatAvailability_IfApiResponseFailed_ThrowsApiException))]
        [Obsolete]
        public void GetSeatAvailability_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string message)
        {
            mockers.SetupFailedExecution<ApiResponse<SeatAvailability>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetSeatAvailability(TestValidProductId, It.IsAny<int>());
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(message, exception.Message);
        }

        #endregion
    }

    public static class InventoryServiceApiTestsSource
    {
        #region SearchProducts

        public static IEnumerable<TestCaseData> SearchProducts_IfApiResponseSuccessful_ReturnsProducts = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""query"": ""wicked""
        },
        ""urlParams"": {}
    },
    ""response"": {
        ""product"": [
            {
                ""id"": 1587,
                ""name"": ""Wicked"",
                ""type"": ""show"",
                ""venue"": {
                    ""id"": ""138""
                },
                ""onSale"": ""yes"",
                ""bookingStarts"": ""2019-08-13T00:00:00+0000"",
                ""bookingEnds"": ""2020-05-23T00:00:00+0000""
            }
        ]
    },
    ""context"": null
}",
                new List<Product>
                {
                    new Product
                    {
                        Id = 1587,
                        Name = "Wicked",
                        Type = "show",
                        Venue = new SDK.Inventory.Models.Venue
                        {
                            Id = "138"
                        },
                        OnSale = "yes",
                        BookingStarts = new DateTime(2019, 08, 13),
                        BookingEnds = new DateTime(2020, 05, 23),
                    }
                }
            ),
        };

        public static IEnumerable<TestCaseData> SearchProducts_IfApiResponseFailed_ThrowsApiException = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""query"": ""rw""
        },
        ""urlParams"": {}
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Sorry, nothing was found""
            }
        ]
    }
}",
                HttpStatusCode.NotFound,
                "Sorry, nothing was found"
            ),
        };

        #endregion

        #region GetAvailabilityRange

        public static IEnumerable<TestCaseData> GetAvailabilityRange_IfApiResponseSuccessful_ReturnsBookingRange = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""productId"": ""1587""
        }
    },
    ""response"": {
        ""firstBookableDate"": ""2020-05-05T00:00:00+00:00"",
        ""lastBookableDate"": ""2020-05-23T00:00:00+00:00""
    },
    ""context"": null
}",
                new AvailabilityRange
                {
                    FirstBookableDate = new DateTime(2020, 05, 05),
                    LastBookableDate = new DateTime(2020, 05, 23),
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetAvailabilityRange_IfApiResponseFailed_ThrowsApiException = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""productId"": ""158""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Sorry, nothing was found""
            }
        ]
    }
}",
                HttpStatusCode.NotFound,
                "Sorry, nothing was found"
            ),
        };

        #endregion

        #region GetAvailabilities

        public static IEnumerable<TestCaseData> GetAvailabilities_IfApiResponseSuccessful_ReturnsPerformances = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""productId"": ""6441"",
            ""quantity"": ""1"",
            ""fromDate"": ""20201003"",
            ""toDate"": ""20201005""
        }
    },
    ""response"": [
        {
            ""datetime"": ""2020-10-03T19:30:00+0000"",
            ""largestLumpOfTickets"": 33
        },
        {
            ""datetime"": ""2020-10-04T16:00:00+0000"",
            ""largestLumpOfTickets"": 33
        },
        {
            ""datetime"": ""2020-10-05T19:30:00+0000"",
            ""largestLumpOfTickets"": 33
        }
    ],
    ""context"": null
}",
                new List<Availability>
                {
                    new Availability
                    {
                        DateTime = new DateTime(2020, 10, 03, 19, 30, 00),
                        LargestLumpOfTickets = 33
                    },
                    new Availability
                    {
                        DateTime = new DateTime(2020, 10, 04, 16, 00, 00),
                        LargestLumpOfTickets = 33
                    },
                    new Availability
                    {
                        DateTime = new DateTime(2020, 10, 05, 19, 30, 00),
                        LargestLumpOfTickets = 33
                    },
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetAvailabilities_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""productId"": ""1587"",
            ""quantity"": ""2"",
            ""fromDate"": ""20200505"",
            ""toDate"": ""20210523""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""fromDate"",
                ""message"": ""end date should not be more than 90 days from start dates""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "fromDate: end date should not be more than 90 days from start dates"
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""productId"": ""1587"",
            ""quantity"": ""2"",
            ""fromDate"": ""20200305"",
            ""toDate"": ""20200523""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""fromDate"",
                ""message"": ""start date should not be in the past""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "fromDate: start date should not be in the past"
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""productId"": ""158)"",
            ""quantity"": ""2"",
            ""fromDate"": ""20200505"",
            ""toDate"": ""20200523""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""productId"",
                ""message"": ""The product ID can only contain numbers, letters and dashes""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "productId: The product ID can only contain numbers, letters and dashes"
            ),

            // 403
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""productId"": ""1001"",
            ""quantity"": ""1"",
            ""fromDate"": ""20201003"",
            ""toDate"": ""20201005""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Invalid request: Request to EApi failed because the specified affiliate does not have access to this product: Not allowed to use this show""
            }
        ]
    }
}",
                HttpStatusCode.Forbidden,
                "Invalid request: Request to EApi failed because the specified affiliate does not have access to this product: Not allowed to use this show"
            ),

            // 404
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""productId"": ""158"",
            ""quantity"": ""2"",
            ""fromDate"": ""20200505"",
            ""toDate"": ""20200523""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Product not found""
            }
        ]
    }
}",
                HttpStatusCode.NotFound,
                "Product not found"
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""productId"": ""1587"",
            ""quantity"": ""2"",
            ""fromDate"": ""20200505"",
            ""toDate"": ""20200523""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Sorry, nothing was found""
            }
        ]
    }
}",
                HttpStatusCode.NotFound,
                "Sorry, nothing was found"
            ),
        };

        #endregion

        #region GetAggregateSeatAvailability

        public static IEnumerable<TestCaseData> GetAggregateSeatAvailability_IfApiResponseSuccessful_ReturnsAvailability = new[]
        {
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": """",
    ""query"": {
      ""affiliateId"": ""resiaapi"",
      ""date"": ""20201023"",
      ""quantity"": ""2"",
      ""time"": ""1930""
    },
    ""urlParams"": {
      ""productId"": ""1587""
    }
  },
  ""response"": {
    ""displayCurrency"": ""GBP"",
    ""areas"": [
      {
        ""availableCount"": 1,
        ""date"": ""2020-10-23T19:30:00+0000"",
        ""name"": ""Circle"",
        ""mode"": ""allocated"",
        ""groupings"": [
          {
            ""groupIdentifier"": ""CIRCLE~X44;50"",
            ""aggregateReference"": null,
            ""row"": ""X"",
            ""seatNumberStart"": 44,
            ""seatNumberEnd"": 45,
            ""availableCount"": 2,
            ""pricing"": {
              ""salePrice"": [
                {
                  ""value"": 3200,
                  ""currency"": ""GBP"",
                  ""decimalPlaces"": 2
                }
              ],
              ""faceValue"": [
                {
                  ""value"": 2500,
                  ""currency"": ""GBP"",
                  ""decimalPlaces"": 2
                }
              ],
              ""percentageDiscount"": 0,
              ""includesBookingFee"": true,
              ""createdAt"": ""2020-05-13T11:09:02+0000""
            },
            ""seats"": [
              {
                ""seatIdentifier"": ""CIRCLE-X44"",
                ""aggregateReference"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDQ7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDQiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.L-E7HTETVnPRzkr6ghsFVTL4X62rSycnF-S_PtIH8KM"",
                ""row"": ""X"",
                ""number"": 44,
                ""pricing"": {
                  ""salePrice"": [
                    {
                      ""value"": 3200,
                      ""currency"": ""GBP"",
                      ""decimalPlaces"": 2
                    }
                  ],
                  ""faceValue"": [
                    {
                      ""value"": 2500,
                      ""currency"": ""GBP"",
                      ""decimalPlaces"": 2
                    }
                  ],
                  ""percentageDiscount"": 0,
                  ""includesBookingFee"": true,
                  ""createdAt"": ""2020-05-13T11:09:02+0000""
                }
              },
              {
                ""seatIdentifier"": ""CIRCLE-X45"",
                ""aggregateReference"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDQ7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.AyrGkcbn5WhSfjA-himFaF9ivbhA1CFBFI5hSd-OKGw"",
                ""row"": ""X"",
                ""number"": 45,
                ""pricing"": {
                  ""salePrice"": [
                    {
                      ""value"": 3200,
                      ""currency"": ""GBP"",
                      ""decimalPlaces"": 2
                    }
                  ],
                  ""faceValue"": [
                    {
                      ""value"": 2500,
                      ""currency"": ""GBP"",
                      ""decimalPlaces"": 2
                    }
                  ],
                  ""percentageDiscount"": 0,
                  ""includesBookingFee"": true,
                  ""createdAt"": ""2020-05-13T11:09:02+0000""
                }
              }
            ],
            ""seatLumps"": [
              {
                ""seats"": [
                  ""CIRCLE-X44"",
                  ""CIRCLE-X45""
                ]
              }
            ]
          }
        ]
      }
    ],
    ""availableCount"": 1
  },
  ""context"": null
}",
                new AggregateSeatAvailability
                {
                    DisplayCurrency = "GBP",
                    Areas = new List<AggregateArea>
                    {
                        new AggregateArea
                        {
                            AvailableCount = 1,
                            Date = new DateTime(2020, 10, 23, 19, 30, 00),
                            Name = "Circle",
                            Mode = "allocated",
                            Groupings = new List<AggregateGrouping>
                            {
                                new AggregateGrouping
                                {
                                    GroupIdentifier = "CIRCLE~X44;50",
                                    AggregateReference = null,
                                    Row = "X",
                                    SeatNumberStart = 44,
                                    SeatNumberEnd = 45,
                                    AvailableCount = 2,
                                    Pricing = new AggregatePricing
                                    {
                                        SalePrice =new List<Price>
                                        {
                                            new Price
                                            {
                                                Value = 3200,
                                                Currency = "GBP",
                                                DecimalPlaces = 2
                                            }
                                        },
                                        FaceValue = new List<Price>
                                        {
                                            new Price
                                            {
                                                Value = 2500,
                                                Currency = "GBP",
                                                DecimalPlaces = 2
                                            }
                                        },
                                        PercentageDiscount = 0,
                                        IncludesBookingFee = true,
                                        CreatedAt = new DateTime(2020, 05, 13, 11, 09, 02)
                                    },
                                    Seats = new List<AggregateSeat>
                                    {
                                        new AggregateSeat
                                        {
                                            SeatIdentifier = "CIRCLE-X44",
                                            AggregateReference = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDQ7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDQiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.L-E7HTETVnPRzkr6ghsFVTL4X62rSycnF-S_PtIH8KM",
                                            Row = "X",
                                            Number = 44,
                                            Pricing = new AggregatePricing
                                            {
                                                SalePrice =new List<Price>
                                                {
                                                    new Price
                                                    {
                                                        Value = 3200,
                                                        Currency = "GBP",
                                                        DecimalPlaces = 2
                                                    }
                                                },
                                                FaceValue = new List<Price>
                                                {
                                                    new Price
                                                    {
                                                        Value = 2500,
                                                        Currency = "GBP",
                                                        DecimalPlaces = 2
                                                    }
                                                },
                                                PercentageDiscount = 0,
                                                IncludesBookingFee = true,
                                                CreatedAt = new DateTime(2020, 05, 13, 11, 09, 02)
                                            },
                                        },
                                        new AggregateSeat
                                        {
                                            SeatIdentifier = "CIRCLE-X45",
                                            AggregateReference = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDQ7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.AyrGkcbn5WhSfjA-himFaF9ivbhA1CFBFI5hSd-OKGw",
                                            Row = "X",
                                            Number = 45,
                                            Pricing = new AggregatePricing
                                            {
                                                SalePrice =new List<Price>
                                                {
                                                    new Price
                                                    {
                                                        Value = 3200,
                                                        Currency = "GBP",
                                                        DecimalPlaces = 2
                                                    }
                                                },
                                                FaceValue = new List<Price>
                                                {
                                                    new Price
                                                    {
                                                        Value = 2500,
                                                        Currency = "GBP",
                                                        DecimalPlaces = 2
                                                    }
                                                },
                                                PercentageDiscount = 0,
                                                IncludesBookingFee = true,
                                                CreatedAt = new DateTime(2020, 05, 13, 11, 09, 02)
                                            },
                                        },
                                    },
                                    SeatLumps = new List<SeatLump>
                                    {
                                        new SeatLump
                                        {
                                            Seats = new List<string>
                                            {
                                                "CIRCLE-X44",
                                                "CIRCLE-X45"
                                            }
                                        },
                                    }
                                }
                            },
                        }
                    },
                    AvailableCount = 1,
                }
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""resiaapi"",
            ""direction"": ""desc"",
            ""quantity"": ""2"",
            ""time"": ""1930""
        },
        ""urlParams"": {
            ""productId"": ""1587""
        }
    },
    ""response"": {
        ""displayCurrency"": null,
        ""areas"": [],
        ""availableCount"": 0
    },
    ""context"": null
}",
                new AggregateSeatAvailability
                {
                    DisplayCurrency = null,
                    Areas = new List<AggregateArea>(),
                    AvailableCount = 0,
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetAggregateSeatAvailability_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""resiaapi"",
            ""date"": ""20201023"",
            ""direction"": ""Desc"",
            ""quantity"": ""2"",
            ""time"": ""1930""
        },
        ""urlParams"": {
            ""productId"": ""1587""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""direction"",
                ""message"": ""The value you selected is not a valid choice.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "direction: The value you selected is not a valid choice."
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""resiaapi"",
            ""direction"": ""desc"",
            ""quantity"": ""2""
        },
        ""urlParams"": {
            ""productId"": ""158""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""time"",
                ""message"": ""This value should not be null.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "time: This value should not be null."
            ),
            
            // 404
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""resiaapi"",
            ""date"": ""20201023"",
            ""quantity"": ""2"",
            ""time"": ""1930""
        },
        ""urlParams"": {
            ""productId"": ""158""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Not found""
            }
        ]
    }
}",
                HttpStatusCode.NotFound,
                "Not found"
            ),

            // 503
            new TestCaseData(
                @"<!DOCTYPE html><html>The request has failed. Fastly</html>",
                HttpStatusCode.ServiceUnavailable,
                "Cannot convert API error correctly.\r\n\r\n<!DOCTYPE html><html>The request has failed. Fastly</html>"
            ),
        };

        #endregion

        #region GetSeatAvailability

        public static IEnumerable<TestCaseData> GetSeatAvailability_IfApiResponseSuccessful_ReturnsAvailability = new[]
        {
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": """",
    ""query"": {
      ""affiliateId"": ""boxoffice"",
      ""date"": ""20201011"",
      ""time"": ""1610""
    },
    ""urlParams"": {
      ""productId"": ""7312"",
      ""quantity"": ""1""
    }
  },
  ""response"": {
    ""availableCount"": 200,
    ""isAvailable"": true,
    ""areas"": [
      {
        ""aggregateReference"": ""OA=="",
        ""itemReference"": ""OA=="",
        ""isAvailable"": true,
        ""availableCount"": 50,
        ""date"": ""2020-10-11T16:10:00+0000"",
        ""name"": ""Child 3-15"",
        ""mode"": ""allocated"",
        ""groupings"": [
          {
            ""groupIdentifier"": ""Child3-15~16101;50"",
            ""aggregateReference"": ""MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzI="",
            ""itemReference"": ""MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzI="",
            ""row"": ""1610"",
            ""seatNumberStart"": 1,
            ""seatNumberEnd"": 50,
            ""availableCount"": 50,
            ""isAvailable"": true,
            ""attributes"": {
              ""restrictedView"": false,
              ""sideView"": false
            },
            ""pricing"": {
              ""priceReference"": ""MTYwMDpHQlB+MTYwMDpHQlB+MjAyMC0wNS0wNVQxODo0NDoyNiswMDAw"",
              ""salePrice"": {
                ""value"": 1600,
                ""currency"": ""GBP"",
                ""decimalPlaces"": 2
              },
              ""faceValue"": {
                ""value"": 1600,
                ""currency"": ""GBP"",
                ""decimalPlaces"": 2
              },
              ""percentage"": 0,
              ""offer"": false,
              ""noBookingFee"": true,
              ""timestamp"": ""2020-05-05T18:44:26+0000""
            },
            ""seats"": [
              {
                ""aggregateReference"": ""MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDE="",
                ""itemReference"": ""MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDE="",
                ""row"": ""1610"",
                ""number"": 1,
                ""isAvailable"": true,
                ""attributes"": {
                  ""restrictedView"": false,
                  ""sideView"": false
                },
                ""seatIdentifier"": ""CHILD_3-15-16101"",
                ""pricing"": {
                  ""priceReference"": ""MTYwMDpHQlB+MTYwMDpHQlB+MjAyMC0wNS0wNVQxODo0NDoyNiswMDAw"",
                  ""salePrice"": {
                    ""value"": 1600,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                  },
                  ""faceValue"": {
                    ""value"": 1600,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                  },
                  ""percentage"": 0,
                  ""offer"": false,
                  ""noBookingFee"": true,
                  ""timestamp"": ""2020-05-05T18:44:26+0000""
                }
              },
              {
                ""aggregateReference"": ""MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDI="",
                ""itemReference"": ""MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDI="",
                ""row"": ""1610"",
                ""number"": 2,
                ""isAvailable"": true,
                ""attributes"": {
                  ""restrictedView"": false,
                  ""sideView"": false
                },
                ""seatIdentifier"": ""CHILD_3-15-16102"",
                ""pricing"": {
                  ""priceReference"": ""MTYwMDpHQlB+MTYwMDpHQlB+MjAyMC0wNS0wNVQxODo0NDoyNiswMDAw"",
                  ""salePrice"": {
                    ""value"": 1600,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                  },
                  ""faceValue"": {
                    ""value"": 1600,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                  },
                  ""percentage"": 0,
                  ""offer"": false,
                  ""noBookingFee"": true,
                  ""timestamp"": ""2020-05-05T18:44:26+0000""
                }
              }
            ],
            ""seatLumps"": [
              {
                ""seats"": [
                  ""CHILD_3-15-16101""
                ]
              },
              {
                ""seats"": [
                  ""CHILD_3-15-16103""
                ]
              }
            ]
          }
        ]
      }
    ]
  },
  ""context"": null
}",
                new SeatAvailability
                {
                    AvailableCount = 200,
                    IsAvailable = true,
                    Areas = new List<Area>
                    {
                        new Area
                        {
                            AggregateReference = "OA==",
                            ItemReference = "OA==",
                            IsAvailable = true,
                            AvailableCount = 50,
                            Date = new DateTime(2020, 10, 11, 16, 10, 00),
                            Name = "Child 3-15",
                            Mode = "allocated",
                            Groupings = new List<Grouping>
                            {
                                new Grouping
                                {
                                    GroupIdentifier = "Child3-15~16101;50",
                                    AggregateReference = "MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzI=",
                                    ItemReference = "MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzI=",
                                    Row = "1610",
                                    SeatNumberStart = 1,
                                    SeatNumberEnd = 50,
                                    AvailableCount = 50,
                                    IsAvailable = true,
                                    Attributes = new Attributes
                                    {
                                        RestrictedView = false,
                                        SideView = false
                                    },
                                    Pricing = new SDK.Inventory.Models.Pricing
                                    {
                                        PriceReference = "MTYwMDpHQlB+MTYwMDpHQlB+MjAyMC0wNS0wNVQxODo0NDoyNiswMDAw",
                                        SalePrice = new Price
                                        {
                                            Value = 1600,
                                            Currency = "GBP",
                                            DecimalPlaces = 2
                                        },
                                        FaceValue = new Price
                                        {
                                            Value = 1600,
                                            Currency = "GBP",
                                            DecimalPlaces = 2
                                        },
                                        Percentage = 0,
                                        Offer = false,
                                        NoBookingFee = true,
                                        Timestamp = new DateTime(2020, 05, 05, 18, 44, 26)
                                    },
                                    Seats = new List<Seat>
                                    {
                                        new Seat
                                        {
                                            AggregateReference = "MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDE=",
                                            ItemReference = "MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDE=",
                                            Row = "1610",
                                            Number = 1,
                                            IsAvailable = true,
                                            Attributes = new Attributes
                                            {
                                                RestrictedView = false,
                                                SideView = false
                                            },
                                            SeatIdentifier = "CHILD_3-15-16101",
                                            Pricing = new SDK.Inventory.Models.Pricing
                                            {
                                                PriceReference = "MTYwMDpHQlB+MTYwMDpHQlB+MjAyMC0wNS0wNVQxODo0NDoyNiswMDAw",
                                                SalePrice = new Price
                                                {
                                                    Value = 1600,
                                                    Currency = "GBP",
                                                    DecimalPlaces = 2
                                                },
                                                FaceValue = new Price
                                                {
                                                    Value = 1600,
                                                    Currency = "GBP",
                                                    DecimalPlaces = 2
                                                },
                                                Percentage = 0,
                                                Offer = false,
                                                NoBookingFee = true,
                                                Timestamp = new DateTime(2020, 05, 05, 18, 44, 26)
                                            },
                                        },
                                        new Seat
                                        {
                                            AggregateReference = "MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDI=",
                                            ItemReference = "MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDI=",
                                            Row = "1610",
                                            Number = 2,
                                            IsAvailable = true,
                                            Attributes = new Attributes
                                            {
                                                RestrictedView = false,
                                                SideView = false
                                            },
                                            SeatIdentifier = "CHILD_3-15-16102",
                                            Pricing = new SDK.Inventory.Models.Pricing
                                            {
                                                PriceReference = "MTYwMDpHQlB+MTYwMDpHQlB+MjAyMC0wNS0wNVQxODo0NDoyNiswMDAw",
                                                SalePrice = new Price
                                                {
                                                    Value = 1600,
                                                    Currency = "GBP",
                                                    DecimalPlaces = 2
                                                },
                                                FaceValue = new Price
                                                {
                                                    Value = 1600,
                                                    Currency = "GBP",
                                                    DecimalPlaces = 2
                                                },
                                                Percentage = 0,
                                                Offer = false,
                                                NoBookingFee = true,
                                                Timestamp = new DateTime(2020, 05, 05, 18, 44, 26)
                                            },
                                        }
                                    },
                                    SeatLumps = new List<SeatLump>
                                    {
                                        new SeatLump
                                        {
                                            Seats = new List<string>
                                            {
                                                "CHILD_3-15-16101"
                                            }
                                        },
                                        new SeatLump
                                        {
                                            Seats = new List<string>
                                            {
                                                "CHILD_3-15-16103"
                                            }
                                        },
                                    }
                                }
                            },
                        }
                    },
                }
            ),
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": """",
    ""query"": {
      ""affiliateId"": ""boxoffice"",
      ""date"": ""20200711"",
      ""direction"": ""desc"",
      ""time"": ""1400""
    },
    ""urlParams"": {
      ""productId"": ""7021"",
      ""quantity"": ""1""
    }
  },
  ""response"": {
    ""availableCount"": null,
    ""isAvailable"": true,
    ""areas"": [
      {
        ""aggregateReference"": ""Mw=="",
        ""itemReference"": ""Mw=="",
        ""isAvailable"": true,
        ""availableCount"": null,
        ""date"": ""2020-07-11T14:00:00-0400"",
        ""name"": ""(Part 2 same day 7:30PM) Premium"",
        ""mode"": ""freesell"",
        ""groupings"": [
          {
            ""groupIdentifier"": ""(Part2sameday7:30PM)Premium"",
            ""aggregateReference"": ""Qkl+NzA0fjcwMjF+MTMxODc5OH4yMDIwLTA3LTExfjE0OjAwfjF+KFBhcnQgMiBzYW1lIGRheSA3OjMwUE0pIFByZW1pdW1+Mzk5MDB+Mzk5MDB+Mzk5MDB+NDkxMDB+MC44MTEzODl+MjAyMC0wNS0wN1QwOTowMTowNiswMDAw"",
            ""itemReference"": ""Qkl+NzA0fjcwMjF+MTMxODc5OH4yMDIwLTA3LTExfjE0OjAwfjF+KFBhcnQgMiBzYW1lIGRheSA3OjMwUE0pIFByZW1pdW0="",
            ""row"": null,
            ""seatNumberStart"": null,
            ""seatNumberEnd"": null,
            ""availableCount"": null,
            ""isAvailable"": true,
            ""attributes"": {
              ""restrictedView"": false,
              ""sideView"": false
            },
            ""pricing"": {
              ""priceReference"": ""Mzk5MDA6VVNEfjM5OTAwOlVTRH4zOTkwMDpHQlB+NDkxMDA6VVNEfjAuODExMzg5fjIwMjAtMDUtMDdUMDk6MDE6MDYrMDAwMA=="",
              ""salePrice"": {
                ""value"": 39900,
                ""currency"": ""GBP"",
                ""decimalPlaces"": 2
              },
              ""faceValue"": {
                ""value"": 39900,
                ""currency"": ""USD"",
                ""decimalPlaces"": 2
              },
              ""percentage"": 0,
              ""offer"": false,
              ""noBookingFee"": false,
              ""timestamp"": ""2020-05-07T09:01:06+0000""
            },
            ""seats"": [],
            ""seatLumps"": [],
            ""aggregateReferenceObject"": {
              ""itemReference"": {
                ""supplierPrefix"": ""BI"",
                ""venueId"": ""704"",
                ""nativeProductId"": ""7021"",
                ""internalItemId"": ""1318798"",
                ""date"": ""2020-07-11"",
                ""time"": ""14:00"",
                ""quantity"": 1,
                ""seatLocationDescription"": ""(Part 2 same day 7:30PM) Premium""
              },
              ""priceReference"": {
                ""faceValue"": {
                  ""value"": 39900,
                  ""currency"": ""USD"",
                  ""decimalPlaces"": 2
                },
                ""costPrice"": {
                  ""value"": 39900,
                  ""currency"": ""USD"",
                  ""decimalPlaces"": 2
                },
                ""salePrice"": {
                  ""value"": 39900,
                  ""currency"": ""GBP"",
                  ""decimalPlaces"": 2
                },
                ""originalSalePrice"": {
                  ""value"": 49100,
                  ""currency"": ""USD"",
                  ""decimalPlaces"": 2
                },
                ""fxRate"": {
                  ""rate"": 0.811389
                },
                ""timestamp"": ""2020-05-07T09:01:06+0000""
              }
            }
          }
        ]
      }
    ]
  },
  ""context"": null
}",
                new SeatAvailability
                {
                    AvailableCount = null,
                    IsAvailable = true,
                    Areas = new List<Area>
                    {
                        new Area
                        {
                            AggregateReference = "Mw==",
                            ItemReference = "Mw==",
                            IsAvailable = true,
                            AvailableCount = null,
                            Date = new DateTime(2020, 07, 11, 18, 00, 00),
                            Name = "(Part 2 same day 7:30PM) Premium",
                            Mode = "freesell",
                            Groupings = new List<Grouping>
                            {
                                new Grouping
                                {
                                    GroupIdentifier = "(Part2sameday7:30PM)Premium",
                                    AggregateReference = "Qkl+NzA0fjcwMjF+MTMxODc5OH4yMDIwLTA3LTExfjE0OjAwfjF+KFBhcnQgMiBzYW1lIGRheSA3OjMwUE0pIFByZW1pdW1+Mzk5MDB+Mzk5MDB+Mzk5MDB+NDkxMDB+MC44MTEzODl+MjAyMC0wNS0wN1QwOTowMTowNiswMDAw",
                                    ItemReference = "Qkl+NzA0fjcwMjF+MTMxODc5OH4yMDIwLTA3LTExfjE0OjAwfjF+KFBhcnQgMiBzYW1lIGRheSA3OjMwUE0pIFByZW1pdW0=",
                                    Row = null,
                                    SeatNumberStart = null,
                                    SeatNumberEnd = null,
                                    AvailableCount = null,
                                    IsAvailable = true,
                                    Attributes = new Attributes
                                    {
                                        RestrictedView = false,
                                        SideView = false
                                    },
                                    Pricing = new SDK.Inventory.Models.Pricing
                                    {
                                        PriceReference = "Mzk5MDA6VVNEfjM5OTAwOlVTRH4zOTkwMDpHQlB+NDkxMDA6VVNEfjAuODExMzg5fjIwMjAtMDUtMDdUMDk6MDE6MDYrMDAwMA==",
                                        SalePrice = new Price
                                        {
                                            Value = 39900,
                                            Currency = "GBP",
                                            DecimalPlaces = 2
                                        },
                                        FaceValue = new Price
                                        {
                                            Value = 39900,
                                            Currency = "USD",
                                            DecimalPlaces = 2
                                        },
                                        Percentage = 0,
                                        Offer = false,
                                        NoBookingFee = false,
                                        Timestamp = new DateTime(2020, 05, 07, 09, 01, 06)
                                    },
                                    Seats = new List<Seat>(),
                                    SeatLumps = new List<SeatLump>(),
                                    AggregateReferenceObject = new AggregateReference
                                    {
                                        ItemReference = new ItemReference
                                        {
                                            SupplierPrefix = "BI",
                                            VenueId = "704",
                                            NativeProductId = "7021",
                                            InternalItemId = "1318798",
                                            Date = new DateTime(2020, 07, 11),
                                            Time = new TimeSpan(14, 00, 00),
                                            Quantity = 1,
                                            SeatLocationDescription = "(Part 2 same day 7:30PM) Premium"
                                        },
                                        PriceReference = new PriceReference
                                        {
                                            FaceValue = new Price
                                            {
                                                Value = 39900,
                                                Currency = "USD",
                                                DecimalPlaces = 2
                                            },
                                            CostPrice = new Price
                                            {
                                                Value = 39900,
                                                Currency = "USD",
                                                DecimalPlaces = 2
                                            },
                                            SalePrice = new Price
                                            {
                                                Value = 39900,
                                                Currency = "GBP",
                                                DecimalPlaces = 2
                                            },
                                            OriginalSalePrice = new Price
                                            {
                                                Value = 49100,
                                                Currency = "USD",
                                                DecimalPlaces = 2
                                            },
                                            FxRate = new FxRate
                                            {
                                                Rate = 0.811389M
                                            },
                                            Timestamp = new DateTime(2020, 05, 07, 09, 01, 06)
                                        }
                                    }
                                }
                            },
                        }
                    },
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetSeatAvailability_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""boxoffice"",
            ""date"": ""20201011"",
            ""time"": ""1610""
        },
        ""urlParams"": {
            ""productId"": ""731_2"",
            ""quantity"": ""1""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""productId"",
                ""message"": ""The product ID can only contain numbers, letters and dashes""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "productId: The product ID can only contain numbers, letters and dashes"
            ),
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""boxoffice"",
            ""date"": ""20201011"",
            ""time"": ""1610""
        },
        ""urlParams"": {
            ""productId"": ""7312"",
            ""quantity"": ""0""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""quantity"",
                ""message"": ""quantity should be at least 1""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "quantity: quantity should be at least 1"
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""boxoffice"",
            ""date"": ""2020-10-11"",
            ""time"": ""1610""
        },
        ""urlParams"": {
            ""productId"": ""7312"",
            ""quantity"": ""1""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""dateString"",
                ""message"": ""invalid date, should be of 'Ymd' format""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "dateString: invalid date, should be of 'Ymd' format"
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""boxoffice"",
            ""date"": ""20201011"",
            ""time"": ""1610)""
        },
        ""urlParams"": {
            ""productId"": ""7312"",
            ""quantity"": ""1""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""timeString"",
                ""message"": ""invalid time, should be of 'Hi' format""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "timeString: invalid time, should be of 'Hi' format"
            ),

            // 401
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""boxoffic"",
            ""date"": ""20201011"",
            ""time"": ""1610""
        },
        ""urlParams"": {
            ""productId"": ""7312"",
            ""quantity"": ""1""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""an invalid affiliateId has been specified, or the specified affiliate does not have access to this request""
            }
        ]
    }
}",
                HttpStatusCode.Unauthorized,
                "an invalid affiliateId has been specified, or the specified affiliate does not have access to this request"
            ),
            
            // 404
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""affiliateId"": ""boxoffice"",
            ""time"": ""1610""
        },
        ""urlParams"": {
            ""productId"": ""7312"",
            ""quantity"": ""1""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Sorry, nothing was found""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Sorry, nothing was found"
            ),
        };

        #endregion
    }
}
