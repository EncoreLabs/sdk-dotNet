using EncoreTickets.SDK.Basket.Extensions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket.Extensions
{
    internal class ReservationExtensionTests
    {
        private static readonly Price DefaultPrice = new Price
        {
            Currency = "GBP",
            DecimalPlaces = 2,
            Value = 2500
        };

        private static readonly int DefaultQuantity = 2;

        [Test]
        public void TotalAdjustedAmountInOfficeCurrency_Correct()
        {
            var reservation = new Reservation { AdjustedSalePriceInOfficeCurrency = DefaultPrice, Quantity = DefaultQuantity };

            var result = reservation.GetTotalAdjustedAmountInOfficeCurrency();

            AssertExtension.AreObjectsValuesEqual(DefaultPrice.MultiplyByNumber(DefaultQuantity), result);
        }

        [Test]
        public void TotalAdjustedAmountInShopperCurrency_Correct()
        {
            var reservation = new Reservation { AdjustedSalePriceInShopperCurrency = DefaultPrice, Quantity = DefaultQuantity };

            var result = reservation.GetTotalAdjustedAmountInShopperCurrency();

            AssertExtension.AreObjectsValuesEqual(DefaultPrice.MultiplyByNumber(DefaultQuantity), result);
        }

        [Test]
        public void TotalAdjustmentInOfficeCurrency_Correct()
        {
            var reservation = new Reservation { AdjustmentAmountInOfficeCurrency = DefaultPrice, Quantity = DefaultQuantity };

            var result = reservation.GetTotalAdjustmentAmountInOfficeCurrency();

            AssertExtension.AreObjectsValuesEqual(DefaultPrice.MultiplyByNumber(DefaultQuantity), result);
        }

        [Test]
        public void TotalAdjustmentInShopperCurrency_Correct()
        {
            var reservation = new Reservation { AdjustmentAmountInShopperCurrency = DefaultPrice, Quantity = DefaultQuantity };

            var result = reservation.GetTotalAdjustmentAmountInShopperCurrency();

            AssertExtension.AreObjectsValuesEqual(DefaultPrice.MultiplyByNumber(DefaultQuantity), result);
        }

        [Test]
        public void TotalSalePriceInOfficeCurrency_Correct()
        {
            var reservation = new Reservation { SalePriceInOfficeCurrency = DefaultPrice, Quantity = DefaultQuantity };

            var result = reservation.GetTotalSalePriceInOfficeCurrency();

            AssertExtension.AreObjectsValuesEqual(DefaultPrice.MultiplyByNumber(DefaultQuantity), result);
        }

        [Test]
        public void TotalSalePriceInShopperCurrency_Correct()
        {
            var reservation = new Reservation { SalePriceInShopperCurrency = DefaultPrice, Quantity = DefaultQuantity };

            var result = reservation.GetTotalSalePriceInShopperCurrency();

            AssertExtension.AreObjectsValuesEqual(DefaultPrice.MultiplyByNumber(DefaultQuantity), result);
        }

        [Test]
        public void TotalFaceInOfficeCurrency_Correct()
        {
            var reservation = new Reservation { FaceValueInOfficeCurrency = DefaultPrice, Quantity = DefaultQuantity };

            var result = reservation.GetTotalFaceValueInOfficeCurrency();

            AssertExtension.AreObjectsValuesEqual(DefaultPrice.MultiplyByNumber(DefaultQuantity), result);
        }

        [Test]
        public void TotalFaceInShopperCurrency_Correct()
        {
            var reservation = new Reservation { FaceValueInShopperCurrency = DefaultPrice, Quantity = DefaultQuantity };

            var result = reservation.GetTotalFaceValueInShopperCurrency();

            AssertExtension.AreObjectsValuesEqual(DefaultPrice.MultiplyByNumber(DefaultQuantity), result);
        }
    }
}
