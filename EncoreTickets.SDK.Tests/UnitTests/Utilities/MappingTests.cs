using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.Common.Mapping;
using FluentAssertions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities
{
    internal class MappingTests
    {
        private const string DefaultCurrency = "GBP";
        private const int DefaultDecimalPlaces = 2;

        [Test]
        public void Basket_ToUpsertBasketRequest_CorrectlyMapped()
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
                    Method = "postage",
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

            var result = sourceBasket.Map<SDK.Basket.Models.Basket, UpsertBasketRequest>();

            result.ShouldBeEquivalentToObjectWithMoreProperties(sourceBasket);
            Assert.AreEqual(sourceBasket.AllowFlexiTickets, result.HasFlexiTickets);
            result.Delivery.Should().BeEquivalentTo(sourceBasket.Delivery);
            result.Coupon.Should().BeEquivalentTo(sourceBasket.Coupon);
            for (int i = 0; i < result.Reservations.Count; i++)
            {
                result.Reservations[i].ShouldBeEquivalentToObjectWithMoreProperties(sourceBasket.Reservations[i]);
            }
        }

        [Test]
        public void Reservation_ToReservationRequest_CorrectlyMapped()
        {
            var reservation = new Reservation
            {
                Date = DateTimeOffset.Now,
                ProductId = "1234",
                VenueId = "123",
                Items = new List<Seat> { new Seat { AggregateReference = "reference1" }, new Seat { AggregateReference = "reference2" } },
                Quantity = 2
            };

            var result = reservation.Map<Reservation, ReservationRequest>();

            result.ShouldBeEquivalentToObjectWithMoreProperties(reservation);
            for (int i = 0; i < result.Items.Count; i++)
            {
                result.Items[i].ShouldBeEquivalentToObjectWithMoreProperties(reservation.Items[i]);
            }
        }
    }
}
