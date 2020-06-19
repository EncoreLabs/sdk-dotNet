using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Basket.Extensions;
using EncoreTickets.SDK.Basket.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket.Extensions
{
    internal class BasketExtensionTests
    {
        private const string DefaultCurrency = "GBP";
        private const int DefaultDecimalPlaces = 2;

        [TestCase(true, -15)]
        [TestCase(false, 15)]
        public void IsExpired_Correct(bool expectedResult, int minutesFromNow)
        {
            var basketDetails = new SDK.Basket.Models.Basket { ExpiredAt = DateTimeOffset.UtcNow.AddMinutes(minutesFromNow) };

            var result = basketDetails.IsExpired();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(6, 3, 2, 1)]
        [TestCase(0, 0, 0)]
        [TestCase(0)]
        public void ItemCount_Correct(int expectedResult, params int[] quantities)
        {
            var reservations = quantities?.Select(q => new Reservation { Quantity = q }).ToList();
            var basketDetails = new SDK.Basket.Models.Basket { Reservations = reservations };

            var result = basketDetails.ItemCount();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(true, "2600000034", "DISCOUNT")]
        [TestCase(true, "260000035", null)]
        [TestCase(false, null, null)]
        public void HasPromotion_Correct(bool expectedResult, string appliedPromotionId, string couponCode)
        {
            var basketDetails = SetupBasketWithPromotion(appliedPromotionId, couponCode);

            var result = basketDetails.HasPromotion();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(false, "2600000034", "DISCOUNT")]
        [TestCase(true, "260000035", null)]
        [TestCase(false, null, null)]
        public void HasAutomaticPromotion_Correct(bool expectedResult, string appliedPromotionId, string couponCode)
        {
            var basketDetails = SetupBasketWithPromotion(appliedPromotionId, couponCode);

            var result = basketDetails.HasAutomaticPromotion();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(true, "2600000034", "DISCOUNT")]
        [TestCase(false, "260000035", null)]
        [TestCase(false, null, null)]
        public void HasNonAutomaticPromotion_Correct(bool expectedResult, string appliedPromotionId, string couponCode)
        {
            var basketDetails = SetupBasketWithPromotion(appliedPromotionId, couponCode);

            var result = basketDetails.HasNonAutomaticPromotion();

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void TotalInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                    (p, i) => new Reservation { Quantity = i + 1, AdjustedSalePriceInOfficeCurrency = p },
                    r => r.AdjustedSalePriceInOfficeCurrency);

            var result = basketDetails.GetBasketTotalInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices.Value, result);
        }

        [Test]
        public void TotalInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, AdjustedSalePriceInShopperCurrency = p },
                r => r.AdjustedSalePriceInShopperCurrency);

            var result = basketDetails.GetBasketTotalInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices.Value, result);
        }

        [Test]
        public void TotalDiscountInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, AdjustmentAmountInOfficeCurrency = p },
                r => r.AdjustmentAmountInOfficeCurrency);

            var result = basketDetails.GetTotalDiscountInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices.Value, result);
        }

        [Test]
        public void TotalDiscountInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, AdjustmentAmountInShopperCurrency = p },
                r => r.AdjustmentAmountInShopperCurrency);

            var result = basketDetails.GetTotalDiscountInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices.Value, result);
        }

        [Test]
        public void TotalSalePriceInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, SalePriceInOfficeCurrency = p },
                r => r.SalePriceInOfficeCurrency);

            var result = basketDetails.GetTotalSalePriceInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices.Value, result);
        }

        [Test]
        public void TotalSalePriceInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, SalePriceInShopperCurrency = p },
                r => r.SalePriceInShopperCurrency);

            var result = basketDetails.GetTotalSalePriceInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices.Value, result);
        }

        [Test]
        public void TotalFaceInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, FaceValueInOfficeCurrency = p },
                r => r.FaceValueInOfficeCurrency);

            var result = basketDetails.GetTotalFaceValueInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices.Value, result);
        }

        [Test]
        public void TotalFaceInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, FaceValueInShopperCurrency = p },
                r => r.FaceValueInShopperCurrency);

            var result = basketDetails.GetTotalFaceValueInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices.Value, result);
        }

        [Test]
        public void TotalInOfficeCurrencyWithoutDelivery_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, AdjustedSalePriceInOfficeCurrency = p },
                r => r.AdjustedSalePriceInOfficeCurrency);
            basketDetails.Delivery = new Delivery
            {
                Method = DeliveryMethod.Postage,
                Charge = new Price
                {
                    Currency = DefaultCurrency,
                    DecimalPlaces = DefaultDecimalPlaces,
                    Value = 145,
                },
            };

            var result = basketDetails.GetBasketTotalWithoutDelivery();

            AssertPriceIsCorrect(sumOfPrices.Value - basketDetails.Delivery.Charge.Value.Value, result);
        }

        private SDK.Basket.Models.Basket SetupBasketWithPromotion(string appliedPromotionId, string couponCode)
        {
            var promotion = appliedPromotionId != null ? new Promotion { Id = appliedPromotionId } : null;
            var coupon = couponCode != null ? new Coupon { Code = couponCode } : null;
            return new SDK.Basket.Models.Basket { AppliedPromotion = promotion, Coupon = coupon };
        }

        private (int? sumOfPrices, SDK.Basket.Models.Basket basketDetails) CreateBasketDetailsFromDefaultPrices(Func<Price, int, Reservation> reservationFunc, Func<Reservation, Price> reverseFunc = null)
        {
            var defaultPrices = CreateDefaultListOfPrices();
            var reservations = defaultPrices.Select(reservationFunc).ToList();
            var sumOfPrices = reverseFunc != null ? reservations.Sum(r => r.Quantity * reverseFunc(r).Value).Value : (int?)null;
            var basketDetails = new SDK.Basket.Models.Basket { Reservations = reservations };
            return (sumOfPrices, basketDetails);
        }

        private List<Price> CreateDefaultListOfPrices()
        {
            return new List<Price>
            {
                new Price
                {
                    Currency = DefaultCurrency,
                    DecimalPlaces = DefaultDecimalPlaces,
                    Value = 2500,
                },
                new Price
                {
                    Currency = DefaultCurrency,
                    DecimalPlaces = DefaultDecimalPlaces,
                    Value = 3700,
                },
                new Price
                {
                    Currency = DefaultCurrency,
                    DecimalPlaces = DefaultDecimalPlaces,
                    Value = 1250,
                },
            };
        }

        private void AssertPriceIsCorrect(int expectedValue, Price price)
        {
            Assert.AreEqual(expectedValue, price.Value);
            Assert.AreEqual(DefaultCurrency, price.Currency);
            Assert.AreEqual(DefaultDecimalPlaces, price.DecimalPlaces);
        }
    }
}
