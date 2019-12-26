using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Basket.Extensions;
using EncoreTickets.SDK.Basket.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket
{
    class BasketDetailsTest
    {
        private const string DefaultCurrency = "GBP";
        private const int DefaultDecimalPlaces = 2;

        [TestCase(true, -15)]
        [TestCase(false, 15)]
        public void Basket_IsExpired_Correct(bool expectedResult, int minutesFromNow)
        {
            var basketDetails = new BasketDetails { ExpiredAt = DateTimeOffset.Now.AddMinutes(minutesFromNow) };

            var result = basketDetails.IsExpired();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(6, 3, 2, 1)]
        [TestCase(0, 0, 0)]
        [TestCase(0)]
        public void Basket_ItemCount_Correct(int expectedResult, params int[] quantities)
        {
            var reservations = quantities?.Select(q => new Reservation {Quantity = q}).ToList();
            var basketDetails = new BasketDetails {Reservations = reservations};

            var result = basketDetails.ItemCount();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(true, "2600000034", "DISCOUNT")]
        [TestCase(true, "260000035", null)]
        [TestCase(false, null, null)]
        public void Basket_HasPromotion_Correct(bool expectedResult, string appliedPromotionId, string couponCode)
        {
            var basketDetails = SetupBasketWithPromotion(appliedPromotionId, couponCode);

            var result = basketDetails.HasPromotion();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(false, "2600000034", "DISCOUNT")]
        [TestCase(true, "260000035", null)]
        [TestCase(false, null, null)]
        public void Basket_HasAutomaticPromotion_Correct(bool expectedResult, string appliedPromotionId, string couponCode)
        {
            var basketDetails = SetupBasketWithPromotion(appliedPromotionId, couponCode);

            var result = basketDetails.HasAutomaticPromotion();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(true, "2600000034", "DISCOUNT")]
        [TestCase(false, "260000035", null)]
        [TestCase(false, null, null)]
        public void Basket_HasNonAutomaticPromotion_Correct(bool expectedResult, string appliedPromotionId, string couponCode)
        {
            var basketDetails = SetupBasketWithPromotion(appliedPromotionId, couponCode);

            var result = basketDetails.HasNonAutomaticPromotion();

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Basket_TotalInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                    (p, i) => new Reservation { Quantity = i + 1, AdjustedSalePriceInOfficeCurrency = p },
                    r => r.AdjustedSalePriceInOfficeCurrency);

            var result = basketDetails.GetBasketTotalInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, AdjustedSalePriceInShopperCurrency = p },
                r => r.AdjustedSalePriceInShopperCurrency);

            var result = basketDetails.GetBasketTotalInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalDiscountInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, AdjustmentAmountInOfficeCurrency = p },
                r => r.AdjustmentAmountInOfficeCurrency);

            var result = basketDetails.GetTotalDiscountInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalDiscountInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, AdjustmentAmountInShopperCurrency = p },
                r => r.AdjustmentAmountInShopperCurrency);

            var result = basketDetails.GetTotalDiscountInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalSalePriceInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, SalePriceInOfficeCurrency = p },
                r => r.SalePriceInOfficeCurrency);

            var result = basketDetails.GetTotalSalePriceInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalSalePriceInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, SalePriceInShopperCurrency = p },
                r => r.SalePriceInShopperCurrency);

            var result = basketDetails.GetTotalSalePriceInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalFaceInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, FaceValueInOfficeCurrency = p },
                r => r.FaceValueInOfficeCurrency);

            var result = basketDetails.GetTotalFaceValueInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalFaceInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, FaceValueInShopperCurrency = p },
                r => r.FaceValueInShopperCurrency);

            var result = basketDetails.GetTotalFaceValueInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalInOfficeCurrencyWithoutDelivery_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { Quantity = i + 1, AdjustedSalePriceInOfficeCurrency = p },
                r => r.AdjustedSalePriceInOfficeCurrency);
            basketDetails.Delivery = new Delivery
            {
                Method = "postage",
                Charge = new Price
                {
                    Currency = DefaultCurrency,
                    DecimalPlaces = DefaultDecimalPlaces,
                    Value = 145
                }
            };

            var result = basketDetails.GetBasketTotalWithoutDelivery();

            AssertPriceIsCorrect(sumOfPrices - basketDetails.Delivery.Charge.Value.Value, result);
        }

        private BasketDetails SetupBasketWithPromotion(string appliedPromotionId, string couponCode)
        {
            var promotion = appliedPromotionId != null ? new Promotion { Id = appliedPromotionId } : null;
            var coupon = couponCode != null ? new Coupon { Code = couponCode } : null;
            return new BasketDetails { AppliedPromotion = promotion, Coupon = coupon };
        }

        private (int sumOfPrices, BasketDetails basketDetails) CreateBasketDetailsFromDefaultPrices(Func<Price, int, Reservation> reservationFunc, Func<Reservation, Price> reverseFunc)
        {
            var defaultPrices = CreateDefaultListOfPrices();
            var reservations = defaultPrices.Select(reservationFunc).ToList();
            var sumOfPrices = reservations.Sum(r => r.Quantity * reverseFunc(r).Value).Value;
            var basketDetails = new BasketDetails { Reservations = reservations };
            return (sumOfPrices, basketDetails);
        }

        private List<Price> CreateDefaultListOfPrices()
        {
            return new List<Price>{ new Price
                {
                    Currency = DefaultCurrency,
                    DecimalPlaces = DefaultDecimalPlaces,
                    Value = 2500
                },
                new Price
                {
                    Currency = DefaultCurrency,
                    DecimalPlaces = DefaultDecimalPlaces,
                    Value = 3700
                },
                new Price
                {
                    Currency = DefaultCurrency,
                    DecimalPlaces = DefaultDecimalPlaces,
                    Value = 1250
                }
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
