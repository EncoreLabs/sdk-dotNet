using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Basket;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using EncoreTickets.SDK.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    internal class BasketServiceForTodayTixTests
    {
        private IConfiguration configuration;
        private BasketServiceApiForTodayTix service;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            var context = new ApiContext(Environments.QA);
            service = new BasketServiceApiForTodayTix(context);
        }

        #region GetBasketDetails

        [Test]
        public void GetBasketDetails_Successful()
        {
            var reference = configuration["Basket:TestBasketReference"];

            var result = service.GetBasketDetails(reference);

            Assert.NotNull(result);
            Assert.AreEqual(reference, result.Reference);
            Assert.NotNull(result.ChannelId);
        }

        [Test]
        public void GetBasketDetails_IfBasketReferenceIsInvalid_Exception404()
        {
            var reference = "invalid";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.GetBasketDetails(reference);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetBasketDetails_IfBasketNotFound_Exception404()
        {
            var reference = configuration["Basket:TestBasketReferenceNotFound"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.GetBasketDetails(reference);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        #endregion

        #region GetBasketDeliveryOptions

        [Test]
        public void GetBasketDeliveryOptions_Successful()
        {
            var reference = configuration["Basket:TestBasketReference"];

            var result = service.GetBasketDeliveryOptions(reference);

            Assert.NotNull(result);
            foreach (var deliveryOption in result)
            {
                Assert.NotNull(deliveryOption.Charge);
                Assert.NotNull(deliveryOption.Method);
            }
        }

        [Test]
        public void GetBasketDeliveryOptions_IfBasketReferenceIsInvalid_Exception404()
        {
            var reference = "invalid";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.GetBasketDetails(reference);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetBasketDeliveryOptions_IfBasketNotFound_Exception404()
        {
            var reference = configuration["Basket:TestBasketReferenceNotFound"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.GetBasketDetails(reference);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        #endregion

        #region UpsertBasket

        [Test]
        public void UpsertBasket_GetBasket_IfNewBasketAndBasketIsUsedAndWithoutFlexi_Successful()
        {
            var upsertBasketResult = (Basket.Models.Basket)null;
            try
            {
                var reference = configuration["Basket:TestReferences:0"];
                var basketInRequest = CreateDefaultBasket(reference);
                upsertBasketResult = service.UpsertBasket(basketInRequest);
                const int expectedReservationsCount = 1;

                var basketDetails = service.GetBasketDetails(upsertBasketResult.Reference);

                AssertUpsertBasketSuccess(basketInRequest, basketDetails, expectedReservationsCount);
                Assert.AreEqual(upsertBasketResult.Reference, basketDetails.Reference);
            }
            finally
            {
                service.ClearBasket(upsertBasketResult?.Reference);
            }
        }

        [Test]
        public void UpsertBasket_GetBasket_IfNewBasketAndBasketIsUsedAndWithFlexi_Successful()
        {
            var upsertBasketResult = (Basket.Models.Basket)null;
            try
            {
                var reference = configuration["Basket:TestReferences:0"];
                var request = CreateDefaultBasket(reference);
                request.AllowFlexiTickets = true;
                upsertBasketResult = service.UpsertBasket(request);
                const int expectedReservationsCount = 1;

                var basketDetails = service.GetBasketDetails(upsertBasketResult.Reference);

                AssertUpsertBasketSuccess(request, basketDetails, expectedReservationsCount);
                Assert.AreEqual(upsertBasketResult.Reference, basketDetails.Reference);
            }
            finally
            {
                service.ClearBasket(upsertBasketResult?.Reference);
            }
        }

        [Test]
        public void UpsertBasket_GetBasket_IfNewBasketAndReservationItemsMoreThan1_Successful()
        {
            var upsertBasketResult = (Basket.Models.Basket)null;
            try
            {
                var referenceItem1 = configuration["Basket:TestReferences:0"];
                var referenceItem2 = configuration["Basket:TestReferences:1"];
                var request = CreateDefaultBasket(referenceItem1, referenceItem2);
                upsertBasketResult = service.UpsertBasket(request);
                const int expectedReservationsCount = 1;

                var basketDetails = service.GetBasketDetails(upsertBasketResult.Reference);

                AssertUpsertBasketSuccess(request, basketDetails, expectedReservationsCount);
                Assert.AreEqual(upsertBasketResult.Reference, basketDetails.Reference);
            }
            finally
            {
                service.ClearBasket(upsertBasketResult?.Reference);
            }
        }

        // todo: add additional tests when the basket API v2 is fully ready

        [Test]
        public void UpsertBasket_IfReservationsMoreThan1_Exception400()
        {
            var referenceItem1 = configuration["Basket:TestReferences:0"];
            var referenceItem2 = configuration["Basket:TestReferences:1"];
            var request = CreateDefaultBasket(referenceItem1);
            var reservation2 = request.Reservations[0].CopyObject();
            reservation2.Items[0].AggregateReference = referenceItem2;
            request.Reservations.Add(reservation2);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var upsertBasketResult = service.UpsertBasket(request);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpsertBasket_IfReservationItemIsNotAvailable_Exception400()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            var upsertBasketResult = service.UpsertBasket(request);

            try
            {
                var exception = Assert.Catch<ApiException>(() =>
                {
                    service.UpsertBasket(request);
                });

                AssertApiException(exception, HttpStatusCode.BadRequest);
            }
            finally
            {
                service.ClearBasket(upsertBasketResult?.Reference);
            }
        }

        [Test]
        public void UpsertBasket_IfReservationItemHasInvalidReference_Exception400()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            request.Reservations[0].Items[0].AggregateReference = "invalid";

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.UpsertBasket(request);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpsertBasket_IfBasketChannelIsMissed_Exception400()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            request.ChannelId = null;

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.UpsertBasket(request);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpsertBasket_IfReservationsAreMissed_Exception400()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            request.Reservations = null;

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.UpsertBasket(request);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpsertBasket_IfCountOfReservationsItemsIsNotEqualToQuantity_Exception400()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            request.Reservations[0].Quantity = request.Reservations[0].Items.Count + 1;

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.UpsertBasket(request);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        #endregion

        #region ClearBasket

        [Test]
        public void ClearBasket_Successful()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            var upsertBasketResult = service.UpsertBasket(request);

            var clearBasketResult = service.ClearBasket(upsertBasketResult.Reference);

            Assert.IsEmpty(clearBasketResult.Reservations);
        }

        [Test]
        public void ClearBasket_IfBasketReferenceIsInvalid_Exception404()
        {
            var reference = "_";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.ClearBasket(reference);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        [Test]
        public void ClearBasket_IfBasketNotFound_Exception404()
        {
            var reference = configuration["Basket:TestBasketReferenceNotFound"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.ClearBasket(reference);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        #endregion

        #region RemoveReservation

        [Test]
        public void RemoveReservation_Successful()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            var upsertBasketResult = service.UpsertBasket(request);

            var removeReservationResult =
                service.RemoveReservation(upsertBasketResult.Reference, upsertBasketResult.Reservations[0].Id);

            Assert.IsEmpty(removeReservationResult.Reservations);
        }

        [Test]
        public void RemoveReservation_IfBasketReferenceIsInvalid_Exception404()
        {
            var basketReference = "test";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var removeReservationResult = service.RemoveReservation(basketReference, 1);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        [Test]
        public void RemoveReservation_IfBasketNotFound_Exception404()
        {
            var reference = configuration["Basket:TestBasketReferenceNotFound"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var removeReservationResult = service.RemoveReservation(reference, 1);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        [Test]
        public void RemoveReservation_IfReservationNotFound_Exception404()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            var upsertBasketResult = service.UpsertBasket(request);
            var reservationId = 1;

            var exception = Assert.Catch<ApiException>(() =>
            {
                var removeReservationResult = service.RemoveReservation(upsertBasketResult.Reference, reservationId);
            });

            try
            {
                service.ClearBasket(upsertBasketResult.Reference);
            }
            catch (Exception)
            {
            }

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        #endregion

        #region GetPromotions

        [Test]
        public void GetPromotions_Successful()
        {
            var pageParameters = new PageRequest
            {
                Limit = 10,
            };

            var promotions = service.GetPromotions(pageParameters);

            foreach (var promoDetails in promotions)
            {
                Assert.NotNull(promoDetails.Id);
                Assert.NotNull(promoDetails.Name);
                Assert.NotNull(promoDetails.Reference);
                Assert.NotNull(promoDetails.ReportingCode);
                Assert.AreNotEqual(promoDetails.ValidFrom, default);
                Assert.AreNotEqual(promoDetails.ValidTo, default);
            }
        }

        [Test]
        public void GetPromotions_IfParametersAreIncorrect_Exception400()
        {
            var pageParameters = new PageRequest
            {
                Limit = -10,
            };

            var exception = Assert.Catch<ApiException>(() =>
            {
                var promotions = service.GetPromotions(pageParameters);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        #endregion

        #region GetPromotionDetails

        [Test]
        public void GetPromotionDetails_Successful()
        {
            const string promoId = "206000034";

            var promoDetails = service.GetPromotionDetails(promoId);

            Assert.AreEqual(promoId, promoDetails.Id);
            Assert.NotNull(promoDetails.Reference);
            Assert.NotNull(promoDetails.Description);
            Assert.NotNull(promoDetails.DisplayText);
            Assert.NotNull(promoDetails.Name);
            Assert.NotNull(promoDetails.ReportingCode);
            Assert.AreNotEqual(promoDetails.ValidFrom, default);
            Assert.AreNotEqual(promoDetails.ValidTo, default);
        }

        [Test]
        public void GetPromotionDetails_IfIdIsIncorrect_Exception404()
        {
            var id = "not_id";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var promotions = service.GetPromotionDetails(id);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        #endregion

        private bool VerifyPromoCodeTestsEnabled()
        {
            if (GetConfigBoolValueOrFalse("RunPromoCodeTests"))
            {
                return true;
            }

            if (GetConfigBoolValueOrFalse("ShowPromoCodeWarning"))
            {
                Assert.Warn("The promo code involving tests are disabled by default because they use a paid service. " +
                            "Set 'RunPromoCodeTests' configuration value to true to run the tests with promo codes.");
            }

            return false;
        }

        private UpsertBasketParameters CreateDefaultBasketParameters(params string[] references)
        {
            var codesEnabled = VerifyPromoCodeTestsEnabled();
            return new UpsertBasketParameters
            {
                ChannelId = "test-channel",
                Coupon = codesEnabled ? new Coupon { Code = configuration["Basket:ValidPromoCode"] } : null,
                Delivery = CreateDefaultDelivery(),
                Reservations = new List<ReservationParameters>
                {
                    new ReservationParameters
                    {
                        Date = DateTimeOffset.ParseExact(
                            configuration["Basket:TestDate"],
                            "yyyy-MM-ddTHH:mm",
                            CultureInfo.InvariantCulture),
                        ProductId = configuration["Basket:TestProductId"],
                        VenueId = configuration["Basket:TestVenueId"],
                        Quantity = references.Length,
                        Items = references.Select(r => new ReservationItemParameters { AggregateReference = r }).ToList(),
                    },
                },
            };
        }

        private Basket.Models.Basket CreateDefaultBasket(params string[] references)
        {
            var codesEnabled = VerifyPromoCodeTestsEnabled();
            return new Basket.Models.Basket
            {
                ChannelId = "boxoffice",
                Coupon = codesEnabled ? new Coupon { Code = configuration["Basket:ValidPromoCode"] } : null,
                Delivery = CreateDefaultDelivery(),
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        Date = DateTimeOffset.ParseExact(
                            configuration["Basket:TestDate"],
                            "yyyy-MM-ddTHH:mm",
                            CultureInfo.InvariantCulture),
                        ProductId = configuration["Basket:TestProductId"],
                        VenueId = configuration["Basket:TestVenueId"],
                        Quantity = references.Length,
                        Items = references.Select(r => new ReservationItem { AggregateReference = r }).ToList(),
                    },
                },
            };
        }

        private Delivery CreateDefaultDelivery()
        {
            return new Delivery
            {
                Charge = new Price
                {
                    Currency = "GBP",
                    DecimalPlaces = 2,
                    Value = 145,
                },
                Method = DeliveryMethod.Postage,
            };
        }

        private void AssertApiException(ApiException exception, HttpStatusCode code)
        {
            Assert.AreEqual(code, exception.ResponseCode);
        }

        private void AssertUpsertBasketSuccess(
            Basket.Models.Basket request,
            Basket.Models.Basket result,
            int expectedReservationsCount)
        {
            // todo: add additional checks when the basket API v2 is fully ready
            AssertUpsertBasketSuccessCommon(result, request.Delivery, expectedReservationsCount);
            Assert.AreEqual(request.ChannelId, result.ChannelId);
        }

        private void AssertUpsertBasketSuccess(
            UpsertBasketParameters request,
            Basket.Models.Basket result,
            int expectedReservationsCount)
        {
            // todo: add additional checks when the basket API v2 is fully ready
            AssertUpsertBasketSuccessCommon(result, request.Delivery, expectedReservationsCount);
            Assert.AreEqual(request.ChannelId, result.ChannelId);
        }

        private void AssertUpsertBasketSuccessCommon(
            Basket.Models.Basket result,
            Delivery sourceDelivery,
            int expectedReservationsCount)
        {
            // todo: add additional checks when the basket API v2 is fully ready
            Assert.NotNull(result.Reference);
            Assert.NotNull(result.Reservations);
            Assert.AreEqual(expectedReservationsCount, result.Reservations.Count);
        }

        private bool GetConfigBoolValueOrFalse(string configName)
        {
            return bool.TryParse(configuration[$"Basket:{configName}"], out var result) && result;
        }
    }
}