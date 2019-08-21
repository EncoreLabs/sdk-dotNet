using EncoreTickets.SDK.EntertainApi.Model;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.EntertainApi.Models
{
    internal class EntertainApiShoppingBasketTests
    {
        [Test]
        public void EntertainApi_ShoppingBasket_Constructor_InitializesCorrectly()
        {
            var customer = new ShoppingBasket();
            Assert.IsNotNull(customer.BasketItemHistories);
        }

        [TestCase("test", "test", true)]
        [TestCase("test", "", false)]
        [TestCase("test", null, false)]
        [TestCase("", "test", false)]
        [TestCase(null, "test", false)]
        public void EntertainApi_ShoppingBasket_IsValid_ReturnsCorrect(string id, string password, bool result)
        {
            var basket = new ShoppingBasket
            {
                Id = id,
                Password = password
            };
            Assert.AreEqual(result, basket.IsValid());
        }
    }
}
