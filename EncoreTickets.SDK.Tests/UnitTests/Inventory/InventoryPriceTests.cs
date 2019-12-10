using EncoreTickets.SDK.Inventory.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Inventory
{
    internal class InventoryPriceTests
    {
        [TestCase(4, "USD", "USD0")]
        [TestCase(400, "GBP", "GBP4")]
        [TestCase(999999999, "USD", "USD9999999")]
        [TestCase(null, "JPY", "JPY")]
        public void Inventory_Price_ToString_ReturnsCorrectly(int? value, string currency, string expected)
        {
            var price = new Price
            {
                Value = value,
                Currency = currency
            };
            Assert.AreEqual(expected, price.ToString());
        }
    }
}
