using System.Collections.Generic;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Inventory.Models.ResponseModels;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Inventory.Models
{
    internal class ProductSearchResponseTests
    {
        [Test]
        public void Data_IsCorrect()
        {
            var product1 = new Product();
            var product2 = new Product();
            var response = new ProductSearchResponse
            {
                Response = new ProductSearchResponseContent
                {
                    Product = new List<Product> { product1, product2 },
                },
            };

            var result = response.Data;

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(product1));
            Assert.IsTrue(result.Contains(product2));
        }
    }
}
