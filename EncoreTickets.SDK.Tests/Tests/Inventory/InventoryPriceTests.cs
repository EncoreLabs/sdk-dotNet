using EncoreTickets.SDK.Inventory.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Inventory
{
    internal class InventoryPriceTests
    {
        [TestCase(4, "test", "test0")]
        [TestCase(400, "test", "test4")]
        [TestCase(999999999, "test", "test9999999")]
        [TestCase(null, "test", "test")]
        public void Inventory_Price_ToString_ReturnsCorrectly(int? value, string currency, string expected)
        {
            var price = new Price
            {
                value = value,
                currency = currency
            };
            Assert.AreEqual(expected, price.ToString());
        }
    }
}
