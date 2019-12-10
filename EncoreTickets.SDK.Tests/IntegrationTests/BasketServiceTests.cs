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

            var removeReservationResult = service.RemoveReservation(upsertBasketResult.Reference, 1);

            Assert.IsEmpty(removeReservationResult.Reservations);
        }

        [Test]
        public void ClearBasket_Successful()
        {
            var reference = configuration["Basket:TestReferences:0"];
            var request = CreateDefaultBasketRequest(reference);
            var upsertBasketResult = service.UpsertBasket(request);

            var clearBasketResult = service.ClearBasket(upsertBasketResult.Reference);

            Assert.IsEmpty(clearBasketResult.Reservations);
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
            var upsertBasketResult = (BasketDetails)null;
            try
            {
                var reference = configuration["Basket:TestReferences:0"];
                var request = CreateDefaultBasketRequest(reference);
                request.Coupon = null;
                upsertBasketResult = service.UpsertBasket(request);
                var coupon = new Coupon { Code = configuration["Basket:ValidPromoCode"] };

                var basketDetails = service.UpsertPromotion(upsertBasketResult.Reference, coupon);

                Assert.Null(upsertBasketResult.Coupon);
                AssertExtension.SimplePropertyValuesAreEquals(coupon, basketDetails.Coupon);
                Assert.NotNull(basketDetails.AppliedPromotion);
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
            var request = CreateDefaultBasketRequest(reference);
            request.Reservations[0].Items[0].AggregateReference = "invalid";

            Assert.Throws<ApiException>(() => service.UpsertBasket(request));
        }

        private UpsertBasketRequest CreateDefaultBasketRequest(params string[] references)
        {
            return new UpsertBasketRequest
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
                Reservations = new List<ReservationRequest>
                {
                    new ReservationRequest
                    {
                        Date = new DateTimeOffset(2020, 4, 30, 19, 30, 0, TimeSpan.Zero),
                        ProductId = "1587",
                        VenueId = "138",
                        Quantity = references.Length,
                        Items = references.Select(r => new ItemRequest { AggregateReference = r }).ToList()
                    }
                }
            };
        }

        private void AssertUpsertBasketSuccess(UpsertBasketRequest request, BasketDetails result)
        {
            Assert.NotNull(result.Reference);
            Assert.NotNull(result.Checksum);
            Assert.AreEqual(request.ChannelId, result.ChannelId);
            Assert.AreEqual(request.Delivery.Method, result.Delivery.Method);
            AssertExtension.SimplePropertyValuesAreEquals(request.Delivery.Charge, result.Delivery.Charge);
            AssertExtension.SimplePropertyValuesAreEquals(request.Coupon, result.Coupon);
            Assert.NotNull(result.AppliedPromotion);
            Assert.NotNull(result.Reservations);
        }
    }
}
