using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Basket;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    class BasketServiceTests
    {
        private IConfiguration configuration;
        private BasketServiceApi service;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            var context = new ApiContext(Environments.QA);
            service = new BasketServiceApi(context);
        }

        [Test]
        public void RemoveReservation_Successful()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasketRequest(reference);
            var upsertBasketResult = service.UpsertBasket(request);

            var removeReservationResult = service.RemoveReservation(upsertBasketResult.reference, 1);

            Assert.IsEmpty(removeReservationResult.reservations);
        }

        [Test]
        public void ClearBasket_Successful()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasketRequest(reference);
            var upsertBasketResult = service.UpsertBasket(request);

            var clearBasketResult = service.ClearBasket(upsertBasketResult.reference);

            Assert.IsEmpty(clearBasketResult.reservations);
        }

        [Test]
        public void UpsertBasket_GetBasket_Successful()
        {
            var upsertBasketResult = (BasketDetails)null;
            try
            {
                var reference = configuration["Basket:TestReferences:0"];
                var request = CreateDefaultBasketRequest(reference);
                upsertBasketResult = service.UpsertBasket(request);

                var basketDetails = service.GetBasketDetails(upsertBasketResult.reference);

                AssertUpsertBasketSuccess(request, basketDetails);
                Assert.AreEqual(upsertBasketResult.reference, basketDetails.reference);
                Assert.AreEqual(upsertBasketResult.checksum, basketDetails.checksum);
            }
            finally
            {
                service.ClearBasket(upsertBasketResult?.reference);
            }
        }

        [Test]
        public void GetPromotion_Successful()
        {
            const string promoId = "206000034";

            var promoDetails = service.GetPromotionDetails(promoId);

            Assert.AreEqual(promoId, promoDetails.id);
            Assert.NotNull(promoDetails.reference);
            Assert.NotNull(promoDetails.description);
            Assert.NotNull(promoDetails.displayText);
            Assert.NotNull(promoDetails.name);
            Assert.NotNull(promoDetails.reportingCode);
            Assert.AreNotEqual(promoDetails.validFrom, default);
            Assert.AreNotEqual(promoDetails.validTo, default);
        }

        [Test]
        public void ApplyPromotion_Successful()
        {
            var upsertBasketResult = (BasketDetails)null;
            try
            {
                var reference = configuration["Basket:TestReferences:0"];
                var request = CreateDefaultBasketRequest(reference);
                request.coupon = null;
                upsertBasketResult = service.UpsertBasket(request);
                var coupon = new Coupon { code = configuration["Basket:ValidPromoCode"] };

                var basketDetails = service.UpsertPromotion(upsertBasketResult.reference, coupon);

                Assert.Null(upsertBasketResult.coupon);
                AssertExtension.SimplePropertyValuesAreEquals(coupon, basketDetails.coupon);
                Assert.NotNull(basketDetails.appliedPromotion);
            }
            finally
            {
                service.ClearBasket(upsertBasketResult?.reference);
            }
        }

        [Test]
        public void UpsertBasket_Failed()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasketRequest(reference);
            request.reservations[0].items[0].aggregateReference = "invalid";

            Assert.Throws<ApiException>(() => service.UpsertBasket(request));
        }

        private UpsertBasketRequest CreateDefaultBasketRequest(params string[] references)
        {
            return new UpsertBasketRequest
            {
                channelId = "test-channel",
                coupon = new Coupon { code = configuration["Basket:ValidPromoCode"] },
                delivery = new Delivery
                {
                    charge = new Price
                    {
                        currency = "GBP",
                        decimalPlaces = 2,
                        value = 145
                    },
                    method = "postage"
                },
                reservations = new List<ReservationRequest>
                {
                    new ReservationRequest
                    {
                        date = new DateTimeOffset(2020, 4, 30, 19, 30, 0, TimeSpan.Zero),
                        productId = "1587",
                        venueId = "138",
                        quantity = references.Length,
                        items = references.Select(r => new ItemRequest { aggregateReference = r }).ToList()
                    }
                }
            };
        }

        private void AssertUpsertBasketSuccess(UpsertBasketRequest request, BasketDetails result)
        {
            Assert.NotNull(result.reference);
            Assert.NotNull(result.checksum);
            Assert.AreEqual(request.channelId, result.channelId);
            Assert.AreEqual(request.delivery.method, result.delivery.method);
            AssertExtension.SimplePropertyValuesAreEquals(request.delivery.charge, result.delivery.charge);
            AssertExtension.SimplePropertyValuesAreEquals(request.coupon, result.coupon);
            Assert.NotNull(result.appliedPromotion);
            Assert.NotNull(result.reservations);
        }
    }
}
