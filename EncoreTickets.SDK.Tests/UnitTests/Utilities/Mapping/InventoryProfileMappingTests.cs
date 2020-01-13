using EncoreTickets.SDK.Inventory.Extensions;
using EncoreTickets.SDK.Utilities.Mapping;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Mapping
{
    internal class InventoryProfileMappingTests
    {
        [TestCase("show", ProductType.Show)]
        [TestCase("attraction", ProductType.Attraction)]
        [TestCase("event", ProductType.Event)]
        [TestCase("test", ProductType.Other)]
        [TestCase("", ProductType.NotSet)]
        [TestCase(null, ProductType.NotSet)]
        public void FromStringToProductType_ReturnsCorrectly(string type, ProductType expected)
        {
            var result = type.Map<string, ProductType>();

            Assert.AreEqual(expected, result);
        }
    }
}