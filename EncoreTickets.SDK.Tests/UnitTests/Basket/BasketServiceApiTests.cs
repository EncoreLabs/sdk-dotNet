using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Basket;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers;
using EncoreTickets.SDK.Utilities.Exceptions;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket
{
    internal class BasketServiceApiTests : BasketServiceApi
    {
        private const string TestBasketValidReference = "12345678";

        private MockersForApiService mockers;
        
        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public BasketServiceApiTests() : base(new ApiContext(Environments.Sandbox))
        {
        }

        [SetUp]
        public void CreateMockers()
        {
            mockers = new MockersForApiService();
        }

        #region GetBasketDetails

        [TestCase(null)]
        [TestCase("")]
        public void GetBasketDetails_IfBasketReferenceIsNotSet_ThrowsArgumentException(string reference)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetBasketDetails(reference);
            });
        }

        [TestCase("791631")]
        [TestCase("not_id")]
        public void GetBasketDetails_IfBasketReferenceIsSet_CallsApiWithRightParameters(string reference)
        {
            mockers.SetupAnyExecution<ApiResponse<SDK.Basket.Models.Basket>>();

            try
            {
                GetBasketDetails(reference);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SDK.Basket.Models.Basket>>(BaseUrl, $"v1/baskets/{reference}", Method.GET);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetBasketDetails_IfApiResponseSuccessful_ReturnsBasket))]
        public void GetBasketDetails_IfApiResponseSuccessful_ReturnsBasket(string responseContent,
            SDK.Basket.Models.Basket expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent);

            var actual = GetBasketDetails(TestBasketValidReference);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetBasketDetails_IfApiResponseFailed_ThrowsApiException))]
        public void GetBasketDetails_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetBasketDetails(TestBasketValidReference);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region UpsertBasket

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertBasket_CallsApiWithRightParameters))]
        public void UpsertBasket_CallsApiWithRightParameters(SDK.Basket.Models.Basket basket, string requestBody)
        {
            mockers.SetupAnyExecution<ApiResponse<SDK.Basket.Models.Basket>>();

            try
            {
                UpsertBasket(basket);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SDK.Basket.Models.Basket>>(BaseUrl, "v1/baskets", Method.PATCH, bodyInJson: requestBody);
        }

        #endregion

        #region GetPromotionDetails

        [TestCase(null)]
        [TestCase("")]
        public void GetPromotionDetails_IfPromotionIdIsNotSet_ThrowsArgumentException(string id)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetPromotionDetails(id);
            });
        }

        #endregion
    }

    public static class BasketServiceApiTestsSource
    {
        public static IEnumerable<TestCaseData> GetBasketDetails_IfApiResponseSuccessful_ReturnsBasket = new[]
        {
            new TestCaseData(
                "{\"request\":{\"body\":\"\",\"query\":{},\"urlParams\":{\"reference\":\"791631\"}},\"response\":{\"reference\":\"791631\",\"checksum\":\"2001040924\",\"channelId\":\"integrator-qa-boxoffice\",\"mixed\":false,\"exchangeRate\":1,\"delivery\":null,\"allowFlexiTickets\":false,\"status\":\"active\",\"officeCurrency\":\"GBP\",\"shopperCurrency\":\"GBP\",\"expiredAt\":\"2020-01-04T09:39:28+0000\",\"createdAt\":\"2020-01-04T09:24:28+0000\",\"reservations\":[{\"id\":1,\"linkedReservationId\":0,\"venueId\":\"139\",\"venueName\":\"Dominion Theatre\",\"productId\":\"2017\",\"productType\":\"SHW\",\"productName\":\"White Christmas\",\"date\":\"2020-01-04T19:30:00+0000\",\"quantity\":2,\"items\":[{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co\",\"areaId\":\"DC\",\"areaName\":\"CIRCLE\",\"row\":\"P\",\"number\":\"31\",\"locationDescription\":\"\"},{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do\",\"areaId\":\"DC\",\"areaName\":\"CIRCLE\",\"row\":\"P\",\"number\":\"32\",\"locationDescription\":\"\"}],\"faceValueInOfficeCurrency\":{\"value\":3950,\"currency\":\"GBP\",\"decimalPlaces\":2},\"faceValueInShopperCurrency\":{\"value\":3950,\"currency\":\"GBP\",\"decimalPlaces\":2},\"salePriceInOfficeCurrency\":{\"value\":5100,\"currency\":\"GBP\",\"decimalPlaces\":2},\"salePriceInShopperCurrency\":{\"value\":5100,\"currency\":\"GBP\",\"decimalPlaces\":2},\"adjustedSalePriceInOfficeCurrency\":{\"value\":5100,\"currency\":\"GBP\",\"decimalPlaces\":2},\"adjustedSalePriceInShopperCurrency\":{\"value\":5100,\"currency\":\"GBP\",\"decimalPlaces\":2},\"adjustmentAmountInOfficeCurrency\":{\"value\":0,\"currency\":\"GBP\",\"decimalPlaces\":2},\"adjustmentAmountInShopperCurrency\":{\"value\":0,\"currency\":\"GBP\",\"decimalPlaces\":2}}],\"coupon\":null,\"appliedPromotion\":null,\"missedPromotions\":null},\"context\":null}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "791631",
                    Checksum = "2001040924",
                    ChannelId = "integrator-qa-boxoffice",
                    Mixed = false,
                    ExchangeRate = 1,
                    Delivery = null,
                    AllowFlexiTickets = false,
                    Status = "active",
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2020, 01, 04, 09, 39, 28, TimeSpan.Zero),
                    CreatedAt = new DateTimeOffset(2020, 01, 04, 09, 24, 28, TimeSpan.Zero),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 1,
                            LinkedReservationId = 0,
                            VenueId = "139",
                            VenueName = "Dominion Theatre",
                            ProductId = "2017",
                            ProductType = "SHW",
                            ProductName = "White Christmas",
                            Date = new DateTimeOffset(2020, 01, 04, 19, 30, 00, TimeSpan.Zero),
                            Quantity = 2,
                            Items = new List<Seat>
                            {
                                new Seat
                                {
                                    AggregateReference = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co",
                                    AreaId = "DC",
                                    AreaName = "CIRCLE",
                                    Row = "P",
                                    Number = "31",
                                    LocationDescription = ""
                                },
                                new Seat
                                {
                                    AggregateReference = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do",
                                    AreaId = "DC",
                                    AreaName = "CIRCLE",
                                    Row = "P",
                                    Number = "32",
                                    LocationDescription = ""
                                },
                            },
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 3950,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 3950,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                        }
                    },
                    Coupon = null,
                    AppliedPromotion = null,
                    MissedPromotions = null
                }
                ),
        };

        public static IEnumerable<TestCaseData> GetBasketDetails_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                "{\"request\":{\"body\":\"\",\"query\":{},\"urlParams\":{\"reference\":\"test\"}},\"response\":\"\",\"context\":{\"errors\":[{\"message\":\"Insufficient data has been supplied for \\\"test\\\" basket to complete this request.\"}]}}",
                HttpStatusCode.BadRequest,
                "Insufficient data has been supplied for \"test\" basket to complete this request."
            ),

            // 404
            new TestCaseData(
                "{\"request\":{\"body\":\"\",\"query\":{},\"urlParams\":{\"reference\":\"5926058\"}},\"response\":\"\",\"context\":{\"errors\":[{\"message\":\"Basket with reference \\\"5926058\\\" was not found.\"}]}}",
                HttpStatusCode.NotFound,
                "Basket with reference \"5926058\" was not found."
            ),
        };

        public static IEnumerable<TestCaseData> UpsertBasket_CallsApiWithRightParameters = new[]
        {
            new TestCaseData(
                new SDK.Basket.Models.Basket
                {
                    Reference = "791631",
                    Checksum = "2001040924",
                    ChannelId = "integrator-qa-boxoffice",
                    Mixed = false,
                    ExchangeRate = 1,
                    Delivery = new Delivery
                    {
                        Charge = new Price
                        {
                            Value = 3950,
                            Currency = "GBP",
                            DecimalPlaces = 2
                        },
                        Method = "test"
                    },
                    AllowFlexiTickets = false,
                    Status = "active",
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ShopperReference = "test",
                    ExpiredAt = new DateTimeOffset(2020, 01, 04, 09, 39, 28, TimeSpan.Zero),
                    CreatedAt = new DateTimeOffset(2020, 01, 04, 09, 24, 28, TimeSpan.Zero),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 1,
                            LinkedReservationId = 0,
                            VenueId = "139",
                            VenueName = "Dominion Theatre",
                            ProductId = "2017",
                            ProductType = "SHW",
                            ProductName = "White Christmas",
                            Date = new DateTimeOffset(2020, 01, 04, 19, 30, 00, TimeSpan.Zero),
                            Quantity = 2,
                            Items = new List<Seat>
                            {
                                new Seat
                                {
                                    AggregateReference = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co",
                                    AreaId = "DC",
                                    AreaName = "CIRCLE",
                                    Row = "P",
                                    Number = "31",
                                    LocationDescription = ""
                                },
                                new Seat
                                {
                                    AggregateReference = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do",
                                    AreaId = "DC",
                                    AreaName = "CIRCLE",
                                    Row = "P",
                                    Number = "32",
                                    LocationDescription = ""
                                },
                            },
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 3950,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 3950,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2
                            },
                        }
                    },
                    Coupon = null,
                    AppliedPromotion = null,
                    MissedPromotions = null
                },
                "{\"reference\":\"791631\",\"channelId\":\"integrator-qa-boxoffice\",\"delivery\":{\"method\":\"test\",\"charge\":{\"value\":3950,\"currency\":\"GBP\",\"decimalPlaces\":2}},\"hasFlexiTickets\":false,\"shopperCurrency\":\"GBP\",\"shopperReference\":\"test\",\"reservations\":[{\"venueId\":\"139\",\"productId\":\"2017\",\"date\":\"2020-01-04T19:30:00+00:00\",\"quantity\":2,\"items\":[{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co\"},{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do\"}]}],\"coupon\":null}"
            ),
        };
    }
}
