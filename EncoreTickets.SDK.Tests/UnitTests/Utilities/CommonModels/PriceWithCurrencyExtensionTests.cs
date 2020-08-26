using System;
using System.Threading;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.CommonModels;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;
using EncoreTickets.SDK.Utilities.Exceptions;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.CommonModels
{
    [TestFixture]
    internal class PriceWithCurrencyExtensionTests
    {
        private const string DefaultCurrency = "GBP";
        private const int DefaultDecimalPlaces = 2;

        [SetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = TestHelper.Culture;
        }

        [TestCase(4, "USD", null, "0.04USD")]
        [TestCase(null, "USD", 1, "USD")]
        [TestCase(4, null, 3, "0.004")]
        [TestCase(4, null, null, "0.04")]
        [TestCase(null, "JPY", null, "JPY")]
        [TestCase(null, null, 2, "")]
        [TestCase(null, null, null, "")]
        [TestCase(400, "GBP", null, "4.00GBP")]
        [TestCase(999999999, "USD", null, "9999999.99USD")]
        [TestCase(10000, "GBP", 4, "1.0000GBP")]
        [TestCase(10000, "GBP", 10, "0.0000010000GBP")]
        [TestCase(19876543, "USD", 3, "19876.543USD")]
        [TestCase(123456789, "USD", 20, "0.00000000000123456789USD")]
        [TestCase(4550, "USD", 2, "45.50USD")]
        public void ToStringFormat_IfPriceIsNotNull_ReturnsCorrectly(int? value, string currency, int? decimalPlaces, string expected)
        {
            var price = new Price
            {
                Value = value,
                Currency = currency,
                DecimalPlaces = decimalPlaces,
            };

            var actual = price.ToStringFormat();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToStringFormat_IfPriceIsNull_ReturnsNull()
        {
            Price price = null;

            var actual = price.ToStringFormat();

            Assert.Null(actual);
        }

        [TestCase(10, 2, 1000)]
        [TestCase(1, 3, 1000)]
        [TestCase(0.1, 4, 1000)]
        [TestCase(0.001, 6, 1000)]
        [TestCase(10, null, 1000)]
        [TestCase(1000, 0, 1000)]
        [TestCase(-10, 2, -1000)]
        [TestCase(-9999999.99, null, -999999999)]
        public void SetDecimalValue_IfPriceIsNotNull_InitializesValueInPriceAndReturnsCorrectValue(decimal sourceValue, int? decimalPlaces, int expected)
        {
            var price = new Price
            {
                DecimalPlaces = decimalPlaces,
            };

            var actual = price.SetDecimalValue(sourceValue);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected, price.Value);
        }

        [Test]
        public void SetDecimalValue_IfPriceIsNull_ThrowsNullReferenceException()
        {
            Price price = null;

            Assert.Throws<NullReferenceException>(() =>
            {
                var actual = price.SetDecimalValue(It.IsAny<decimal>());
            });
        }

        [TestCase(1000, 2, 10)]
        [TestCase(1000, 3, 1)]
        [TestCase(1000, 4, 0.1)]
        [TestCase(1000, 6, 0.001)]
        [TestCase(1000, null, 10)]
        [TestCase(1000, 0, 1000)]
        [TestCase(-1000, 2, -10)]
        [TestCase(-999999999, null, -9999999.99)]
        [TestCase(null, null, 0)]
        [TestCase(null, 3, 0)]
        public void ValueToDecimal_IfPriceIsNotNull_ReturnsCorrectValue(int? sourceValue, int? decimalPlaces, decimal expected)
        {
            var price = new Price
            {
                Value = sourceValue,
                DecimalPlaces = decimalPlaces,
            };

            var actual = price.ValueToDecimal();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValueToDecimal_IfPriceIsNull_Returns0()
        {
            Price price = null;

            var actual = price.ValueToDecimal();

            Assert.AreEqual(0, actual);
        }

        [TestCase(1000, 2, "10.00")]
        [TestCase(1000, 3, "1.000")]
        [TestCase(1000, 4, "0.1000")]
        [TestCase(1000, 6, "0.001000")]
        [TestCase(1000, null, "10.00")]
        [TestCase(1000, 0, "1000")]
        [TestCase(-1000, 2, "-10.00")]
        [TestCase(-999999999, null, "-9999999.99")]
        [TestCase(null, null, "0.00")]
        [TestCase(null, 3, "0.000")]
        public void ValueToDecimalAsString_IfPriceIsNotNull_ReturnsCorrectValue(int? sourceValue, int? decimalPlaces, string expected)
        {
            var price = new Price
            {
                Value = sourceValue,
                DecimalPlaces = decimalPlaces,
            };

            var actual = price.ValueToDecimalAsString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValueToDecimalAsString_IfPriceIsNull_Returns0AsString()
        {
            Price price = null;

            var actual = price.ValueToDecimalAsString();

            Assert.AreEqual("0.00", actual);
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
                Value = value,
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
