using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.Mapping;
using FluentAssertions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Mapping
{
    internal class BasketProfileMappingTests
    {
        private const string DefaultCurrency = "GBP";
        private const int DefaultDecimalPlaces = 2;

        [Test]
        public void FromSeatToItemRequest_CorrectlyMapped()
        {
            var seat = new ReservationItem
            {
                AggregateReference = "reference",
                AreaName = "E"
            };

            var result = seat.Map<ReservationItem, ReservationItemParameters>();

            result.ShouldBeEquivalentToObjectWithMoreProperties(seat);
        }

        [Test]
        public void FromBasketToUpsertBasketRequest_CorrectlyMapped()
        {
            var sourceBasket = new SDK.Basket.Models.Basket
            {
                Reservations = Enumerable.Range(1, 3).Select(i => new Reservation { Quantity = i}).ToList(),
                Coupon = new Coupon { Code = "DISCOUNT" },
                Reference = "1234567",
                AllowFlexiTickets = true,
                ChannelId = "test-channel",
                Delivery = new Delivery
                {
                    Method = DeliveryMethod.Postage,
                    Charge = new Price
                    {
                        Currency = DefaultCurrency,
                        DecimalPlaces = DefaultDecimalPlaces,
                        Value = 145
                    }
                },
                ShopperReference = "test reference",
                ShopperCurrency = "USD"
            };

            var result = sourceBasket.Map<SDK.Basket.Models.Basket, UpsertBasketParameters>();

            result.ShouldBeEquivalentToObjectWithMoreProperties(sourceBasket);
            Assert.AreEqual(sourceBasket.AllowFlexiTickets, result.HasFlexiTickets);
            result.Delivery.Should().BeEquivalentTo(sourceBasket.Delivery);
            result.Coupon.Should().BeEquivalentTo(sourceBasket.Coupon);
            for (var i = 0; i < result.Reservations.Count; i++)
            {
                result.Reservations[i].ShouldBeEquivalentToObjectWithMoreProperties(sourceBasket.Reservations[i]);
            }
        }

        [Test]
        public void FromReservationToReservationRequest_CorrectlyMapped()
        {
            var reservation = new Reservation
            {
                Date = DateTimeOffset.Now,
                ProductId = "1234",
                VenueId = "123",
                Items = new List<ReservationItem> { new ReservationItem { AggregateReference = "reference1" }, new ReservationItem { AggregateReference = "reference2" } },
                Quantity = 2
            };

            var result = reservation.Map<Reservation, ReservationParameters>();

            result.ShouldBeEquivalentToObjectWithMoreProperties(reservation);
            for (var i = 0; i < result.Items.Count; i++)
            {
                result.Items[i].ShouldBeEquivalentToObjectWithMoreProperties(reservation.Items[i]);
            }
        }
    }
}
