using System.Threading;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Tests.Helpers;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket.Models
{
    [TestFixture]
    internal class PriceTests
    {
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
        public void ToString_ReturnsCorrectly(int? value, string currency, int? decimalPlaces, string expected)
        {
            var price = new Price
            {
                Value = value,
                Currency = currency,
                DecimalPlaces = decimalPlaces,
            };

            var actual = price.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
