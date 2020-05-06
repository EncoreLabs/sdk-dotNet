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

            mockers.VerifyExecution<ProductSearchResponse>(BaseUrl, "v4/search", Method.GET,
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

            mockers.VerifyExecution<ApiResponse<AvailabilityRange>>(BaseUrl,
                $"v4/products/{productId}/availability-range", Method.GET);
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
                $"v4/availability/products/{productId}/quantity/{quantity}/from/{from:yyyyMMdd}/to/{to:yyyyMMdd}",
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

        #region GetSeatAvailability

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetSeatAvailability_IfTextIsNotSet_ThrowsArgumentException(string productId)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetSeatAvailability(productId, It.IsAny<int>());
            });
        }

        [TestCase(1587, 2)]
        public void GetSeatAvailability_IfProductIdAndQuantityAreSet_CallsApiWithRightParameters(int productId, int quantity)
        {
            mockers.SetupAnyExecution<ApiResponse<SeatAvailability>>();

            try
            {
                GetSeatAvailability(productId, quantity);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SeatAvailability>>(BaseUrl,
                $"v4/europa/availability/products/{productId}/quantity/{quantity}/seats", Method.GET);
        }

        [TestCase(1587, 2, "1/10/2020 3:56:51 PM")]
        public void GetSeatAvailability_IfProductIdAndQuantityAndPerformanceAreSet_CallsApiWithRightParameters(int productId, int quantity, string dateAsStr)
        {
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

            mockers.VerifyExecution<ApiResponse<SeatAvailability>>(BaseUrl,
                $"v4/europa/availability/products/{productId}/quantity/{quantity}/seats", Method.GET,
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
        public void GetSeatAvailability_IfProductIdAndQuantityAndPerformanceAreSet_CallsApiWithRightParameters(
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
            mockers.SetupAnyExecution<ApiResponse<SeatAvailability>>();

            try
            {
                GetSeatAvailability(productId, quantity, date, time);
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SeatAvailability>>(BaseUrl,
                $"v4/europa/availability/products/{productId}/quantity/{quantity}/seats", Method.GET,
                expectedQueryParameters: queryParameters);
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetSeatAvailability_IfApiResponseSuccessful_ReturnsAvailability))]
        public void GetSeatAvailabilityy_IfApiResponseSuccessful_ReturnsAvailability(
            string responseContent,
            SeatAvailability expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<SeatAvailability>>(responseContent);

            var actual = GetSeatAvailability(TestValidProductId, It.IsAny<int>());

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(InventoryServiceApiTestsSource), nameof(InventoryServiceApiTestsSource.GetSeatAvailability_IfApiResponseFailed_ThrowsApiException))]
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
                                            AggregateReference =
                                                "MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDE=",
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
                                            AggregateReference =
                                                "MTdLMjMwMjY2OEsyUzY3SzIzMDM2MDJLMlM0OUsyMzAzNjAzSzItMTYxMDI=",
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
