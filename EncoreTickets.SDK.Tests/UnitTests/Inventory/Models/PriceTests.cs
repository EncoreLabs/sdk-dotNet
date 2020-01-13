using EncoreTickets.SDK.Inventory.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Inventory.Models
{
    internal class PriceTests
    {
        [TestCase(4, "USD", "0.04USD")]
        [TestCase(400, "GBP", "4.00GBP")]
        [TestCase(999999999, "USD", "9999999.99USD")]
        [TestCase(null, "JPY", "JPY")]
        public void ToString_ReturnsCorrectly(int? value, string currency, string expected)
        {
            var price = new Price
            {
                Value = value,
                Currency = currency
            };

            var actual = price.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
