using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Basket.Extensions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Inventory.Models;
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
            var basketDetails = new BasketDetails { expiredAt = DateTimeOffset.Now.AddMinutes(minutesFromNow) };

            var result = basketDetails.IsExpired();

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(6, 3, 2, 1)]
        [TestCase(0, 0, 0)]
        [TestCase(0)]
        public void Basket_ItemCount_Correct(int expectedResult, params int[] quantities)
        {
            var reservations = quantities?.Select(q => new Reservation {quantity = q}).ToList();
            var basketDetails = new BasketDetails {reservations = reservations};

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

        [Test]
        public void Basket_TotalInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                    (p, i) => new Reservation { quantity = i + 1, adjustedSalePriceInOfficeCurrency = p },
                    r => r.adjustedSalePriceInOfficeCurrency);

            var result = basketDetails.GetBasketTotalInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { quantity = i + 1, adjustedSalePriceInShopperCurrency = p },
                r => r.adjustedSalePriceInShopperCurrency);

            var result = basketDetails.GetBasketTotalInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalDiscountInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { quantity = i + 1, adjustmentAmountInOfficeCurrency = p },
                r => r.adjustmentAmountInOfficeCurrency);

            var result = basketDetails.GetTotalDiscountInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalDiscountInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { quantity = i + 1, adjustmentAmountInShopperCurrency = p },
                r => r.adjustmentAmountInShopperCurrency);

            var result = basketDetails.GetTotalDiscountInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalSalePriceInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { quantity = i + 1, salePriceInOfficeCurrency = p },
                r => r.salePriceInOfficeCurrency);

            var result = basketDetails.GetTotalSalePriceInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalSalePriceInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { quantity = i + 1, salePriceInShopperCurrency = p },
                r => r.salePriceInShopperCurrency);

            var result = basketDetails.GetTotalSalePriceInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalFaceInOfficeCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { quantity = i + 1, faceValueInOfficeCurrency = p },
                r => r.faceValueInOfficeCurrency);

            var result = basketDetails.GetTotalFaceValueInOfficeCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalFaceInShopperCurrency_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { quantity = i + 1, faceValueInShopperCurrency = p },
                r => r.faceValueInShopperCurrency);

            var result = basketDetails.GetTotalFaceValueInShopperCurrency();

            AssertPriceIsCorrect(sumOfPrices, result);
        }

        [Test]
        public void Basket_TotalInOfficeCurrencyWithoutDelivery_Correct()
        {
            var (sumOfPrices, basketDetails) = CreateBasketDetailsFromDefaultPrices(
                (p, i) => new Reservation { quantity = i + 1, adjustedSalePriceInOfficeCurrency = p },
                r => r.adjustedSalePriceInOfficeCurrency);
            basketDetails.delivery = new Delivery
            {
                method = "postage",
                charge = new Price
                {
                    currency = DefaultCurrency,
                    decimalPlaces = DefaultDecimalPlaces,
                    value = 145
                }
            };

            var result = basketDetails.GetBasketTotalWithoutDelivery();

            AssertPriceIsCorrect(sumOfPrices - basketDetails.delivery.charge.value.Value, result);
        }

        private BasketDetails SetupBasketWithPromotion(string appliedPromotionId, string couponCode)
        {
            var promotion = appliedPromotionId != null ? new Promotion { id = appliedPromotionId } : null;
            var coupon = couponCode != null ? new Coupon { code = couponCode } : null;
            return new BasketDetails { appliedPromotion = promotion, coupon = coupon };
        }

        private (int sumOfPrices, BasketDetails basketDetails) CreateBasketDetailsFromDefaultPrices(Func<Price, int, Reservation> reservationFunc, Func<Reservation, Price> reverseFunc)
        {
            var defaultPrices = CreateDefaultListOfPrices();
            var reservations = defaultPrices.Select(reservationFunc).ToList();
            var sumOfPrices = reservations.Sum(r => r.quantity * reverseFunc(r).value).Value;
            var basketDetails = new BasketDetails { reservations = reservations };
            return (sumOfPrices, basketDetails);
        }

        private List<Price> CreateDefaultListOfPrices()
        {
            return new List<Price>{ new Price
                {
                    currency = DefaultCurrency,
                    decimalPlaces = DefaultDecimalPlaces,
                    value = 2500
                },
                new Price
                {
                    currency = DefaultCurrency, 
                    decimalPlaces = DefaultDecimalPlaces, 
                    value = 3700
                },
                new Price
                {
                    currency = DefaultCurrency,
                    decimalPlaces = DefaultDecimalPlaces,
                    value = 1250
                }
            };
        }

        private void AssertPriceIsCorrect(int expectedValue, Price price)
        {
            Assert.AreEqual(expectedValue, price.value);
            Assert.AreEqual(DefaultCurrency, price.currency);
            Assert.AreEqual(DefaultDecimalPlaces, price.decimalPlaces);
        }
    }
}
