using EncoreTickets.SDK.Basket.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket
{
    internal class BasketAddressTest
    {
        [Test]
        public void Basket_Address_Constructor_InitializesCorrectly()
        {
            var address = new Address();
            Assert.IsNotNull(address.Type);
        }
    }
}
