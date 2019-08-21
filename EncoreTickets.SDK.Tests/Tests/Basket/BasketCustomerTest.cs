using EncoreTickets.SDK.Basket.Models;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Basket
{
    internal class BasketCustomerTest
    {
        [Test]
        public void Basket_Customer_Constructor_InitializesCorrectly()
        {
            var customer = new Customer();
            Assert.IsNotNull(customer.address);
        }
    }
}
