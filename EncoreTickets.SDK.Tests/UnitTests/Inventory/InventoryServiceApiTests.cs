using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Inventory;
using EncoreTickets.SDK.Inventory.Models;
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

        private MockersForApiService mockers;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public InventoryServiceApiTests() : base(new ApiContext(Environments.Sandbox))
        {
        }

        [SetUp]
        public void CreateMockers()
        {
            mockers = new MockersForApiService();
        }

        [Test]
        public void Constructor_InitializesServiceCorrectly()
        {
            Assert.AreEqual("https://inventory-service.devtixuk.io/api/v4/", BaseUrl);
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
            mockers.SetupAnyExecution<ProductSearchResponse>();

            try
            {
                SearchProducts(text);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ProductSearchResponse>(BaseUrl, "search", Method.GET,
                expectedQueryParameters: new Dictionary<string, object> { { "query", text } });
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
            mockers.SetupAnyExecution<ApiResponse<AvailabilityRange>>();

            try
            {
                GetAvailabilityRange(productId);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<AvailabilityRange>>(BaseUrl, $"products/{productId}/availability-range", Method.GET);
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

            mockers.VerifyExecution<ApiResponse<List<Availability>>>(BaseUrl,
                $"availability/products/{productId}/quantity/{quantity}/from/{from:yyyyMMdd}/to/{to:yyyyMMdd}",
                Method.GET);
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

        #region GetAvailability

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetAvailability_IfTextIsNotSet_ThrowsArgumentException(string productId)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetAvailability(productId, It.IsAny<int>());
            });
        }

        [TestCase(1587, 2)]
        public void GetAvailability_IfProductIdAndQuantityAreSet_CallsApiWithRightParameters(int productId, int quantity)
        {
            mockers.SetupAnyExecution<Availability>();

            try
            {
                GetAvailability(productId, quantity);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<Availability>(BaseUrl, $"v1/availability/products/{productId}/quantity/{quantity}/seats", Method.GET);
        }

        [TestCase(1587, 2, "1/10/2020 3:56:51 PM")]
        public void GetAvailability_IfProductIdAndQuantityAndPerformanceAreSet_CallsApiWithRightParameters(int productId, int quantity, string dateAsStr)
        {
            var performance = TestHelper.ConvertTestArgumentToDateTime(dateAsStr);
            mockers.SetupAnyExecution<Availability>();

            try
            {
                GetAvailability(productId, quantity, performance);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<Availability>(BaseUrl,
                $"v1/availability/products/{productId}/quantity/{quantity}/seats", Method.GET,
                expectedQueryParameters: new Dictionary<string, object>
                {
                    {"date", performance.ToString("yyyyMMdd")},
                    {"time", performance.ToString("HHmm")},
                });
        }

        [TestCase("1587", 2, "1/10/2020 0:0:0 PM", "1/10/2020 3:56:51 PM")]
        [TestCase("1587", 2, "1/10/2020 0:0:0 PM", "")]
        [TestCase("1587", 2, "", "1/10/2020 3:56:51 PM")]
        [TestCase("1587", 2, "1/10/2020 0:0:0 PM", null)]
        [TestCase("1587", 2, null, "1/10/2020 3:56:51 PM")]
        [TestCase("1587", 2, null, null)]
        public void GetAvailability_IfProductIdAndQuantityAndPerformanceAreSet_CallsApiWithRightParameters(
            string productId, int quantity, string dateAsStr, string timeAsStr)
        {
            var date = string.IsNullOrWhiteSpace(dateAsStr) ? null : (DateTime?)TestHelper.ConvertTestArgumentToDateTime(dateAsStr);
            var time = string.IsNullOrWhiteSpace(timeAsStr) ? null : (DateTime?)TestHelper.ConvertTestArgumentToDateTime(timeAsStr);
            var queryParameters = new Dictionary<string, object>();
            if (date != null)
            {
                queryParameters.Add("date", date.Value.ToString("yyyyMMdd"));
            }
            if (time != null)
            {
                queryParameters.Add("time", time.Value.ToString("HHmm"));
            }
            mockers.SetupAnyExecution<Availability>();

            try
            {
                GetAvailability(productId, quantity, date, time);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<Availability>(BaseUrl,
                $"v1/availability/products/{productId}/quantity/{quantity}/seats", Method.GET,
                expectedQueryParameters: queryParameters);
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetAvailability_IfApiResponseSuccessful_ReturnsAvailability))]
        public void GetAvailability_IfApiResponseSuccessful_ReturnsAvailability(
            string responseContent,
            Availability expected)
        {
            mockers.SetupSuccessfulExecution<Availability>(responseContent);

            var actual = GetAvailability(TestValidProductId, It.IsAny<int>());

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetAvailability_IfApiResponseFailed_ThrowsApiException))]
        public void GetAvailability_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string message)
        {
            mockers.SetupFailedExecution<Availability>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetAvailability(TestValidProductId, It.IsAny<int>());
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

        public static IEnumerable<TestCaseData> GetAvailability_IfApiResponseSuccessful_ReturnsAvailability = new[]
        {
            new TestCaseData(
                "{\"availableCount\":168,\"isAvailable\":true,\"areas\":[{\"aggregateReference\":\"Mg==\",\"itemReference\":\"Mg==\",\"isAvailable\":true,\"availableCount\":79,\"date\":\"2020-01-13T19:30:00+0000\",\"name\":\"Circle\",\"mode\":\"allocated\",\"groupings\":[{\"groupIdentifier\":\"Circle~P8;9\",\"aggregateReference\":\"ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzI=\",\"itemReference\":\"ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzI=\",\"row\":\"P\",\"seatNumberStart\":8,\"seatNumberEnd\":9,\"availableCount\":2,\"isAvailable\":true,\"attributes\":{\"restrictedView\":false,\"sideView\":false},\"pricing\":{\"priceReference\":\"MjcwMDpHQlB+MjUwMDpHQlB+MjAyMC0wMS0xM1QwODoxNjozNSswMDAw\",\"salePrice\":{\"value\":2700,\"currency\":\"GBP\",\"decimalPlaces\":2},\"faceValue\":{\"value\":2500,\"currency\":\"GBP\",\"decimalPlaces\":2},\"percentage\":0,\"offer\":false,\"noBookingFee\":false,\"timestamp\":\"2020-01-13T08:16:35+0000\"},\"seats\":[{\"aggregateReference\":\"ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzItUDg=\",\"itemReference\":\"ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzItUDg=\",\"row\":\"P\",\"number\":8,\"isAvailable\":true,\"attributes\":{\"restrictedView\":false,\"sideView\":false},\"seatIdentifier\":\"CIRCLE-P8\",\"pricing\":{\"priceReference\":\"MjcwMDpHQlB+MjUwMDpHQlB+MjAyMC0wMS0xM1QwODoxNjozNSswMDAw\",\"salePrice\":{\"value\":2700,\"currency\":\"GBP\",\"decimalPlaces\":2},\"faceValue\":{\"value\":2500,\"currency\":\"GBP\",\"decimalPlaces\":2},\"percentage\":0,\"offer\":false,\"noBookingFee\":false,\"timestamp\":\"2020-01-13T08:16:35+0000\"}},{\"aggregateReference\":\"ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzItUDk=\",\"itemReference\":\"ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzItUDk=\",\"row\":\"P\",\"number\":9,\"isAvailable\":true,\"attributes\":{\"restrictedView\":false,\"sideView\":false},\"seatIdentifier\":\"CIRCLE-P9\",\"pricing\":{\"priceReference\":\"MjcwMDpHQlB+MjUwMDpHQlB+MjAyMC0wMS0xM1QwODoxNjozNSswMDAw\",\"salePrice\":{\"value\":2700,\"currency\":\"GBP\",\"decimalPlaces\":2},\"faceValue\":{\"value\":2500,\"currency\":\"GBP\",\"decimalPlaces\":2},\"percentage\":0,\"offer\":false,\"noBookingFee\":false,\"timestamp\":\"2020-01-13T08:16:35+0000\"}}],\"seatLumps\":[{\"seats\":[\"CIRCLE-P8\",\"CIRCLE-P9\"]}]}]}]}",
                new Availability
                {
                    AvailableCount = 168,
                    IsAvailable = true,
                    Areas = new List<Area>
                    {
                        new Area
                        {
                            AggregateReference = "Mg==",
                            ItemReference = "Mg==",
                            IsAvailable = true,
                            AvailableCount = 79,
                            Date = new DateTime(2020, 01, 13, 19, 30, 00),
                            Name = "Circle",
                            Mode = "allocated",
                            Groupings = new List<Grouping>
                            {
                                new Grouping
                                {
                                    GroupIdentifier = "Circle~P8;9",
                                    AggregateReference = "ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzI=",
                                    ItemReference = "ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzI=",
                                    Row = "P",
                                    SeatNumberStart = 8,
                                    SeatNumberEnd = 9,
                                    AvailableCount = 2,
                                    IsAvailable = true,
                                    Attributes = new Attributes
                                    {
                                        RestrictedView = false,
                                        SideView = false
                                    },
                                    Pricing = new SDK.Inventory.Models.Pricing
                                    {
                                        PriceReference = "MjcwMDpHQlB+MjUwMDpHQlB+MjAyMC0wMS0xM1QwODoxNjozNSswMDAw",
                                        SalePrice = new Price
                                        {
                                            Value = 2700,
                                            Currency = "GBP",
                                            DecimalPlaces = 2
                                        },
                                        FaceValue = new Price
                                        {
                                            Value = 2500,
                                            Currency = "GBP",
                                            DecimalPlaces = 2
                                        },
                                        Percentage = 0,
                                        Offer = false,
                                        NoBookingFee = false,
                                        Timestamp = new DateTime(2020, 01, 13, 08, 16, 35)
                                    },
                                    Seats = new List<Seat>
                                    {
                                        new Seat
                                        {
                                            AggregateReference =
                                                "ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzItUDg=",
                                            ItemReference = "ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzItUDg=",
                                            Row = "P",
                                            Number = 8,
                                            IsAvailable = true,
                                            Attributes = new Attributes
                                            {
                                                RestrictedView = false,
                                                SideView = false
                                            },
                                            SeatIdentifier = "CIRCLE-P8",
                                            Pricing = new SDK.Inventory.Models.Pricing
                                            {
                                                PriceReference =
                                                    "MjcwMDpHQlB+MjUwMDpHQlB+MjAyMC0wMS0xM1QwODoxNjozNSswMDAw",
                                                SalePrice = new Price
                                                {
                                                    Value = 2700,
                                                    Currency = "GBP",
                                                    DecimalPlaces = 2
                                                },
                                                FaceValue = new Price
                                                {
                                                    Value = 2500,
                                                    Currency = "GBP",
                                                    DecimalPlaces = 2
                                                },
                                                Percentage = 0,
                                                Offer = false,
                                                NoBookingFee = false,
                                                Timestamp = new DateTime(2020, 01, 13, 08, 16, 35)
                                            }
                                        },
                                        new Seat
                                        {
                                            AggregateReference =
                                                "ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzItUDk=",
                                            ItemReference = "ODJLMTU2NTQzNUsyUzE2SzIyODI5NDlLMlMxN0syMjgyOTQ5SzItUDk=",
                                            Row = "P",
                                            Number = 9,
                                            IsAvailable = true,
                                            Attributes = new Attributes
                                            {
                                                RestrictedView = false,
                                                SideView = false
                                            },
                                            SeatIdentifier = "CIRCLE-P9",
                                            Pricing = new SDK.Inventory.Models.Pricing
                                            {
                                                PriceReference =
                                                    "MjcwMDpHQlB+MjUwMDpHQlB+MjAyMC0wMS0xM1QwODoxNjozNSswMDAw",
                                                SalePrice = new Price
                                                {
                                                    Value = 2700,
                                                    Currency = "GBP",
                                                    DecimalPlaces = 2
                                                },
                                                FaceValue = new Price
                                                {
                                                    Value = 2500,
                                                    Currency = "GBP",
                                                    DecimalPlaces = 2
                                                },
                                                Percentage = 0,
                                                Offer = false,
                                                NoBookingFee = false,
                                                Timestamp = new DateTime(2020, 01, 13, 08, 16, 35)
                                            }
                                        }
                                    },
                                    SeatLumps = new List<SeatLump>
                                    {
                                        new SeatLump
                                        {
                                            Seats = new List<string>
                                            {
                                                "CIRCLE-P8",
                                                "CIRCLE-P9"
                                            }
                                        }
                                    }
                                }
                            },
                        }
                    },
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetAvailability_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                "{\"errors\":[{\"field\":\"productId\",\"message\":\"The product ID can only contain numbers, letters and dashes\"}]}",
                HttpStatusCode.BadRequest,
                "productId: The product ID can only contain numbers, letters and dashes"
            ),

            // 401
            new TestCaseData(
                "{\"code\":401,\"message\":\"an invalid affiliateId has been specified, or the specified affiliate does not have access to this request\"}",
                HttpStatusCode.Unauthorized,
                "an invalid affiliateId has been specified, or the specified affiliate does not have access to this request"
            ),
            
            // 404
            new TestCaseData(
                "{\"code\":404,\"message\":\"Sorry, nothing was found\"}",
                HttpStatusCode.BadRequest,
                "Sorry, nothing was found"
            ),
        };
    }
}
