using EncoreTickets.SDK.Inventory.Extensions;
using EncoreTickets.SDK.Inventory.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Inventory.Extensions
{
    internal class ProductExtensionTests
    {
        [TestCase("yes", true)]
        [TestCase("YES", true)]
        [TestCase("YeS", true)]
        [TestCase("  YeS ", true)]
        [TestCase("no", false)]
        [TestCase("No", false)]
        [TestCase("test", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void GetOnSale_ReturnsCorrectly(string onSale, bool expected)
        {
            var product = new Product
            {
                OnSale = onSale
            };

            var result = product.GetOnSale();

            Assert.AreEqual(expected, result);
        }

        [TestCase("show", ProductType.Show)]
        [TestCase("attraction", ProductType.Attraction)]
        [TestCase("event", ProductType.Event)]
        [TestCase("test", ProductType.Other)]
        [TestCase("", ProductType.NotSet)]
        [TestCase(null, ProductType.NotSet)]
        public void GetProductType_ReturnsCorrectly(string type, ProductType expected)
        {
            var product = new Product
            {
                Type = type
            };

            var result = product.GetProductType();

            Assert.AreEqual(expected, result);
        }
    }
}
