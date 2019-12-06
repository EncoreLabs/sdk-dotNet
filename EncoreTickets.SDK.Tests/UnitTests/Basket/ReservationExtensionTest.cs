using EncoreTickets.SDK.Basket.Extensions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Inventory.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket
{
    class ReservationExtensionTest
    {
        private static readonly Price DefaultPrice = new Price
        {
            currency = "GBP",
            decimalPlaces = 2,
            value = 2500
        };

        private static readonly int DefaultQuantity = 2;

        [Test]
        public void Reservation_TotalAdjustedAmountInOfficeCurrency_Correct()
        {
            var reservation = new Reservation { adjustedSalePriceInOfficeCurrency = DefaultPrice, quantity = DefaultQuantity };

            var result = reservation.GetTotalAdjustedAmountInOfficeCurrency();

            AssertExtension.SimplePropertyValuesAreEquals(DefaultPrice * DefaultQuantity, result);
        }

        [Test]
        public void Reservation_TotalAdjustedAmountInShopperCurrency_Correct()
        {
            var reservation = new Reservation { adjustedSalePriceInShopperCurrency = DefaultPrice, quantity = DefaultQuantity };

            var result = reservation.GetTotalAdjustedAmountInShopperCurrency();

            AssertExtension.SimplePropertyValuesAreEquals(DefaultPrice * DefaultQuantity, result);
        }

        [Test]
        public void Reservation_TotalAdjustmentInOfficeCurrency_Correct()
        {
            var reservation = new Reservation { adjustmentAmountInOfficeCurrency = DefaultPrice, quantity = DefaultQuantity };

            var result = reservation.GetTotalAdjustmentAmountInOfficeCurrency();

            AssertExtension.SimplePropertyValuesAreEquals(DefaultPrice * DefaultQuantity, result);
        }

        [Test]
        public void Reservation_TotalAdjustmentInShopperCurrency_Correct()
        {
            var reservation = new Reservation { adjustmentAmountInShopperCurrency = DefaultPrice, quantity = DefaultQuantity };

            var result = reservation.GetTotalAdjustmentAmountInShopperCurrency();

            AssertExtension.SimplePropertyValuesAreEquals(DefaultPrice * DefaultQuantity, result);
        }

        [Test]
        public void Reservation_TotalSalePriceInOfficeCurrency_Correct()
        {
            var reservation = new Reservation { salePriceInOfficeCurrency = DefaultPrice, quantity = DefaultQuantity };

            var result = reservation.GetTotalSalePriceInOfficeCurrency();

            AssertExtension.SimplePropertyValuesAreEquals(DefaultPrice * DefaultQuantity, result);
        }

        [Test]
        public void Reservation_TotalSalePriceInShopperCurrency_Correct()
        {
            var reservation = new Reservation { salePriceInShopperCurrency = DefaultPrice, quantity = DefaultQuantity };

            var result = reservation.GetTotalSalePriceInShopperCurrency();

            AssertExtension.SimplePropertyValuesAreEquals(DefaultPrice * DefaultQuantity, result);
        }

        [Test]
        public void Reservation_TotalFaceInOfficeCurrency_Correct()
        {
            var reservation = new Reservation { faceValueInOfficeCurrency = DefaultPrice, quantity = DefaultQuantity };

            var result = reservation.GetTotalFaceValueInOfficeCurrency();

            AssertExtension.SimplePropertyValuesAreEquals(DefaultPrice * DefaultQuantity, result);
        }

        [Test]
        public void Reservation_TotalFaceInShopperCurrency_Correct()
        {
            var reservation = new Reservation { faceValueInShopperCurrency = DefaultPrice, quantity = DefaultQuantity };

            var result = reservation.GetTotalFaceValueInShopperCurrency();

            AssertExtension.SimplePropertyValuesAreEquals(DefaultPrice * DefaultQuantity, result);
        }
    }
}
