using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Utilities.CommonModels;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;
using EncoreTickets.SDK.Utilities.Exceptions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.CommonModels
{
    internal class PriceWithCurrencyExtensionTests
    {
        private const string DefaultCurrency = "GBP";
        private const int DefaultDecimalPlaces = 2;

        [TestCase(4, "USD", null, "0.04USD")]
        [TestCase(400, "GBP", null, "4.00GBP")]
        [TestCase(999999999, "USD", null, "9999999.99USD")]
        [TestCase(null, "JPY", null, "JPY")]
        [TestCase(10000, "GBP", 4, "1.00GBP")]
        [TestCase(10000, "GBP", 10, "0.000001GBP")]
        [TestCase(19876543, "USD", 3, "19876.543USD")]
        [TestCase(123456789, "USD", 20, "0.00000000000123456789USD")]
        [TestCase(4550, "USD", 2, "45.50USD")]
        public void ToStringFormat_ReturnsCorrectly(int? value, string currency, int? decimalPlaces, string expected)
        {
            var price = new Price
            {
                Value = value,
                Currency = currency,
                DecimalPlaces = decimalPlaces
            };

            var actual = price.ToStringFormat();

            Assert.AreEqual(expected, actual);
        }

        [TestCase(6551, 4500, 2051)]
        [TestCase(50, 50, null)]
        [TestCase(50, null, 50)]
        [TestCase(0, null, null)]
        public void Add_ReturnsCorrectly(int? expectedResult, int? firstPriceValue, int? secondPriceValue)
        {
            var firstPrice = CreatePrice(firstPriceValue, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = CreatePrice(secondPriceValue, DefaultCurrency, DefaultDecimalPlaces);

            var result = firstPrice.Add(secondPrice);

            AssertPriceIsExpected(expectedResult, result);
        }

        [TestCase("USD", 2)]
        [TestCase("GBP", 3)]
        public void Add_MismatchedCurrencies(string currencyCode, int decimalPlaces)
        {
            var firstPrice = CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = CreatePrice(2000, currencyCode, decimalPlaces);

            Assert.Throws<CurrenciesDontMatchException>(() => firstPrice.Add(secondPrice));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Add_NullPrice(bool firstPriceNull, bool secondPriceNull)
        {
            var firstPrice = firstPriceNull ? null : CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = secondPriceNull ? null : CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);

            var result = firstPrice.Add(secondPrice);

            Assert.Null(result);
        }

        [TestCase(2449, 4500, 2051)]
        [TestCase(50, 50, null)]
        [TestCase(-50, null, 50)]
        [TestCase(0, null, null)]
        public void Subtract_ReturnsCorrectly(int? expectedResult, int? firstPriceValue, int? secondPriceValue)
        {
            var firstPrice = CreatePrice(firstPriceValue, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = CreatePrice(secondPriceValue, DefaultCurrency, DefaultDecimalPlaces);

            var result = firstPrice.Subtract(secondPrice);

            AssertPriceIsExpected(expectedResult, result);
        }

        [TestCase("USD", 2)]
        [TestCase("GBP", 3)]
        public void Subtract_MismatchedCurrencies(string currencyCode, int decimalPlaces)
        {
            var firstPrice = CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = CreatePrice(2000, currencyCode, decimalPlaces);

            Assert.Throws<CurrenciesDontMatchException>(() => firstPrice.Subtract(secondPrice));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Subtract_NullPrice(bool firstPriceNull, bool secondPriceNull)
        {
            var firstPrice = firstPriceNull ? null : CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);
            var secondPrice = secondPriceNull ? null : CreatePrice(2500, DefaultCurrency, DefaultDecimalPlaces);

            var result = firstPrice.Add(secondPrice);

            Assert.Null(result);
        }

        [TestCase(13500, 4500, 3)]
        [TestCase(0, null, 2)]
        public void Multiply_ReturnsCorrectly(int? expectedResult, int? priceValue, int number)
        {
            var price = CreatePrice(priceValue, DefaultCurrency, DefaultDecimalPlaces);

            var result = price.MultiplyByNumber(number);

            AssertPriceIsExpected(expectedResult, result);
        }

        [Test]
        public void Multiply_NullPrice()
        {
            var price = (Price)null;

            var result = price.MultiplyByNumber(2);

            Assert.Null(result);
        }

        private static Price CreatePrice(int? value, string currency, int? decimalPlaces)
        {
            return new Price
            {
                Currency = currency,
                DecimalPlaces = decimalPlaces,
                Value = value
            };
        }

        private static void AssertPriceIsExpected(int? expectedResult, IPriceWithCurrency result)
        {
            Assert.AreEqual(expectedResult, result.Value);
            Assert.AreEqual(DefaultCurrency, result.Currency);
            Assert.AreEqual(DefaultDecimalPlaces, result.DecimalPlaces);
        }
    }
}
