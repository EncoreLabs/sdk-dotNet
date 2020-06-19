using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Basket;
using EncoreTickets.SDK.Basket.Exceptions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket
{
    internal class BasketServiceApiTests : BasketServiceApi
    {
        private const string TestBasketValidReference = "12345678";
        private const string TestPromotionValidId = "12345678";

        private readonly SDK.Basket.Models.Basket testBasketValid = new SDK.Basket.Models.Basket();

        private ApiServiceMocker mockers;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public BasketServiceApiTests()
            : base(new ApiContext(Environments.Sandbox))
        {
        }

        [SetUp]
        public void CreateMockers()
        {
            mockers = new ApiServiceMocker();
        }

        #region GetBasketDetails

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
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

            mockers.VerifyExecution<ApiResponse<SDK.Basket.Models.Basket>>(
                BaseUrl,
                $"v1/baskets/{reference}",
                Method.GET,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetBasketDetails_IfApiResponseSuccessful_ReturnsBasket))]
        public void GetBasketDetails_IfApiResponseSuccessful_ReturnsBasket(
            string responseContent,
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

        #region GetBasketDeliveryOptions

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetBasketDeliveryOptions_IfBasketReferenceIsNotSet_ThrowsArgumentException(string reference)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetBasketDeliveryOptions(reference);
            });
        }

        [TestCase("791631")]
        [TestCase("not_id")]
        public void GetBasketDeliveryOptions_IfBasketReferenceIsSet_CallsApiWithRightParameters(string reference)
        {
            mockers.SetupAnyExecution<ApiResponseWithResultsBlock<List<Delivery>>>();

            try
            {
                GetBasketDeliveryOptions(reference);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponseWithResultsBlock<List<Delivery>>>(
                BaseUrl,
                $"v1/baskets/{reference}/deliveryOptions",
                Method.GET,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetBasketDeliveryOptions_IfApiResponseSuccessful_ReturnsBasket))]
        public void GetBasketDeliveryOptions_IfApiResponseSuccessful_ReturnsBasket(
            string responseContent,
            List<Delivery> expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponseWithResultsBlock<List<Delivery>>>(responseContent);

            var actual = GetBasketDeliveryOptions(TestBasketValidReference);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetBasketDeliveryOptions_IfApiResponseFailed_ThrowsApiException))]
        public void GetBasketDeliveryOptions_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponseWithResultsBlock<List<Delivery>>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetBasketDeliveryOptions(TestBasketValidReference);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region UpsertBasket

        [Test]
        public void UpsertBasket_IfBasketIsNotSet_ThrowsArgumentException()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                UpsertBasket((SDK.Basket.Models.Basket)null);
            });
        }

        [Test]
        public void UpsertBasket_IfBasketParametersAreNotSet_ThrowsArgumentException()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                UpsertBasket((UpsertBasketParameters)null);
            });
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertBasket_IfBasketIsPassed_CallsApiWithRightParameters))]
        public void UpsertBasket_IfBasketIsPassed_CallsApiWithRightParameters(
            SDK.Basket.Models.Basket basket,
            bool? hasFlexiTickets,
            string requestBody)
        {
            mockers.SetupAnyExecution<ApiResponse<SDK.Basket.Models.Basket>>();

            try
            {
                UpsertBasket(basket, hasFlexiTickets);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SDK.Basket.Models.Basket>>(
                BaseUrl,
                "v1/baskets",
                Method.PATCH,
                bodyInJson: requestBody,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertBasket_IfBasketParametersArePassed_CallsApiWithRightParameters))]
        public void UpsertBasket_IfBasketParametersArePassed_CallsApiWithRightParameters(UpsertBasketParameters parameters, string requestBody)
        {
            mockers.SetupAnyExecution<ApiResponse<SDK.Basket.Models.Basket>>();

            try
            {
                UpsertBasket(parameters);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SDK.Basket.Models.Basket>>(
                BaseUrl,
                "v1/baskets",
                Method.PATCH,
                bodyInJson: requestBody,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertBasket_IfApiResponseSuccessful_ReturnsBasket))]
        public void UpsertBasket_IfApiResponseSuccessful_ReturnsBasket(
            string responseContent,
            SDK.Basket.Models.Basket expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent);

            var actual = UpsertBasket(testBasketValid);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertBasket_IfApiResponseFailed_ThrowsApiException))]
        public void UpsertBasket_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = UpsertBasket(testBasketValid);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region UpsertPromotion

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void UpsertPromotion_IfBasketReferenceIsNotSet_ThrowsArgumentException(string reference)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                UpsertPromotion(reference, It.IsAny<Coupon>());
            });
        }

        [TestCaseSource(
            typeof(BasketServiceApiTestsSource),
            nameof(BasketServiceApiTestsSource.UpsertPromotion_IfBasketReferenceAndCouponAreSet_CallsApiWithRightParameters))]
        public void UpsertPromotion_IfBasketReferenceAndCouponAreSet_CallsApiWithRightParameters(
            string reference,
            Coupon coupon,
            string expectedBody)
        {
            mockers.SetupAnyExecution<ApiResponse<SDK.Basket.Models.Basket>>();

            try
            {
                UpsertPromotion(reference, coupon);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SDK.Basket.Models.Basket>>(
                BaseUrl,
                $"v1/baskets/{reference}/applyPromotion",
                Method.PATCH,
                bodyInJson: expectedBody,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(
            typeof(BasketServiceApiTestsSource),
            nameof(BasketServiceApiTestsSource.UpsertPromotion_IfBasketReferenceAndCouponNameAreSet_CallsApiWithRightParameters))]
        public void UpsertPromotion_IfBasketReferenceAndCouponNameAreSet_CallsApiWithRightParameters(
            string reference,
            string couponName,
            string expectedBody)
        {
            mockers.SetupAnyExecution<ApiResponse<SDK.Basket.Models.Basket>>();

            try
            {
                UpsertPromotion(reference, couponName);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SDK.Basket.Models.Basket>>(
                BaseUrl,
                $"v1/baskets/{reference}/applyPromotion",
                Method.PATCH,
                bodyInJson: expectedBody,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertPromotion_IfApiResponseSuccessfulAndPromoCodeValid_ReturnsBasket))]
        public void UpsertPromotion_IfApiResponseSuccessfulAndPromoCodeValid_ReturnsBasket(
            string responseContent,
            SDK.Basket.Models.Basket expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent);

            var actual = UpsertPromotion(TestBasketValidReference, It.IsAny<Coupon>());

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertPromotion_IfApiResponseSuccessfulButPromoCodeInvalid_ThrowsInvalidPromoCodeException))]
        public void UpsertPromotion_IfApiResponseSuccessfulButPromoCodeInvalid_ThrowsInvalidPromoCodeException(
            Coupon coupon,
            string responseContent,
            string expectedMessage)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent);

            var exception = Assert.Catch<InvalidPromoCodeException>(() =>
            {
                UpsertPromotion(TestBasketValidReference, coupon);
            });

            Assert.AreEqual(expectedMessage, exception.Message);
            AssertExtension.AreObjectsValuesEqual(coupon, exception.Coupon);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertPromotion_IfApiResponseFailedWith400Code_ThrowsBasketCannotBeModifiedException))]
        public void UpsertPromotion_IfApiResponseFailedWith400Code_ThrowsBasketCannotBeModifiedException(
            string responseContent,
            string expectedMessage)
        {
            var code = HttpStatusCode.BadRequest;
            mockers.SetupFailedExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent, code);

            var exception = Assert.Catch<BasketCannotBeModifiedException>(() =>
            {
                UpsertPromotion(TestBasketValidReference, It.IsAny<string>());
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
            Assert.AreEqual(TestBasketValidReference, exception.BasketId);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertPromotion_IfApiResponseFailedWith404Code_ThrowsBasketNotFoundException))]
        public void UpsertPromotion_IfApiResponseFailedWith404Code_ThrowsBasketNotFoundException(
            string responseContent,
            string expectedMessage)
        {
            var code = HttpStatusCode.NotFound;
            mockers.SetupFailedExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent, code);

            var exception = Assert.Catch<BasketNotFoundException>(() =>
            {
                UpsertPromotion(TestBasketValidReference, It.IsAny<string>());
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
            Assert.AreEqual(TestBasketValidReference, exception.BasketId);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.UpsertPromotion_IfApiResponseFailedWithUnexpectedCode_ThrowsApiException))]
        public void UpsertPromotion_IfApiResponseFailedWithUnexpectedCode_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = UpsertPromotion(TestBasketValidReference, It.IsAny<string>());
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region ClearBasket

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ClearBasket_IfBasketReferenceIsNotSet_ThrowsArgumentException(string reference)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                ClearBasket(reference);
            });
        }

        [TestCase("791631")]
        [TestCase("not_id")]
        public void ClearBasket_IfBasketReferenceIsSet_CallsApiWithRightParameters(string reference)
        {
            mockers.SetupAnyExecution<ApiResponse<SDK.Basket.Models.Basket>>();

            try
            {
                ClearBasket(reference);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SDK.Basket.Models.Basket>>(
                BaseUrl,
                $"v1/baskets/{reference}/clear",
                Method.PATCH,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.ClearBasket_IfApiResponseSuccessful_ReturnsBasket))]
        public void ClearBasket_IfApiResponseSuccessful_ReturnsBasket(
            string responseContent,
            SDK.Basket.Models.Basket expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent);

            var actual = ClearBasket(TestBasketValidReference);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.ClearBasket_IfApiResponseFailed_ThrowsApiException))]
        public void ClearBasket_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = ClearBasket(TestBasketValidReference);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region RemoveReservation

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RemoveReservation_IfBasketReferenceIsNotSet_ThrowsArgumentException(string reference)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                RemoveReservation(reference, It.IsAny<int>());
            });
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RemoveReservation_IfReservationIdIsNotSet_ThrowsArgumentException(string id)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                RemoveReservation(TestBasketValidReference, id);
            });
        }

        [TestCase("791631", "1")]
        [TestCase("not_id", "id")]
        public void RemoveReservation_IfBasketReferenceAndReservationIdAreSet_CallsApiWithRightParameters(string reference, string reservationId)
        {
            mockers.SetupAnyExecution<ApiResponse<SDK.Basket.Models.Basket>>();

            try
            {
                RemoveReservation(reference, reservationId);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<SDK.Basket.Models.Basket>>(
                BaseUrl,
                $"v1/baskets/{reference}/reservations/{reservationId}",
                Method.DELETE,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.RemoveReservation_IfApiResponseSuccessful_ReturnsBasket))]
        public void RemoveReservation_IfApiResponseSuccessful_ReturnsBasket(
            string responseContent,
            SDK.Basket.Models.Basket expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent);

            var actual = RemoveReservation(TestBasketValidReference, 1);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.RemoveReservation_IfApiResponseFailed_ThrowsApiException))]
        public void RemoveReservation_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<SDK.Basket.Models.Basket>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = RemoveReservation(TestBasketValidReference, 1);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region GetPromotions

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetPromotions_CallsApiWithRightParameters))]
        public void GetPromotions_CallsApiWithRightParameters(PageRequest pageRequest)
        {
            mockers.SetupAnyExecution<ApiResponseWithResultsBlock<List<Promotion>>>();
            var queryParameters = new Dictionary<string, object>();
            if (pageRequest?.Limit != null)
            {
                queryParameters.Add("limit", pageRequest.Limit);
            }

            if (pageRequest?.Page != null)
            {
                queryParameters.Add("page", pageRequest.Page);
            }

            try
            {
                GetPromotions(pageRequest);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponseWithResultsBlock<List<Promotion>>>(
                BaseUrl,
                "v1/promotions",
                Method.GET,
                expectedHeaders: null,
                expectedQueryParameters: queryParameters);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetPromotions_IfApiResponseSuccessful_ReturnsPromotions))]
        public void GetPromotions_IfApiResponseSuccessful_ReturnsPromotions(
            string responseContent,
            List<Promotion> expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponseWithResultsBlock<List<Promotion>>>(responseContent);

            var actual = GetPromotions();

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetPromotions_IfApiResponseFailed_ThrowsApiException))]
        public void GetPromotions_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponseWithResultsBlock<List<Promotion>>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetPromotions();
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region GetPromotionDetails

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetPromotionDetails_IfPromotionIdIsNotSet_ThrowsArgumentException(string id)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetPromotionDetails(id);
            });
        }

        [TestCase("206000019")]
        [TestCase("not_id")]
        public void GetPromotionDetails_IfPromotionIdIsSet_CallsApiWithRightParameters(string id)
        {
            mockers.SetupAnyExecution<ApiResponse<Promotion>>();

            try
            {
                GetPromotionDetails(id);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<Promotion>>(
                BaseUrl,
                $"v1/promotions/{id}",
                Method.GET,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetPromotionDetails_IfApiResponseSuccessful_ReturnsPromotion))]
        public void GetPromotionDetails_IfApiResponseSuccessful_ReturnsPromotion(
            string responseContent,
            Promotion expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<Promotion>>(responseContent);

            var actual = GetPromotionDetails(TestPromotionValidId);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(BasketServiceApiTestsSource), nameof(BasketServiceApiTestsSource.GetPromotionDetails_IfApiResponseFailed_ThrowsApiException))]
        public void GetPromotionDetails_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<Promotion>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetPromotionDetails(TestPromotionValidId);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion
    }

    internal static class BasketServiceApiTestsSource
    {
        #region GetBasketDetails

        public static IEnumerable<TestCaseData> GetBasketDetails_IfApiResponseSuccessful_ReturnsBasket { get; } = new[]
        {
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": """",
    ""query"": {},
    ""urlParams"": {
      ""reference"": ""791631""
    }
  },
  ""response"": {
    ""reference"": ""791631"",
    ""checksum"": ""2001040924"",
    ""channelId"": ""integrator-qa-boxoffice"",
    ""mixed"": false,
    ""exchangeRate"": 1,
    ""delivery"": null,
    ""allowFlexiTickets"": false,
    ""status"": ""active"",
    ""officeCurrency"": ""GBP"",
    ""shopperCurrency"": ""GBP"",
    ""expiredAt"": ""2020-01-04T09:39:28+0000"",
    ""createdAt"": ""2020-01-04T09:24:28+0000"",
    ""reservations"": [
      {
        ""id"": 1,
        ""linkedReservationId"": 0,
        ""venueId"": ""139"",
        ""venueName"": ""Dominion Theatre"",
        ""productId"": ""2017"",
        ""productType"": ""SHW"",
        ""productName"": ""White Christmas"",
        ""date"": ""2020-01-04T19:30:00+0000"",
        ""quantity"": 2,
        ""items"": [
          {
            ""aggregateReference"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co"",
            ""areaId"": ""DC"",
            ""areaName"": ""CIRCLE"",
            ""row"": ""P"",
            ""number"": ""31"",
            ""locationDescription"": """"
          },
          {
            ""aggregateReference"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do"",
            ""areaId"": ""DC"",
            ""areaName"": ""CIRCLE"",
            ""row"": ""P"",
            ""number"": ""32"",
            ""locationDescription"": """"
          }
        ],
        ""faceValueInOfficeCurrency"": {
          ""value"": 3950,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""faceValueInShopperCurrency"": {
          ""value"": 3950,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInOfficeCurrency"": {
          ""value"": 5100,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInShopperCurrency"": {
          ""value"": 5100,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInOfficeCurrency"": {
          ""value"": 5100,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInShopperCurrency"": {
          ""value"": 5100,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInOfficeCurrency"": {
          ""value"": 0,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInShopperCurrency"": {
          ""value"": 0,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        }
      }
    ],
    ""coupon"": null,
    ""appliedPromotion"": null,
    ""missedPromotions"": null
  },
  ""context"": null
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "791631",
                    Checksum = "2001040924",
                    ChannelId = "integrator-qa-boxoffice",
                    Mixed = false,
                    ExchangeRate = 1,
                    Delivery = null,
                    AllowFlexiTickets = false,
                    Status = BasketStatus.Active,
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
                            Items = new List<ReservationItem>
                            {
                                new ReservationItem
                                {
                                    AggregateReference =
                                        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co",
                                    AreaId = "DC",
                                    AreaName = "CIRCLE",
                                    Row = "P",
                                    Number = "31",
                                    LocationDescription = "",
                                },
                                new ReservationItem
                                {
                                    AggregateReference =
                                        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do",
                                    AreaId = "DC",
                                    AreaName = "CIRCLE",
                                    Row = "P",
                                    Number = "32",
                                    LocationDescription = "",
                                },
                            },
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 3950,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 3950,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 5100,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                    },
                    Coupon = null,
                    AppliedPromotion = null,
                    MissedPromotions = null,
                }),
        };

        public static IEnumerable<TestCaseData> GetBasketDetails_IfApiResponseFailed_ThrowsApiException { get; } = new[]
        {
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""test""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Insufficient data has been supplied for \""test\"" basket to complete this request.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Insufficient data has been supplied for \"test\" basket to complete this request."),

            // 404
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""5926058""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Basket with reference \""5926058\"" was not found.""
            }
        ]
    }
}",
                HttpStatusCode.NotFound,
                "Basket with reference \"5926058\" was not found."),
        };

        #endregion

        #region GetBasketDeliveryOptions

        public static IEnumerable<TestCaseData> GetBasketDeliveryOptions_IfApiResponseSuccessful_ReturnsBasket { get; } =
            new[]
            {
                new TestCaseData(
                    @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""8604612""
        }
    },
    ""response"": {
        ""results"": [
            {
                ""method"": ""postage"",
                ""charge"": {
                    ""value"": 145,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                }
            },
            {
                ""method"": ""eticket"",
                ""charge"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                }
            },
            {
                ""method"": ""collection"",
                ""charge"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                }
            },
            {
                ""method"": ""evoucher"",
                ""charge"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                }
            }
        ]
    },
    ""context"": null
}",
                    new List<Delivery>
                    {
                        new Delivery
                        {
                            Method = DeliveryMethod.Postage,
                            Charge = new Price
                            {
                                Value = 145,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                        new Delivery
                        {
                            Method = DeliveryMethod.Eticket,
                            Charge = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                        new Delivery
                        {
                            Method = DeliveryMethod.Collection,
                            Charge = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                        new Delivery
                        {
                            Method = DeliveryMethod.Evoucher,
                            Charge = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                    }),
            };

        public static IEnumerable<TestCaseData> GetBasketDeliveryOptions_IfApiResponseFailed_ThrowsApiException { get; } =
            new[]
            {
                // 400
                new TestCaseData(
                    @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""test""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Insufficient data has been supplied for \""test\"" basket to complete this request.""
            }
        ]
    }
}",
                    HttpStatusCode.BadRequest,
                    "Insufficient data has been supplied for \"test\" basket to complete this request."),

                // 404
                new TestCaseData(
                    @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""86046""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Basket with reference \""86046\"" was not found.""
            }
        ]
    }
}",
                    HttpStatusCode.NotFound,
                    "Basket with reference \"86046\" was not found."),
            };

        #endregion

        #region UpsertBasket

        public static IEnumerable<TestCaseData> UpsertBasket_IfBasketIsPassed_CallsApiWithRightParameters { get; } =
            new[]
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
                                DecimalPlaces = 2,
                            },
                            Method = DeliveryMethod.Eticket,
                        },
                        AllowFlexiTickets = false,
                        Status = BasketStatus.Active,
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
                                Items = new List<ReservationItem>
                                {
                                    new ReservationItem
                                    {
                                        AggregateReference =
                                            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co",
                                        AreaId = "DC",
                                        AreaName = "CIRCLE",
                                        Row = "P",
                                        Number = "31",
                                        LocationDescription = "",
                                    },
                                    new ReservationItem
                                    {
                                        AggregateReference =
                                            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do",
                                        AreaId = "DC",
                                        AreaName = "CIRCLE",
                                        Row = "P",
                                        Number = "32",
                                        LocationDescription = "",
                                    },
                                },
                                FaceValueInOfficeCurrency = new Price
                                {
                                    Value = 3950,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                FaceValueInShopperCurrency = new Price
                                {
                                    Value = 3950,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                SalePriceInOfficeCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                SalePriceInShopperCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustedSalePriceInOfficeCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustedSalePriceInShopperCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustmentAmountInOfficeCurrency = new Price
                                {
                                    Value = 0,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustmentAmountInShopperCurrency = new Price
                                {
                                    Value = 0,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                            },
                        },
                        Coupon = null,
                        AppliedPromotion = null,
                        MissedPromotions = null,
                    },
                    null,
                    "{\"reference\":\"791631\",\"channelId\":\"integrator-qa-boxoffice\",\"delivery\":{\"method\":\"eticket\",\"charge\":{\"value\":3950,\"currency\":\"GBP\",\"decimalPlaces\":2}},\"hasFlexiTickets\":false,\"shopperCurrency\":\"GBP\",\"shopperReference\":\"test\",\"reservations\":[{\"venueId\":\"139\",\"productId\":\"2017\",\"date\":\"2020-01-04T19:30:00+00:00\",\"quantity\":2,\"items\":[{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co\"},{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do\"}]}],\"coupon\":null}"),
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
                                DecimalPlaces = 2,
                            },
                            Method = DeliveryMethod.Eticket,
                        },
                        AllowFlexiTickets = false,
                        Status = BasketStatus.Active,
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
                                Items = new List<ReservationItem>
                                {
                                    new ReservationItem
                                    {
                                        AggregateReference =
                                            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co",
                                        AreaId = "DC",
                                        AreaName = "CIRCLE",
                                        Row = "P",
                                        Number = "31",
                                        LocationDescription = "",
                                    },
                                    new ReservationItem
                                    {
                                        AggregateReference =
                                            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do",
                                        AreaId = "DC",
                                        AreaName = "CIRCLE",
                                        Row = "P",
                                        Number = "32",
                                        LocationDescription = "",
                                    },
                                },
                                FaceValueInOfficeCurrency = new Price
                                {
                                    Value = 3950,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                FaceValueInShopperCurrency = new Price
                                {
                                    Value = 3950,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                SalePriceInOfficeCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                SalePriceInShopperCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustedSalePriceInOfficeCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustedSalePriceInShopperCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustmentAmountInOfficeCurrency = new Price
                                {
                                    Value = 0,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustmentAmountInShopperCurrency = new Price
                                {
                                    Value = 0,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                            },
                        },
                        Coupon = null,
                        AppliedPromotion = null,
                        MissedPromotions = null,
                    },
                    false,
                    "{\"reference\":\"791631\",\"channelId\":\"integrator-qa-boxoffice\",\"delivery\":{\"method\":\"eticket\",\"charge\":{\"value\":3950,\"currency\":\"GBP\",\"decimalPlaces\":2}},\"hasFlexiTickets\":false,\"shopperCurrency\":\"GBP\",\"shopperReference\":\"test\",\"reservations\":[{\"venueId\":\"139\",\"productId\":\"2017\",\"date\":\"2020-01-04T19:30:00+00:00\",\"quantity\":2,\"items\":[{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co\"},{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do\"}]}],\"coupon\":null}"),
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
                                DecimalPlaces = 2,
                            },
                            Method = DeliveryMethod.Eticket,
                        },
                        AllowFlexiTickets = false,
                        Status = BasketStatus.Active,
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
                                Items = new List<ReservationItem>
                                {
                                    new ReservationItem
                                    {
                                        AggregateReference =
                                            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co",
                                        AreaId = "DC",
                                        AreaName = "CIRCLE",
                                        Row = "P",
                                        Number = "31",
                                        LocationDescription = "",
                                    },
                                    new ReservationItem
                                    {
                                        AggregateReference =
                                            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do",
                                        AreaId = "DC",
                                        AreaName = "CIRCLE",
                                        Row = "P",
                                        Number = "32",
                                        LocationDescription = "",
                                    },
                                },
                                FaceValueInOfficeCurrency = new Price
                                {
                                    Value = 3950,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                FaceValueInShopperCurrency = new Price
                                {
                                    Value = 3950,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                SalePriceInOfficeCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                SalePriceInShopperCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustedSalePriceInOfficeCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustedSalePriceInShopperCurrency = new Price
                                {
                                    Value = 5100,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustmentAmountInOfficeCurrency = new Price
                                {
                                    Value = 0,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustmentAmountInShopperCurrency = new Price
                                {
                                    Value = 0,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                            },
                        },
                        Coupon = null,
                        AppliedPromotion = null,
                        MissedPromotions = null,
                    },
                    true,
                    "{\"reference\":\"791631\",\"channelId\":\"integrator-qa-boxoffice\",\"delivery\":{\"method\":\"eticket\",\"charge\":{\"value\":3950,\"currency\":\"GBP\",\"decimalPlaces\":2}},\"hasFlexiTickets\":true,\"shopperCurrency\":\"GBP\",\"shopperReference\":\"test\",\"reservations\":[{\"venueId\":\"139\",\"productId\":\"2017\",\"date\":\"2020-01-04T19:30:00+00:00\",\"quantity\":2,\"items\":[{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co\"},{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do\"}]}],\"coupon\":null}"),
                new TestCaseData(
                    new SDK.Basket.Models.Basket
                    {
                        Reference = "1010101",
                        ChannelId = "encoretickets",
                        Delivery = new Delivery
                        {
                            Method = DeliveryMethod.Collection,
                            Charge = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                        AllowFlexiTickets = true,
                        ShopperCurrency = "GBP",
                        ShopperReference = "5ec76ed81e39699b102d01a39fe74f1c",
                        Reservations = new List<Reservation>
                        {
                            new Reservation
                            {
                                Id = 1,
                                VenueId = "163",
                                ProductId = "2102",
                                Date = new DateTimeOffset(2019, 04, 10, 19, 30, 00, TimeSpan.Zero),
                                Quantity = 1,
                                Items = new List<ReservationItem>
                                {
                                    new ReservationItem
                                    {
                                        AggregateReference =
                                            "eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==",
                                    },
                                },
                            },
                            new Reservation
                            {
                                Id = 2,
                                LinkedReservationId = 1,
                                VenueId = "901",
                                ProductId = "9001",
                                ProductType = "FLX",
                                Date = new DateTimeOffset(2020, 10, 23, 00, 01, 00, TimeSpan.FromHours(1)),
                                Quantity = 1,
                                FaceValueInOfficeCurrency = new Price
                                {
                                    Value = 199,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                FaceValueInShopperCurrency = new Price
                                {
                                    Value = 199,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                SalePriceInOfficeCurrency = new Price
                                {
                                    Value = 199,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                SalePriceInShopperCurrency = new Price
                                {
                                    Value = 199,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustedSalePriceInOfficeCurrency = new Price
                                {
                                    Value = 199,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustedSalePriceInShopperCurrency = new Price
                                {
                                    Value = 199,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustmentAmountInOfficeCurrency = new Price
                                {
                                    Value = 0,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                                AdjustmentAmountInShopperCurrency = new Price
                                {
                                    Value = 0,
                                    Currency = "GBP",
                                    DecimalPlaces = 2,
                                },
                            },
                            new Reservation(),
                        },
                        Coupon = new Coupon
                        {
                            Code = "SAMPLE_SOURCE_CODE",
                        },
                    },
                    null,
                    "{\"reference\":\"1010101\",\"channelId\":\"encoretickets\",\"delivery\":{\"method\":\"collection\",\"charge\":{\"value\":1000,\"currency\":\"GBP\",\"decimalPlaces\":2}},\"hasFlexiTickets\":true,\"shopperCurrency\":\"GBP\",\"shopperReference\":\"5ec76ed81e39699b102d01a39fe74f1c\",\"reservations\":[{\"venueId\":\"163\",\"productId\":\"2102\",\"date\":\"2019-04-10T19:30:00+00:00\",\"quantity\":1,\"items\":[{\"aggregateReference\":\"eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==\"}]}],\"coupon\":{\"code\":\"SAMPLE_SOURCE_CODE\"}}"),
                new TestCaseData(
                    new SDK.Basket.Models.Basket
                    {
                        Reference = "1010101",
                        ChannelId = "encoretickets",
                        Delivery = new Delivery
                        {
                            Method = DeliveryMethod.Collection,
                            Charge = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                        AllowFlexiTickets = true,
                        ShopperCurrency = "GBP",
                        ShopperReference = "5ec76ed81e39699b102d01a39fe74f1c",
                        Reservations = new List<Reservation>
                        {
                            new Reservation
                            {
                                VenueId = "163",
                                ProductId = "2102",
                                Date = new DateTimeOffset(2019, 04, 10, 19, 30, 00, TimeSpan.Zero),
                                Quantity = 1,
                                Items = new List<ReservationItem>
                                {
                                    new ReservationItem
                                    {
                                        AggregateReference =
                                            "eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==",
                                    },
                                },
                            },
                        },
                        Coupon = new Coupon
                        {
                            Code = "SAMPLE_SOURCE_CODE",
                        },
                    },
                    false,
                    "{\"reference\":\"1010101\",\"channelId\":\"encoretickets\",\"delivery\":{\"method\":\"collection\",\"charge\":{\"value\":1000,\"currency\":\"GBP\",\"decimalPlaces\":2}},\"hasFlexiTickets\":false,\"shopperCurrency\":\"GBP\",\"shopperReference\":\"5ec76ed81e39699b102d01a39fe74f1c\",\"reservations\":[{\"venueId\":\"163\",\"productId\":\"2102\",\"date\":\"2019-04-10T19:30:00+00:00\",\"quantity\":1,\"items\":[{\"aggregateReference\":\"eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==\"}]}],\"coupon\":{\"code\":\"SAMPLE_SOURCE_CODE\"}}"),
                new TestCaseData(
                    new SDK.Basket.Models.Basket
                    {
                        ChannelId = "{{affiliateId}}",
                        Reservations = new List<Reservation>
                        {
                            new Reservation
                            {
                                VenueId = "138",
                                ProductId = "1587",
                                Date = new DateTimeOffset(2020, 10, 23, 19, 30, 00, TimeSpan.Zero),
                                Quantity = 1,
                                Items = new List<ReservationItem>
                                {
                                    new ReservationItem
                                    {
                                        AggregateReference =
                                            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDU7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.-M7KQoFh1N7PKWestjbdbVR7EkwbsrVh9jwtsGMJh_k",
                                    },
                                },
                            },
                        },
                    },
                    null,
                    "{\"reference\":null,\"channelId\":\"{{affiliateId}}\",\"delivery\":null,\"hasFlexiTickets\":false,\"shopperCurrency\":null,\"shopperReference\":null,\"reservations\":[{\"venueId\":\"138\",\"productId\":\"1587\",\"date\":\"2020-10-23T19:30:00+00:00\",\"quantity\":1,\"items\":[{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDU7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.-M7KQoFh1N7PKWestjbdbVR7EkwbsrVh9jwtsGMJh_k\"}]}],\"coupon\":null}"),
            };

        public static IEnumerable<TestCaseData> UpsertBasket_IfBasketParametersArePassed_CallsApiWithRightParameters { get; } = new[]
        {
            new TestCaseData(
                new UpsertBasketParameters
                {
                    Reference = "791631",
                    ChannelId = "integrator-qa-boxoffice",
                    Delivery = new Delivery
                    {
                        Charge = new Price
                        {
                            Value = 3950,
                            Currency = "GBP",
                            DecimalPlaces = 2,
                        },
                        Method = DeliveryMethod.Eticket,
                    },
                    ShopperCurrency = "GBP",
                    ShopperReference = "test",
                    Reservations = new List<ReservationParameters>
                    {
                        new ReservationParameters
                        {
                            VenueId = "139",
                            ProductId = "2017",
                            Date = new DateTimeOffset(2020, 01, 04, 19, 30, 00, TimeSpan.Zero),
                            Quantity = 2,
                            Items = new List<ReservationItemParameters>
                            {
                                new ReservationItemParameters
                                {
                                    AggregateReference =
                                        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co",
                                },
                                new ReservationItemParameters
                                {
                                    AggregateReference =
                                        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do",
                                },
                            },
                        },
                    },
                    Coupon = null,
                },
                "{\"reference\":\"791631\",\"channelId\":\"integrator-qa-boxoffice\",\"delivery\":{\"method\":\"eticket\",\"charge\":{\"value\":3950,\"currency\":\"GBP\",\"decimalPlaces\":2}},\"hasFlexiTickets\":false,\"shopperCurrency\":\"GBP\",\"shopperReference\":\"test\",\"reservations\":[{\"venueId\":\"139\",\"productId\":\"2017\",\"date\":\"2020-01-04T19:30:00+00:00\",\"quantity\":2,\"items\":[{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzEiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.T58JjzInDwXHCaytrA2eaAbmdi1wj1MkrVmiQvSm5co\"},{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOSIsInZjIjoiIiwicGkiOiIyMDE3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJQIiwiaXNuIjoiMzIiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTAxLTA0VDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDAxMDQiLCJwcnYiOjB9.5RWZjTbph1R-AXXq2e0qj4s-tepdXBbICEqMSXB35Do\"}]}],\"coupon\":null}"),
            new TestCaseData(
                new UpsertBasketParameters
                {
                    Reference = "1010101",
                    ChannelId = "encoretickets",
                    Delivery = new Delivery
                    {
                        Method = DeliveryMethod.Collection,
                        Charge = new Price
                        {
                            Value = 1000,
                            Currency = "GBP",
                            DecimalPlaces = 2,
                        },
                    },
                    HasFlexiTickets = true,
                    ShopperCurrency = "GBP",
                    ShopperReference = "5ec76ed81e39699b102d01a39fe74f1c",
                    Reservations = new List<ReservationParameters>
                    {
                        new ReservationParameters
                        {
                            VenueId = "163",
                            ProductId = "2102",
                            Date = new DateTimeOffset(2019, 04, 10, 19, 30, 00, TimeSpan.Zero),
                            Quantity = 1,
                            Items = new List<ReservationItemParameters>
                            {
                                new ReservationItemParameters
                                {
                                    AggregateReference =
                                        "eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==",
                                },
                            },
                        },
                    },
                    Coupon = new Coupon
                    {
                        Code = "SAMPLE_SOURCE_CODE",
                    },
                },
                "{\"reference\":\"1010101\",\"channelId\":\"encoretickets\",\"delivery\":{\"method\":\"collection\",\"charge\":{\"value\":1000,\"currency\":\"GBP\",\"decimalPlaces\":2}},\"hasFlexiTickets\":true,\"shopperCurrency\":\"GBP\",\"shopperReference\":\"5ec76ed81e39699b102d01a39fe74f1c\",\"reservations\":[{\"venueId\":\"163\",\"productId\":\"2102\",\"date\":\"2019-04-10T19:30:00+00:00\",\"quantity\":1,\"items\":[{\"aggregateReference\":\"eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==\"}]}],\"coupon\":{\"code\":\"SAMPLE_SOURCE_CODE\"}}"),
            new TestCaseData(
                new UpsertBasketParameters
                {
                    ChannelId = "{{affiliateId}}",
                    Reservations = new List<ReservationParameters>
                    {
                        new ReservationParameters
                        {
                            VenueId = "138",
                            ProductId = "1587",
                            Date = new DateTimeOffset(2020, 10, 23, 19, 30, 00, TimeSpan.Zero),
                            Quantity = 1,
                            Items = new List<ReservationItemParameters>
                            {
                                new ReservationItemParameters
                                {
                                    AggregateReference =
                                        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDU7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.-M7KQoFh1N7PKWestjbdbVR7EkwbsrVh9jwtsGMJh_k",
                                },
                            },
                        },
                    },
                },
                "{\"reference\":null,\"channelId\":\"{{affiliateId}}\",\"delivery\":null,\"hasFlexiTickets\":false,\"shopperCurrency\":null,\"shopperReference\":null,\"reservations\":[{\"venueId\":\"138\",\"productId\":\"1587\",\"date\":\"2020-10-23T19:30:00+00:00\",\"quantity\":1,\"items\":[{\"aggregateReference\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDU7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.-M7KQoFh1N7PKWestjbdbVR7EkwbsrVh9jwtsGMJh_k\"}]}],\"coupon\":null}"),
        };

        public static IEnumerable<TestCaseData> UpsertBasket_IfApiResponseSuccessful_ReturnsBasket { get; } = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\n  \""channelId\"":\""{{affiliateId}}\"",\n  \""reservations\"":[\n     {\n        \""venueId\"":\""138\"",\n        \""productId\"":\""1587\"",\n        \""date\"":\""2020-10-23T19:30:00+0000\"",\n        \""quantity\"":1,\n        \""items\"":[\n           {\n              \""aggregateReference\"":\""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDU7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.-M7KQoFh1N7PKWestjbdbVR7EkwbsrVh9jwtsGMJh_k\""\n           }\n        ]\n     }\n  ]\n}"",
        ""query"": {},
        ""urlParams"": {}
    },
    ""response"": {
        ""reference"": ""8604640"",
        ""checksum"": ""2006051235"",
        ""channelId"": ""{{affiliateId}}"",
        ""mixed"": false,
        ""exchangeRate"": 1.0,
        ""delivery"": null,
        ""allowFlexiTickets"": true,
        ""status"": ""active"",
        ""officeCurrency"": ""GBP"",
        ""shopperCurrency"": ""GBP"",
        ""expiredAt"": ""2020-06-05T11:50:49+0000"",
        ""createdAt"": ""2020-06-05T11:35:49+0000"",
        ""reservations"": [
            {
                ""id"": 1,
                ""linkedReservationId"": 0,
                ""venueId"": ""138"",
                ""productId"": ""1587"",
                ""productType"": ""SHW"",
                ""date"": ""2020-10-23T19:30:00+0100"",
                ""quantity"": 1,
                ""items"": [
                    {
                        ""aggregateReference"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiIiwicGkiOiIxNTg3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTEwLTIzVDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDA2MDUiLCJwcnYiOjB9.ge0efOx2fFWthyy11W_ACz2_BDcG6S9xAlRHxK0tu50"",
                        ""areaId"": ""DC"",
                        ""areaName"": ""CIRCLE"",
                        ""row"": ""X"",
                        ""number"": ""45"",
                        ""locationDescription"": """"
                    }
                ],
                ""faceValueInOfficeCurrency"": {
                    ""value"": 2500,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""faceValueInShopperCurrency"": {
                    ""value"": 2500,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""salePriceInOfficeCurrency"": {
                    ""value"": 3200,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""salePriceInShopperCurrency"": {
                    ""value"": 3200,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustedSalePriceInOfficeCurrency"": {
                    ""value"": 3200,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustedSalePriceInShopperCurrency"": {
                    ""value"": 3200,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustmentAmountInOfficeCurrency"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustmentAmountInShopperCurrency"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                }
            }
        ],
        ""coupon"": null,
        ""appliedPromotion"": null,
        ""missedPromotions"": null
    },
    ""context"": null
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "8604640",
                    Checksum = "2006051235",
                    ChannelId = "{{affiliateId}}",
                    Mixed = false,
                    ExchangeRate = 1,
                    Delivery = null,
                    AllowFlexiTickets = true,
                    Status = BasketStatus.Active,
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2020, 06, 05, 11, 50, 49, TimeSpan.Zero),
                    CreatedAt = new DateTimeOffset(2020, 06, 05, 11, 35, 49, TimeSpan.Zero),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 1,
                            LinkedReservationId = 0,
                            VenueId = "138",
                            ProductId = "1587",
                            ProductType = "SHW",
                            Date = new DateTimeOffset(2020, 10, 23, 19, 30, 00, TimeSpan.FromHours(1)),
                            Quantity = 1,
                            Items = new List<ReservationItem>
                            {
                                new ReservationItem
                                {
                                    AggregateReference =
                                        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiIiwicGkiOiIxNTg3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTEwLTIzVDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDA2MDUiLCJwcnYiOjB9.ge0efOx2fFWthyy11W_ACz2_BDcG6S9xAlRHxK0tu50",
                                    AreaId = "DC",
                                    AreaName = "CIRCLE",
                                    Row = "X",
                                    Number = "45",
                                    LocationDescription = "",
                                },
                            },
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 2500,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 2500,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 3200,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 3200,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 3200,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 3200,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                    },
                    Coupon = null,
                    AppliedPromotion = null,
                    MissedPromotions = null,
                }),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\n  \""channelId\"":\""encoretickets\"",\n  \""hasFlexiTickets\"": \""true\"",\n  \""reservations\"":[\n     {\n        \""venueId\"":\""138\"",\n        \""productId\"":\""1587\"",\n        \""date\"":\""2020-10-23T19:30:00+0000\"",\n        \""quantity\"":1,\n        \""items\"":[\n           {\n              \""aggregateReference\"":\""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDU7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.-M7KQoFh1N7PKWestjbdbVR7EkwbsrVh9jwtsGMJh_k\""\n           }\n        ]\n     }\n  ]\n}"",
        ""query"": {},
        ""urlParams"": {}
    },
    ""response"": {
        ""reference"": ""8605173"",
        ""checksum"": ""2006100949"",
        ""channelId"": ""encoretickets"",
        ""mixed"": false,
        ""exchangeRate"": 1.0,
        ""delivery"": null,
        ""allowFlexiTickets"": true,
        ""status"": ""active"",
        ""officeCurrency"": ""GBP"",
        ""shopperCurrency"": ""GBP"",
        ""expiredAt"": ""2020-06-10T09:04:44+0000"",
        ""createdAt"": ""2020-06-10T08:49:44+0000"",
        ""reservations"": [
            {
                ""id"": 1,
                ""linkedReservationId"": 0,
                ""venueId"": ""138"",
                ""productId"": ""1587"",
                ""productType"": ""SHW"",
                ""date"": ""2020-10-23T19:30:00+0100"",
                ""quantity"": 1,
                ""items"": [
                    {
                        ""aggregateReference"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiIiwicGkiOiIxNTg3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTEwLTIzVDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDA2MTAiLCJwcnYiOjB9.9EHyJHe5DMvpGnDZJliAh4hSmAlck42YLek3vI9YfLA"",
                        ""areaId"": ""DC"",
                        ""areaName"": ""CIRCLE"",
                        ""row"": ""X"",
                        ""number"": ""45"",
                        ""locationDescription"": """"
                    }
                ],
                ""faceValueInOfficeCurrency"": {
                    ""value"": 2500,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""faceValueInShopperCurrency"": {
                    ""value"": 2500,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""salePriceInOfficeCurrency"": {
                    ""value"": 3200,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""salePriceInShopperCurrency"": {
                    ""value"": 3200,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustedSalePriceInOfficeCurrency"": {
                    ""value"": 3200,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustedSalePriceInShopperCurrency"": {
                    ""value"": 3200,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustmentAmountInOfficeCurrency"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustmentAmountInShopperCurrency"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                }
            },
            {
                ""id"": 2,
                ""linkedReservationId"": 1,
                ""venueId"": ""901"",
                ""productId"": ""9001"",
                ""productType"": ""FLX"",
                ""date"": ""2020-10-23T00:01:00+0100"",
                ""quantity"": 1,
                ""faceValueInOfficeCurrency"": {
                    ""value"": 199,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""faceValueInShopperCurrency"": {
                    ""value"": 199,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""salePriceInOfficeCurrency"": {
                    ""value"": 199,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""salePriceInShopperCurrency"": {
                    ""value"": 199,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustedSalePriceInOfficeCurrency"": {
                    ""value"": 199,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustedSalePriceInShopperCurrency"": {
                    ""value"": 199,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustmentAmountInOfficeCurrency"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustmentAmountInShopperCurrency"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                }
            }
        ],
        ""coupon"": null,
        ""appliedPromotion"": null,
        ""missedPromotions"": null
    },
    ""context"": null
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "8605173",
                    Checksum = "2006100949",
                    ChannelId = "encoretickets",
                    Mixed = false,
                    ExchangeRate = 1,
                    Delivery = null,
                    AllowFlexiTickets = true,
                    Status = BasketStatus.Active,
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2020, 06, 10, 09, 04, 44, TimeSpan.Zero),
                    CreatedAt = new DateTimeOffset(2020, 06, 10, 08, 49, 44, TimeSpan.Zero),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 1,
                            LinkedReservationId = 0,
                            VenueId = "138",
                            ProductId = "1587",
                            ProductType = "SHW",
                            Date = new DateTimeOffset(2020, 10, 23, 19, 30, 00, TimeSpan.FromHours(1)),
                            Quantity = 1,
                            Items = new List<ReservationItem>
                            {
                                new ReservationItem
                                {
                                    AggregateReference =
                                        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiIiwicGkiOiIxNTg3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTEwLTIzVDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDA2MTAiLCJwcnYiOjB9.9EHyJHe5DMvpGnDZJliAh4hSmAlck42YLek3vI9YfLA",
                                    AreaId = "DC",
                                    AreaName = "CIRCLE",
                                    Row = "X",
                                    Number = "45",
                                    LocationDescription = "",
                                },
                            },
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 2500,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 2500,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 3200,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 3200,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 3200,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 3200,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                        new Reservation
                        {
                            Id = 2,
                            LinkedReservationId = 1,
                            VenueId = "901",
                            ProductId = "9001",
                            ProductType = "FLX",
                            Date = new DateTimeOffset(2020, 10, 23, 00, 01, 00, TimeSpan.FromHours(1)),
                            Quantity = 1,
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 199,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 199,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 199,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 199,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 199,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 199,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 0,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                    },
                    Coupon = null,
                    AppliedPromotion = null,
                    MissedPromotions = null,
                }),
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""string"",
    ""query"": {
      ""additionalProp1"": ""string"",
      ""additionalProp2"": ""string"",
      ""additionalProp3"": ""string""
    },
    ""urlParams"": {
      ""additionalProp1"": ""string"",
      ""additionalProp2"": ""string"",
      ""additionalProp3"": ""string""
    }
  },
  ""context"": {
  },
  ""response"": {
    ""reference"": ""1010101"",
    ""checksum"": ""1234567890"",
    ""channelId"": ""encoretickets"",
    ""exchangeRate"": ""1.2"",
    ""delivery"": {
      ""method"": ""collection"",
      ""charge"": {
        ""value"": 1000,
        ""currency"": ""GBP"",
        ""decimalPlaces"": 2
      }
    },
    ""allowFlexiTickets"": true,
    ""status"": ""active"",
    ""officeCurrency"": ""GBP"",
    ""shopperCurrency"": ""GBP"",
    ""expiredAt"": ""2019-04-01T14:15:00+02:00"",
    ""createdAt"": ""2019-04-01T14:00:00+02:00"",
    ""reservations"": [
      {
        ""id"": ""1"",
        ""linkedReservationId"": ""1"",
        ""venueId"": ""163"",
        ""productId"": ""2102"",
        ""productType"": ""SHW"",
        ""date"": ""2019-04-10T19:30:00+02:00"",
        ""quantity"": ""1"",
        ""items"": [
          {
            ""aggregateReference"": ""eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ=="",
            ""areaId"": ""ST"",
            ""areaName"": ""STALLS"",
            ""row"": ""G"",
            ""number"": ""14"",
            ""locationDescription"": ""Seat Location Description.""
          }
        ],
        ""faceValueInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""faceValueInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        }
      }
    ],
    ""coupon"": {
      ""code"": ""SAMPLE_SOURCE_CODE""
    },
    ""appliedPromotion"": {
      ""id"": ""1"",
      ""name"": ""Free ticket"",
      ""displayText"": ""Buy one ticket, get another free.""
    },
    ""missedPromotions"": [
      {
        ""id"": ""1"",
        ""name"": ""Free ticket"",
        ""displayText"": ""Buy one ticket, get another free.""
      }
    ]
  }
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "1010101",
                    Checksum = "1234567890",
                    ChannelId = "encoretickets",
                    ExchangeRate = 1.2M,
                    Delivery = new Delivery
                    {
                        Method = DeliveryMethod.Collection,
                        Charge = new Price
                        {
                            Value = 1000,
                            Currency = "GBP",
                            DecimalPlaces = 2,
                        },
                    },
                    AllowFlexiTickets = true,
                    Status = BasketStatus.Active,
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2019, 04, 01, 14, 15, 00, TimeSpan.FromHours(2)),
                    CreatedAt = new DateTimeOffset(2019, 04, 01, 14, 00, 00, TimeSpan.FromHours(2)),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 1,
                            LinkedReservationId = 1,
                            VenueId = "163",
                            ProductId = "2102",
                            ProductType = "SHW",
                            Date = new DateTimeOffset(2019, 04, 10, 19, 30, 00, TimeSpan.FromHours(2)),
                            Quantity = 1,
                            Items = new List<ReservationItem>
                            {
                                new ReservationItem
                                {
                                    AggregateReference =
                                        "eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==",
                                    AreaId = "ST",
                                    AreaName = "STALLS",
                                    Row = "G",
                                    Number = "14",
                                    LocationDescription = "Seat Location Description.",
                                },
                            },
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                    },
                    Coupon = new Coupon
                    {
                        Code = "SAMPLE_SOURCE_CODE",
                    },
                    AppliedPromotion = new Promotion
                    {
                        Id = "1",
                        Name = "Free ticket",
                        DisplayText = "Buy one ticket, get another free.",
                    },
                    MissedPromotions = new List<Promotion>
                    {
                        new Promotion
                        {
                            Id = "1",
                            Name = "Free ticket",
                            DisplayText = "Buy one ticket, get another free.",
                        },
                    },
                }),
        };

        public static IEnumerable<TestCaseData> UpsertBasket_IfApiResponseFailed_ThrowsApiException { get; } = new[]
        {
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\n  \""reservations\"":[\n     {\n        \""venueId\"":\""138\"",\n        \""productId\"":\""1587\"",\n        \""date\"":\""2020-10-23T19:30:00+0000\"",\n        \""quantity\"":1,\n        \""items\"":[\n           {\n              \""aggregateReference\"":\""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDU7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.-M7KQoFh1N7PKWestjbdbVR7EkwbsrVh9jwtsGMJh_k\""\n           }\n        ]\n     }\n  ]\n}"",
        ""query"": {},
        ""urlParams"": {}
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""field"": ""channelId"",
                ""message"": ""This value should not be blank."",
                ""code"": ""validation_error""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "channelId: This value should not be blank."),

            // 503
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\n  \""channelId\"":\""{{affiliateId}}\"",\n  \""reservations\"":[\n     {\n        \""venueId\"":\""1\"",\n        \""productId\"":\""1587\"",\n        \""date\"":\""2020-10-23T19:30:00+0000\"",\n        \""quantity\"":1,\n        \""items\"":[\n           {\n              \""aggregateReference\"":\""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiR0IiLCJwaSI6IjE1ODciLCJpaSI6IkNJUkNMRX5YNDU7NTAiLCJpYiI6IkRDIiwiaXIiOiJYIiwiaXNuIjoiNDUiLCJpc2xkIjoiQ2lyY2xlIiwiaXBpIjpudWxsLCJpZCI6IjIwMjAtMTAtMjNUMTk6MzA6MDArMDA6MDAiLCJlc2kiOiJJTlRFUk5BTCIsImVyaSI6bnVsbCwiZXNlaSI6bnVsbCwiZWJpIjpudWxsLCJlcGkiOm51bGwsImVkY3QiOm51bGwsInBhaSI6IjM1MzgiLCJjcHYiOjAsImNwYyI6IkdCUCIsIm9zcHYiOjMyMDAsIm9zcGMiOiJHQlAiLCJvZnZ2IjoyNTAwLCJvZnZjIjoiR0JQIiwic3NwdiI6MzIwMCwic3NwYyI6IkdCUCIsInNmdnYiOjI1MDAsInNmdmMiOiJHQlAiLCJvdHNzcGZyIjoxLCJzdG9zcGZyIjoxLCJpYyI6NCwicG1jIjpudWxsLCJyZWQiOiIxODU4MTExNyIsInBydiI6MH0.-M7KQoFh1N7PKWestjbdbVR7EkwbsrVh9jwtsGMJh_k\""\n           }\n        ]\n     }\n  ]\n}"",
        ""query"": {},
        ""urlParams"": {}
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Unable to process \""new\"" basket. Error code: \""THTNOTFOUND\"".""
            }
        ]
    }
}",
                HttpStatusCode.ServiceUnavailable,
                "Unable to process \"new\" basket. Error code: \"THTNOTFOUND\"."),
        };

        #endregion

        #region UpsertPromotion

        public static IEnumerable<TestCaseData> UpsertPromotion_IfBasketReferenceAndCouponAreSet_CallsApiWithRightParameters { get; } = new[]
        {
            new TestCaseData(
                "791631",
                null,
                "{\"coupon\":null}"),
            new TestCaseData(
                "791631",
                new Coupon
                {
                    Code = "PRODUCTDISCOUNT",
                },
                "{\"coupon\":{\"code\":\"PRODUCTDISCOUNT\"}}"),
        };

        public static IEnumerable<TestCaseData> UpsertPromotion_IfBasketReferenceAndCouponNameAreSet_CallsApiWithRightParameters { get; } = new[]
        {
            new TestCaseData(
                "791631",
                null,
                "{\"coupon\":{\"code\":null}}"),
            new TestCaseData(
                "791631",
                "PRODUCTDISCOUNT",
                "{\"coupon\":{\"code\":\"PRODUCTDISCOUNT\"}}"),
        };

        public static IEnumerable<TestCaseData> UpsertPromotion_IfApiResponseSuccessfulAndPromoCodeValid_ReturnsBasket { get; } = new[]
        {
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""string"",
    ""query"": {
      ""additionalProp1"": ""string"",
      ""additionalProp2"": ""string"",
      ""additionalProp3"": ""string""
    },
    ""urlParams"": {
      ""additionalProp1"": ""string"",
      ""additionalProp2"": ""string"",
      ""additionalProp3"": ""string""
    }
  },
  ""context"": {
  },
  ""response"": {
    ""reference"": ""1010101"",
    ""checksum"": ""1234567890"",
    ""channelId"": ""encoretickets"",
    ""exchangeRate"": ""1.2"",
    ""delivery"": {
      ""method"": ""collection"",
      ""charge"": {
        ""value"": 1000,
        ""currency"": ""GBP"",
        ""decimalPlaces"": 2
      }
    },
    ""allowFlexiTickets"": true,
    ""status"": ""active"",
    ""officeCurrency"": ""GBP"",
    ""shopperCurrency"": ""GBP"",
    ""expiredAt"": ""2019-04-01T14:15:00+02:00"",
    ""createdAt"": ""2019-04-01T14:00:00+02:00"",
    ""reservations"": [
      {
        ""id"": ""1"",
        ""linkedReservationId"": ""1"",
        ""venueId"": ""163"",
        ""productId"": ""2102"",
        ""productType"": ""SHW"",
        ""date"": ""2019-04-10T19:30:00+02:00"",
        ""quantity"": ""1"",
        ""items"": [
          {
            ""aggregateReference"": ""eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ=="",
            ""areaId"": ""ST"",
            ""areaName"": ""STALLS"",
            ""row"": ""G"",
            ""number"": ""14"",
            ""locationDescription"": ""Seat Location Description.""
          }
        ],
        ""faceValueInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""faceValueInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        }
      }
    ],
    ""coupon"": {
      ""code"": ""SAMPLE_SOURCE_CODE""
    },
    ""appliedPromotion"": {
      ""id"": ""1"",
      ""name"": ""Free ticket"",
      ""displayText"": ""Buy one ticket, get another free.""
    },
    ""missedPromotions"": [
      {
        ""id"": ""1"",
        ""name"": ""Free ticket"",
        ""displayText"": ""Buy one ticket, get another free.""
      }
    ]
  }
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "1010101",
                    Checksum = "1234567890",
                    ChannelId = "encoretickets",
                    ExchangeRate = 1.2M,
                    Delivery = new Delivery
                    {
                        Method = DeliveryMethod.Collection,
                        Charge = new Price
                        {
                            Value = 1000,
                            Currency = "GBP",
                            DecimalPlaces = 2,
                        },
                    },
                    AllowFlexiTickets = true,
                    Status = BasketStatus.Active,
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2019, 04, 01, 14, 15, 00, TimeSpan.FromHours(2)),
                    CreatedAt = new DateTimeOffset(2019, 04, 01, 14, 00, 00, TimeSpan.FromHours(2)),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 1,
                            LinkedReservationId = 1,
                            VenueId = "163",
                            ProductId = "2102",
                            ProductType = "SHW",
                            Date = new DateTimeOffset(2019, 04, 10, 19, 30, 00, TimeSpan.FromHours(2)),
                            Quantity = 1,
                            Items = new List<ReservationItem>
                            {
                                new ReservationItem
                                {
                                    AggregateReference =
                                        "eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==",
                                    AreaId = "ST",
                                    AreaName = "STALLS",
                                    Row = "G",
                                    Number = "14",
                                    LocationDescription = "Seat Location Description.",
                                },
                            },
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                    },
                    Coupon = new Coupon
                    {
                        Code = "SAMPLE_SOURCE_CODE",
                    },
                    AppliedPromotion = new Promotion
                    {
                        Id = "1",
                        Name = "Free ticket",
                        DisplayText = "Buy one ticket, get another free.",
                    },
                    MissedPromotions = new List<Promotion>
                    {
                        new Promotion
                        {
                            Id = "1",
                            Name = "Free ticket",
                            DisplayText = "Buy one ticket, get another free.",
                        },
                    },
                }),
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""string"",
    ""query"": {
      ""additionalProp1"": ""string"",
      ""additionalProp2"": ""string"",
      ""additionalProp3"": ""string""
    },
    ""urlParams"": {
      ""additionalProp1"": ""string"",
      ""additionalProp2"": ""string"",
      ""additionalProp3"": ""string""
    }
  },
  ""response"": {
    ""reference"": ""1010101"",
    ""checksum"": ""1234567890"",
    ""channelId"": ""encoretickets"",
    ""exchangeRate"": ""1.2"",
    ""delivery"": {
      ""method"": ""collection"",
      ""charge"": {
        ""value"": 1000,
        ""currency"": ""GBP"",
        ""decimalPlaces"": 2
      }
    },
    ""allowFlexiTickets"": true,
    ""status"": ""active"",
    ""officeCurrency"": ""GBP"",
    ""shopperCurrency"": ""GBP"",
    ""expiredAt"": ""2019-04-01T14:15:00+02:00"",
    ""createdAt"": ""2019-04-01T14:00:00+02:00"",
    ""reservations"": [
      {
        ""id"": ""1"",
        ""linkedReservationId"": ""1"",
        ""venueId"": ""163"",
        ""productId"": ""2102"",
        ""productType"": ""SHW"",
        ""date"": ""2019-04-10T19:30:00+02:00"",
        ""quantity"": ""1"",
        ""items"": [
          {
            ""aggregateReference"": ""eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ=="",
            ""areaId"": ""ST"",
            ""areaName"": ""STALLS"",
            ""row"": ""G"",
            ""number"": ""14"",
            ""locationDescription"": ""Seat Location Description.""
          }
        ],
        ""faceValueInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""faceValueInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        }
      }
    ],
    ""coupon"": {
      ""code"": ""SAMPLE_SOURCE_CODE""
    },
    ""appliedPromotion"": {
      ""id"": ""1"",
      ""name"": ""Free ticket"",
      ""displayText"": ""Buy one ticket, get another free.""
    },
    ""missedPromotions"": [
      {
        ""id"": ""1"",
        ""name"": ""Free ticket"",
        ""displayText"": ""Buy one ticket, get another free.""
      }
    ]
  },
    ""context"": {
        ""info"": [
            {
                ""code"": ""some_text"",
                ""type"": ""information"",
                ""name"": ""coupon"",
                ""message"": ""Example text: The supplied promotion code [test] was not applied as it didn't match a valid promotion code""
            }
        ]
    }
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "1010101",
                    Checksum = "1234567890",
                    ChannelId = "encoretickets",
                    ExchangeRate = 1.2M,
                    Delivery = new Delivery
                    {
                        Method = DeliveryMethod.Collection,
                        Charge = new Price
                        {
                            Value = 1000,
                            Currency = "GBP",
                            DecimalPlaces = 2,
                        },
                    },
                    AllowFlexiTickets = true,
                    Status = BasketStatus.Active,
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2019, 04, 01, 14, 15, 00, TimeSpan.FromHours(2)),
                    CreatedAt = new DateTimeOffset(2019, 04, 01, 14, 00, 00, TimeSpan.FromHours(2)),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 1,
                            LinkedReservationId = 1,
                            VenueId = "163",
                            ProductId = "2102",
                            ProductType = "SHW",
                            Date = new DateTimeOffset(2019, 04, 10, 19, 30, 00, TimeSpan.FromHours(2)),
                            Quantity = 1,
                            Items = new List<ReservationItem>
                            {
                                new ReservationItem
                                {
                                    AggregateReference =
                                        "eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==",
                                    AreaId = "ST",
                                    AreaName = "STALLS",
                                    Row = "G",
                                    Number = "14",
                                    LocationDescription = "Seat Location Description.",
                                },
                            },
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                    },
                    Coupon = new Coupon
                    {
                        Code = "SAMPLE_SOURCE_CODE",
                    },
                    AppliedPromotion = new Promotion
                    {
                        Id = "1",
                        Name = "Free ticket",
                        DisplayText = "Buy one ticket, get another free.",
                    },
                    MissedPromotions = new List<Promotion>
                    {
                        new Promotion
                        {
                            Id = "1",
                            Name = "Free ticket",
                            DisplayText = "Buy one ticket, get another free.",
                        },
                    },
                }),
        };

        public static IEnumerable<TestCaseData> UpsertPromotion_IfApiResponseSuccessfulButPromoCodeInvalid_ThrowsInvalidPromoCodeException { get; } = new[]
        {
            new TestCaseData(
                new Coupon
                {
                    Code = "test",
                },
                @"{
    ""request"": {
        ""body"": ""{\n  \""coupon\"": {\n    \""code\"": \""test\""\n  }\n}"",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""8605949""
        }
    },
    ""response"": {
        ""reference"": ""8605949"",
        ""checksum"": ""2006101722"",
        ""channelId"": ""encoretickets"",
        ""mixed"": false,
        ""exchangeRate"": 1.0,
        ""delivery"": null,
        ""allowFlexiTickets"": true,
        ""status"": ""active"",
        ""officeCurrency"": ""GBP"",
        ""shopperCurrency"": ""GBP"",
        ""expiredAt"": ""2020-06-10T16:37:58+0000"",
        ""createdAt"": ""2020-06-10T16:22:58+0000"",
        ""reservations"": [
            {
                ""id"": 1,
                ""linkedReservationId"": 0,
                ""venueId"": ""138"",
                ""productId"": ""1587"",
                ""productType"": ""SHW"",
                ""date"": ""2020-10-23T19:30:00+0100"",
                ""quantity"": 1,
                ""items"": [
                    {
                        ""aggregateReference"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2aSI6IjEzOCIsInZjIjoiIiwicGkiOiIxNTg3IiwiaWkiOiIiLCJpYiI6IkRDIiwiaXIiOiJIIiwiaXNuIjoiNDMiLCJpc2xkIjoiIiwiaXBpIjoiIiwiaWQiOiIyMDIwLTEwLTIzVDE5OjMwOjAwKzAwOjAwIiwiZXNpIjoiIiwiZXJpIjoiIiwiZXNlaSI6IiIsImViaSI6IiIsImVwaSI6IiIsImVkY3QiOiIiLCJwYWkiOiIiLCJjcHYiOjAsImNwYyI6IiIsIm9zcHYiOjAsIm9zcGMiOiIiLCJvZnZ2IjowLCJvZnZjIjoiIiwic3NwdiI6MCwic3NwYyI6IiIsInNmdnYiOjAsInNmdmMiOiIiLCJvdHNzcGZyIjowLCJzdG9zcGZyIjowLCJpYyI6MCwicG1jIjoiIiwicmVkIjoiMjAyMDA2MTAiLCJwcnYiOjB9.CxiDEFy_x5tOtb3-K1jmAGVot-mkA8PCDIcfVGo3ukc"",
                        ""areaId"": ""DC"",
                        ""areaName"": ""CIRCLE"",
                        ""row"": ""H"",
                        ""number"": ""43"",
                        ""locationDescription"": """"
                    }
                ],
                ""faceValueInOfficeCurrency"": {
                    ""value"": 6500,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""faceValueInShopperCurrency"": {
                    ""value"": 6500,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""salePriceInOfficeCurrency"": {
                    ""value"": 8400,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""salePriceInShopperCurrency"": {
                    ""value"": 8400,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustedSalePriceInOfficeCurrency"": {
                    ""value"": 8400,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustedSalePriceInShopperCurrency"": {
                    ""value"": 8400,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustmentAmountInOfficeCurrency"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                },
                ""adjustmentAmountInShopperCurrency"": {
                    ""value"": 0,
                    ""currency"": ""GBP"",
                    ""decimalPlaces"": 2
                }
            }
        ],
        ""coupon"": null,
        ""appliedPromotion"": null,
        ""missedPromotions"": null
    },
    ""context"": {
        ""info"": [
            {
                ""code"": ""notValidPromotionCode"",
                ""type"": ""information"",
                ""name"": ""coupon"",
                ""message"": ""The supplied promotion code [test] was not applied as it didn't match a valid promotion code""
            }
        ]
    }
}",
                "The supplied promotion code [test] was not applied as it didn't match a valid promotion code"),
        };

        public static IEnumerable<TestCaseData> UpsertPromotion_IfApiResponseFailedWith400Code_ThrowsBasketCannotBeModifiedException { get; } = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\n  \""coupon\"": {\n    \""code\"": \""sfhshd\""\n  }\n}"",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""8605916""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""The basket with reference [8605916] cannot be modified. It has a status of [cancelled]. Only baskets in [active] status can be updated or confirmed.""
            }
        ]
    }
}",
                "The basket with reference [8605916] cannot be modified. It has a status of [cancelled]. Only baskets in [active] status can be updated or confirmed."),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\n  \""coupon\"": {\n    \""code\"": \""sfhshd\""\n  }\n}"",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""test""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Insufficient data has been supplied for \""test\"" basket to complete this request.""
            }
        ]
    }
}",
                "Insufficient data has been supplied for \"test\" basket to complete this request."),
        };

        public static IEnumerable<TestCaseData> UpsertPromotion_IfApiResponseFailedWith404Code_ThrowsBasketNotFoundException { get; } = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\n  \""coupon\"": {\n    \""code\"": \""sfhshd\""\n  }\n}"",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""34343434""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Basket with reference \""34343434\"" was not found.""
            }
        ]
    }
}",
                "Basket with reference \"34343434\" was not found."),
        };

        public static IEnumerable<TestCaseData> UpsertPromotion_IfApiResponseFailedWithUnexpectedCode_ThrowsApiException { get; } = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\n  \""coupon\"": {\n    \""code\"": \""sfhshd\""\n  }\n}"",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""8605916""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""test""
            }
        ]
    }
}",
                HttpStatusCode.ServiceUnavailable,
                "test"),
        };

        #endregion

        #region ClearBasket

        public static IEnumerable<TestCaseData> ClearBasket_IfApiResponseSuccessful_ReturnsBasket { get; } = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""8604754""
        }
    },
    ""response"": {
        ""reference"": ""8604754"",
        ""checksum"": ""2006081529"",
        ""channelId"": ""encoretickets"",
        ""mixed"": false,
        ""exchangeRate"": 1.0,
        ""delivery"": null,
        ""allowFlexiTickets"": false,
        ""status"": ""active"",
        ""officeCurrency"": ""GBP"",
        ""shopperCurrency"": ""GBP"",
        ""expiredAt"": ""2020-06-08T14:44:19+0000"",
        ""createdAt"": ""2020-06-08T14:29:19+0000"",
        ""reservations"": [],
        ""coupon"": null,
        ""appliedPromotion"": null,
        ""missedPromotions"": null
    },
    ""context"": null
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "8604754",
                    Checksum = "2006081529",
                    ChannelId = "encoretickets",
                    Mixed = false,
                    ExchangeRate = 1,
                    Delivery = null,
                    AllowFlexiTickets = false,
                    Status = BasketStatus.Active,
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2020, 06, 08, 14, 44, 19, TimeSpan.Zero),
                    CreatedAt = new DateTimeOffset(2020, 06, 08, 14, 29, 19, TimeSpan.Zero),
                    Reservations = new List<Reservation>(),
                    Coupon = null,
                    AppliedPromotion = null,
                    MissedPromotions = null,
                }),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""8604754""
        }
    },
    ""response"": {
        ""reference"": ""8604754"",
        ""checksum"": ""2006081529"",
        ""channelId"": ""encoretickets"",
        ""mixed"": false,
        ""exchangeRate"": 1.0,
        ""delivery"": null,
        ""allowFlexiTickets"": false,
        ""status"": ""active"",
        ""officeCurrency"": ""GBP"",
        ""shopperCurrency"": ""GBP"",
        ""expiredAt"": ""2020-06-08T14:44:19+0000"",
        ""createdAt"": ""2020-06-08T14:29:19+0000"",
        ""reservations"": [],
        ""coupon"": null,
        ""appliedPromotion"": null,
        ""missedPromotions"": null
    },
    ""context"": null
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "8604754",
                    Checksum = "2006081529",
                    ChannelId = "encoretickets",
                    Mixed = false,
                    ExchangeRate = 1,
                    Delivery = null,
                    AllowFlexiTickets = false,
                    Status = BasketStatus.Active,
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2020, 06, 08, 14, 44, 19, TimeSpan.Zero),
                    CreatedAt = new DateTimeOffset(2020, 06, 08, 14, 29, 19, TimeSpan.Zero),
                    Reservations = new List<Reservation>(),
                    Coupon = null,
                    AppliedPromotion = null,
                    MissedPromotions = null,
                }),
        };

        public static IEnumerable<TestCaseData> ClearBasket_IfApiResponseFailed_ThrowsApiException { get; } = new[]
        {
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""test""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Insufficient data has been supplied for \""test\"" basket to complete this request.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Insufficient data has been supplied for \"test\" basket to complete this request."),

            // 404
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""5926058""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Basket with reference \""5926058\"" was not found.""
            }
        ]
    }
}",
                HttpStatusCode.NotFound,
                "Basket with reference \"5926058\" was not found."),
        };

        #endregion

        #region RemoveReservation

        public static IEnumerable<TestCaseData> RemoveReservation_IfApiResponseSuccessful_ReturnsBasket { get; } = new[]
        {
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""string"",
    ""query"": {
      ""additionalProp1"": ""string"",
      ""additionalProp2"": ""string"",
      ""additionalProp3"": ""string""
    },
    ""urlParams"": {
      ""additionalProp1"": ""string"",
      ""additionalProp2"": ""string"",
      ""additionalProp3"": ""string""
    }
  },
  ""context"": {
  },
  ""response"": {
    ""reference"": ""1010101"",
    ""checksum"": ""1234567890"",
    ""channelId"": ""encoretickets"",
    ""exchangeRate"": ""1.2"",
    ""delivery"": {
      ""method"": ""collection"",
      ""charge"": {
        ""value"": 1000,
        ""currency"": ""GBP"",
        ""decimalPlaces"": 2
      }
    },
    ""allowFlexiTickets"": true,
    ""status"": ""active"",
    ""officeCurrency"": ""GBP"",
    ""shopperCurrency"": ""GBP"",
    ""expiredAt"": ""2019-04-01T14:15:00+02:00"",
    ""createdAt"": ""2019-04-01T14:00:00+02:00"",
    ""reservations"": [
      {
        ""id"": ""1"",
        ""linkedReservationId"": ""1"",
        ""venueId"": ""163"",
        ""productId"": ""2102"",
        ""productType"": ""SHW"",
        ""date"": ""2019-04-10T19:30:00+02:00"",
        ""quantity"": ""1"",
        ""items"": [
          {
            ""aggregateReference"": ""eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ=="",
            ""areaId"": ""ST"",
            ""areaName"": ""STALLS"",
            ""row"": ""G"",
            ""number"": ""14"",
            ""locationDescription"": ""Seat Location Description.""
          }
        ],
        ""faceValueInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""faceValueInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""salePriceInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustedSalePriceInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInOfficeCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        },
        ""adjustmentAmountInShopperCurrency"": {
          ""value"": 1000,
          ""currency"": ""GBP"",
          ""decimalPlaces"": 2
        }
      }
    ],
    ""coupon"": {
      ""code"": ""SAMPLE_SOURCE_CODE""
    },
    ""appliedPromotion"": {
      ""id"": ""1"",
      ""name"": ""Free ticket"",
      ""displayText"": ""Buy one ticket, get another free.""
    },
    ""missedPromotions"": [
      {
        ""id"": ""1"",
        ""name"": ""Free ticket"",
        ""displayText"": ""Buy one ticket, get another free.""
      }
    ]
  }
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "1010101",
                    Checksum = "1234567890",
                    ChannelId = "encoretickets",
                    ExchangeRate = 1.2M,
                    Delivery = new Delivery
                    {
                        Method = DeliveryMethod.Collection,
                        Charge = new Price
                        {
                            Value = 1000,
                            Currency = "GBP",
                            DecimalPlaces = 2,
                        },
                    },
                    AllowFlexiTickets = true,
                    Status = BasketStatus.Active,
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2019, 04, 01, 14, 15, 00, TimeSpan.FromHours(2)),
                    CreatedAt = new DateTimeOffset(2019, 04, 01, 14, 00, 00, TimeSpan.FromHours(2)),
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            Id = 1,
                            LinkedReservationId = 1,
                            VenueId = "163",
                            ProductId = "2102",
                            ProductType = "SHW",
                            Date = new DateTimeOffset(2019, 04, 10, 19, 30, 00, TimeSpan.FromHours(2)),
                            Quantity = 1,
                            Items = new List<ReservationItem>
                            {
                                new ReservationItem
                                {
                                    AggregateReference =
                                        "eyJzYm9BbW91bnQiOjY5MDAsInNib1ByaWNlIjo2OTAwLCJob3VzZVByaWNlIjo2OTAwLCJzdGFDb3N0Ijo2OTAwfQ==",
                                    AreaId = "ST",
                                    AreaName = "STALLS",
                                    Row = "G",
                                    Number = "14",
                                    LocationDescription = "Seat Location Description.",
                                },
                            },
                            FaceValueInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            FaceValueInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            SalePriceInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustedSalePriceInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInOfficeCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                            AdjustmentAmountInShopperCurrency = new Price
                            {
                                Value = 1000,
                                Currency = "GBP",
                                DecimalPlaces = 2,
                            },
                        },
                    },
                    Coupon = new Coupon
                    {
                        Code = "SAMPLE_SOURCE_CODE",
                    },
                    AppliedPromotion = new Promotion
                    {
                        Id = "1",
                        Name = "Free ticket",
                        DisplayText = "Buy one ticket, get another free.",
                    },
                    MissedPromotions = new List<Promotion>
                    {
                        new Promotion
                        {
                            Id = "1",
                            Name = "Free ticket",
                            DisplayText = "Buy one ticket, get another free.",
                        },
                    },
                }),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""8604865"",
            ""reservationId"": ""1""
        }
    },
    ""response"": {
        ""reference"": ""8604865"",
        ""checksum"": ""2006091514"",
        ""channelId"": ""encoretickets"",
        ""mixed"": false,
        ""exchangeRate"": 1.0,
        ""delivery"": null,
        ""allowFlexiTickets"": false,
        ""status"": ""active"",
        ""officeCurrency"": ""GBP"",
        ""shopperCurrency"": ""GBP"",
        ""expiredAt"": ""2020-06-09T14:29:35+0000"",
        ""createdAt"": ""2020-06-09T14:14:35+0000"",
        ""reservations"": [],
        ""coupon"": null,
        ""appliedPromotion"": null,
        ""missedPromotions"": null
    },
    ""context"": null
}",
                new SDK.Basket.Models.Basket
                {
                    Reference = "8604865",
                    Checksum = "2006091514",
                    ChannelId = "encoretickets",
                    Mixed = false,
                    ExchangeRate = 1,
                    Delivery = null,
                    AllowFlexiTickets = false,
                    Status = BasketStatus.Active,
                    OfficeCurrency = "GBP",
                    ShopperCurrency = "GBP",
                    ExpiredAt = new DateTimeOffset(2020, 06, 09, 14, 29, 35, TimeSpan.Zero),
                    CreatedAt = new DateTimeOffset(2020, 06, 09, 14, 14, 35, TimeSpan.Zero),
                    Reservations = new List<Reservation>(),
                    Coupon = null,
                    AppliedPromotion = null,
                    MissedPromotions = null,
                }),
        };

        public static IEnumerable<TestCaseData> RemoveReservation_IfApiResponseFailed_ThrowsApiException { get; } = new[]
            {
                // 404
                new TestCaseData(
                    @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""8604865"",
            ""reservationId"": ""2""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Reservation with id [2] does not exist for basket [8604865].""
            }
        ]
    }
}",
                    HttpStatusCode.NotFound,
                    "Reservation with id [2] does not exist for basket [8604865]."),
                new TestCaseData(
                    @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""reference"": ""04865"",
            ""reservationId"": ""1""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Basket with reference \""04865\"" was not found.""
            }
        ]
    }
}",
                    HttpStatusCode.NotFound,
                    "Basket with reference \"04865\" was not found."),
            };

        #endregion

        #region GetPromotions

        public static IEnumerable<TestCaseData> GetPromotions_CallsApiWithRightParameters { get; } = new[]
        {
            new TestCaseData(
                new PageRequest
                {
                    Limit = 500,
                    Page = 2,
                }),
            new TestCaseData(
                null),
            new TestCaseData(
                new PageRequest
                {
                    Limit = 500,
                }),
        };

        public static IEnumerable<TestCaseData> GetPromotions_IfApiResponseSuccessful_ReturnsPromotions { get; } = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""limit"": ""2"",
            ""page"": ""1""
        },
        ""urlParams"": {}
    },
    ""response"": {
        ""results"": [
            {
                ""id"": ""206000019"",
                ""name"": ""Test promo"",
                ""reference"": ""TESTPROMO"",
                ""reportingCode"": ""ReportingCode001"",
                ""validFrom"": ""2019-08-07T08:37:49+0000"",
                ""validTo"": ""2019-08-11T08:35:42+0000""
            },
            {
                ""id"": ""206000013"",
                ""name"": ""Black Friday - 10% off (23-05-2019)"",
                ""reference"": ""2019-BLACKFRIDAY-10"",
                ""reportingCode"": """",
                ""validFrom"": ""2019-05-28T07:00:00+0000"",
                ""validTo"": ""2019-10-30T22:59:59+0000""
            }
        ]
    },
    ""context"": null
}",
                new List<Promotion>
                {
                    new Promotion
                    {
                        Id = "206000019",
                        Name = "Test promo",
                        Reference = "TESTPROMO",
                        ReportingCode = "ReportingCode001",
                        ValidFrom = new DateTimeOffset(2019, 08, 07, 08, 37, 49, TimeSpan.Zero),
                        ValidTo = new DateTimeOffset(2019, 08, 11, 08, 35, 42, TimeSpan.Zero),
                    },
                    new Promotion
                    {
                        Id = "206000013",
                        Name = "Black Friday - 10% off (23-05-2019)",
                        Reference = "2019-BLACKFRIDAY-10",
                        ReportingCode = "",
                        ValidFrom = new DateTimeOffset(2019, 05, 28, 07, 00, 00, TimeSpan.Zero),
                        ValidTo = new DateTimeOffset(2019, 10, 30, 22, 59, 59, TimeSpan.Zero),
                    },
                }),
        };

        public static IEnumerable<TestCaseData> GetPromotions_IfApiResponseFailed_ThrowsApiException { get; } = new[]
        {
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {
            ""limit"": ""2"",
            ""page"": ""-1""
        },
        ""urlParams"": {}
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Parameter \""page\"" of value \""-1\"" violated a constraint \""must be an integer\""""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Parameter \"page\" of value \"-1\" violated a constraint \"must be an integer\""),
        };

        #endregion

        #region GetPromotionDetails

        public static IEnumerable<TestCaseData> GetPromotionDetails_IfApiResponseSuccessful_ReturnsPromotion { get; } =
            new[]
            {
                new TestCaseData(
                    @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""promotionId"": ""206000054""
        }
    },
    ""response"": {
        ""id"": ""206000054"",
        ""name"": ""Product % or value_2"",
        ""displayText"": """",
        ""description"": """",
        ""reference"": """",
        ""reportingCode"": """",
        ""validFrom"": ""2019-11-01T07:47:43+0000"",
        ""validTo"": ""2019-11-20T07:45:28+0000""
    },
    ""context"": null
}",
                    new Promotion
                    {
                        Id = "206000054",
                        Name = "Product % or value_2",
                        DisplayText = "",
                        Description = "",
                        Reference = "",
                        ReportingCode = "",
                        ValidFrom = new DateTimeOffset(2019, 11, 01, 07, 47, 43, TimeSpan.Zero),
                        ValidTo = new DateTimeOffset(2019, 11, 20, 07, 45, 28, TimeSpan.Zero),
                    }),
                new TestCaseData(
                    @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""promotionId"": ""206000019""
        }
    },
    ""response"": {
        ""id"": ""206000019"",
        ""name"": ""Test promo"",
        ""displayText"": ""This is test promotion text for end-user"",
        ""description"": ""This is test promotion"",
        ""reference"": ""TESTPROMO"",
        ""reportingCode"": ""ReportingCode001"",
        ""validFrom"": ""2019-08-07T08:37:49+0000"",
        ""validTo"": ""2019-08-11T08:35:42+0000""
    },
    ""context"": null
}",
                    new Promotion
                    {
                        Id = "206000019",
                        Name = "Test promo",
                        DisplayText = "This is test promotion text for end-user",
                        Description = "This is test promotion",
                        Reference = "TESTPROMO",
                        ReportingCode = "ReportingCode001",
                        ValidFrom = new DateTimeOffset(2019, 08, 07, 08, 37, 49, TimeSpan.Zero),
                        ValidTo = new DateTimeOffset(2019, 08, 11, 08, 35, 42, TimeSpan.Zero),
                    }),
            };

        public static IEnumerable<TestCaseData> GetPromotionDetails_IfApiResponseFailed_ThrowsApiException { get; } = new[]
        {
            // 404
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": """",
        ""query"": {},
        ""urlParams"": {
            ""promotionId"": ""2060""
        }
    },
    ""response"": """",
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Promotion with id [2060] does not exist.""
            }
        ]
    }
}",
                HttpStatusCode.NotFound,
                "Promotion with id [2060] does not exist."),
        };

        #endregion
    }
}
