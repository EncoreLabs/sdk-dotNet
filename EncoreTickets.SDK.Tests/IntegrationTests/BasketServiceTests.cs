using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Basket;
using EncoreTickets.SDK.Basket.Exceptions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    internal class BasketServiceTests
    {
        private IConfiguration configuration;
        private BasketServiceApi service;
        private bool runPromoCodeTests = false;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            var context = new ApiContext(Environments.QA);
            service = new BasketServiceApi(context);
        }

        [Test]
        public void GetBasket_Successful()
        {
            var reference = configuration["Basket:TestBasketReference"];

            var result = service.GetBasketDetails(reference);

            Assert.NotNull(result);
            Assert.AreEqual(reference, result.Reference);
            Assert.NotNull(result.Checksum);
            Assert.NotNull(result.ChannelId);
        }

        [Test]
        public void GetBasket_Exception400()
        {
            var reference = "test";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.GetBasketDetails(reference);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [Test]
        public void GetBasket_Exception404()
        {
            var reference = configuration["Basket:TestBasketReferenceNotFound"];
            
            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.GetBasketDetails(reference);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        [Test]
        public void UpsertBasket_GetBasket_Successful()
        {
            VerifyPromoCodeTestsEnabled();
            var upsertBasketResult = (Basket.Models.Basket)null;
            try
            {
                var reference = configuration["Basket:TestReferences:0"];
                var request = CreateDefaultBasket(reference);
                upsertBasketResult = service.UpsertBasket(request);

                var basketDetails = service.GetBasketDetails(upsertBasketResult.Reference);

                AssertUpsertBasketSuccess(request, basketDetails);
                Assert.AreEqual(upsertBasketResult.Reference, basketDetails.Reference);
                Assert.AreEqual(upsertBasketResult.Checksum, basketDetails.Checksum);
            }
            finally
            {
                service.ClearBasket(upsertBasketResult?.Reference);
            }
        }

        [Test]
        public void UpsertBasket_Failed()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            request.Reservations[0].Items[0].AggregateReference = "invalid";

            Assert.Throws<ApiException>(() => service.UpsertBasket(request));
        }

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
        public void RemoveReservation_Successful()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasket(reference);
            var upsertBasketResult = service.UpsertBasket(request);

            var removeReservationResult = service.RemoveReservation(upsertBasketResult.Reference, 1);

            Assert.IsEmpty(removeReservationResult.Reservations);
        }

        [Test]
        public void GetPromotion_Successful()
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
        public void ApplyPromotion_Successful()
        {
            VerifyPromoCodeTestsEnabled();
            var upsertBasketResult = (Basket.Models.Basket)null;
            Coupon coupon;
            try
            {
                (upsertBasketResult, coupon) = PrepareUpsertPromotionRequest(configuration["Basket:TestReferences:0"],
                    configuration["Basket:ValidPromoCode"]);

                var basketDetails = service.UpsertPromotion(upsertBasketResult.Reference, coupon);

                Assert.Null(upsertBasketResult.Coupon);
                AssertExtension.AreObjectsValuesEqual(coupon, basketDetails.Coupon);
                Assert.NotNull(basketDetails.AppliedPromotion);
            }
            finally
            {
                service.ClearBasket(upsertBasketResult?.Reference);
            }
        }

        [Test]
        public void ApplyPromotion_InvalidPromoCode()
        {
            Basket.Models.Basket basket = null;
            Coupon coupon;
            try
            {
                (basket, coupon) = PrepareUpsertPromotionRequest(configuration["Basket:TestReferences:0"],
                    "invalid promo code");

                Assert.Throws<InvalidPromoCodeException>(() =>
                {
                    service.UpsertPromotion(basket.Reference, coupon);
                });
            }
            finally
            {
                service.ClearBasket(basket?.Reference);
            }
        }

        [Test]
        public void ApplyPromotion_BasketNotFound()
        {
            Assert.Throws<BasketNotFoundException>(() =>
            {
                service.UpsertPromotion(configuration["Basket:TestBasketReferenceNotFound"], new Coupon { Code = "test" });
            });
        }

        [Test]
        public void ApplyPromotion_BasketCannotBeModified()
        {
            Assert.Throws<BasketCannotBeModifiedException>(() =>
            {
                service.UpsertPromotion("invalid basket reference", new Coupon { Code = "test" });
            });
        }

        private void VerifyPromoCodeTestsEnabled()
        {
            if (!runPromoCodeTests)
            {
                Assert.Ignore("The promo code involving tests are disabled by default because they use a paid service. " +
                              "Set 'runPromoCodeTests' field to true to run the tests.");
            }
        }

        private Basket.Models.Basket CreateDefaultBasket(params string[] references)
        {
            return new Basket.Models.Basket
            {
                ChannelId = "test-channel",
                Coupon = new Coupon { Code = configuration["Basket:ValidPromoCode"] },
                Delivery = new Delivery
                {
                    Charge = new Price
                    {
                        Currency = "GBP",
                        DecimalPlaces = 2,
                        Value = 145
                    },
                    Method = "postage"
                },
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        Date = DateTimeOffset.ParseExact(configuration["Basket:TestDate"], "yyyy-MM-ddTHH:mm", 
                            CultureInfo.InvariantCulture),
                        ProductId = configuration["Basket:TestProductId"],
                        VenueId = configuration["Basket:TestVenueId"],
                        Quantity = references.Length,
                        Items = references.Select(r => new Seat { AggregateReference = r }).ToList()
                    }
                }
            };
        }

        private (Basket.Models.Basket Basket, Coupon Coupon) PrepareUpsertPromotionRequest(
            string aggregateReference, string couponCode)
        {
            var request = CreateDefaultBasket(aggregateReference);
            request.Coupon = null;
            var upsertBasketResult = service.UpsertBasket(request);
            var coupon = new Coupon { Code = couponCode };
            return (upsertBasketResult, coupon);
        }

        private void AssertApiException(ApiException exception, HttpStatusCode code)
        {
            Assert.AreEqual(code, exception.ResponseCode);
        }

        private void AssertUpsertBasketSuccess(Basket.Models.Basket request, Basket.Models.Basket result)
        {
            Assert.NotNull(result.Reference);
            Assert.NotNull(result.Checksum);
            Assert.AreEqual(request.ChannelId, result.ChannelId);
            Assert.AreEqual(request.Delivery.Method, result.Delivery.Method);
            AssertExtension.AreObjectsValuesEqual(request.Delivery.Charge, result.Delivery.Charge);
            AssertExtension.AreObjectsValuesEqual(request.Coupon, result.Coupon);
            Assert.NotNull(result.AppliedPromotion);
            Assert.NotNull(result.Reservations);
        }
    }
}
