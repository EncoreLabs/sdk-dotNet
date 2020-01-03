using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;
using EncoreTickets.SDK.Utilities.Exceptions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities
{
    internal class PriceWithCurrencyTests
    {
        private static readonly string DefaultCurrency = "GBP";
        private static readonly int DefaultDecimalPlaces = 2;

        [TestCase(6551, 4500, 2051)]
        [TestCase(50, 50, null)]
        [TestCase(50, null, 50)]
        [TestCase(0, null, null)]
        public void Price_Add_Successful(int? expectedResult, int? firstPriceValue, int? secondPriceValue)
        {
            var firstPrice = CreatePrice(firstPriceValue, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = CreatePrice(secondPriceValue, DefaultCurrency, DefaultDecimalPlaces);

            var result = firstPrice.Add(secondPrice);

            AssertPriceIsExpected(expectedResult, result);
        }

        [TestCase(2449, 4500, 2051)]
        [TestCase(50, 50, null)]
        [TestCase(-50, null, 50)]
        [TestCase(0, null, null)]
        public void Price_Subtract_Successful(int? expectedResult, int? firstPriceValue, int? secondPriceValue)
        {
            var firstPrice = CreatePrice(firstPriceValue, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = CreatePrice(secondPriceValue, DefaultCurrency, DefaultDecimalPlaces);

            var result = firstPrice.Subtract(secondPrice);

            AssertPriceIsExpected(expectedResult, result);
        }

        [TestCase(13500, 4500, 3)]
        [TestCase(0, null, 2)]
        public void Price_Multiply_Successful(int? expectedResult, int? priceValue, int number)
        {
            var price = CreatePrice(priceValue, DefaultCurrency, DefaultDecimalPlaces);

            var result = price.MultiplyByNumber(number);

            AssertPriceIsExpected(expectedResult, result);
        }

        [TestCase("USD", 2)]
        [TestCase("GBP", 3)]
        public void Price_Add_MismatchedCurrencies(string currencyCode, int decimalPlaces)
        {
            var firstPrice = CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = CreatePrice(2000, currencyCode, decimalPlaces);

            Assert.Throws<CurrenciesDontMatchException>(() => firstPrice.Add(secondPrice));
        }

        [TestCase("USD", 2)]
        [TestCase("GBP", 3)]
        public void Price_Subtract_MismatchedCurrencies(string currencyCode, int decimalPlaces)
        {
            var firstPrice = CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = CreatePrice(2000, currencyCode, decimalPlaces);

            Assert.Throws<CurrenciesDontMatchException>(() => firstPrice.Subtract(secondPrice));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Price_Add_NullPrice(bool firstPriceNull, bool secondPriceNull)
        {
            var firstPrice = firstPriceNull ? null : CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = secondPriceNull ? null : CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);

            var result = firstPrice.Add(secondPrice);

            Assert.Null(result);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Price_Subtract_NullPrice(bool firstPriceNull, bool secondPriceNull)
        {
            var firstPrice = firstPriceNull ? null : CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = secondPriceNull ? null : CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);

            var result = firstPrice.Add(secondPrice);

            Assert.Null(result);
        }

        [Test]
        public void Price_Multiply_NullPrice()
        {
            var price = (Price)null;

            var result = price.MultiplyByNumber(2);

            Assert.Null(result);
        }

        private Price CreatePrice(int? value, string currency, int? decimalPlaces)
        {
            return new Price
            {
                Currency = currency,
                DecimalPlaces = decimalPlaces,
                Value = value
            };
        }

        private void AssertPriceIsExpected(int? expectedResult, Price result)
        {
            Assert.AreEqual(expectedResult, result.Value);
            Assert.AreEqual(DefaultCurrency, result.Currency);
            Assert.AreEqual(DefaultDecimalPlaces, result.DecimalPlaces);
        }
    }
}
