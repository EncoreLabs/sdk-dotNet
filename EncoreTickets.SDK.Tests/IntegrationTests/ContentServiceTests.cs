using System;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Content;
using EncoreTickets.SDK.Content.Models;
using EncoreTickets.SDK.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    internal class ContentServiceTests
    {
        private IConfiguration configuration;
        private ContentServiceApi service;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            var context = new ApiContext(Environments.QA)
            {
                Correlation = Guid.NewGuid().ToString()
            };
            service = new ContentServiceApi(context);
        }

        [Test]
        public void GetLocations_Successful()
        {
            var locations = service.GetLocations();

            foreach (var location in locations)
            {
                Assert.False(string.IsNullOrEmpty(location.Name));
                Assert.False(string.IsNullOrEmpty(location.IsoCode));
                Assert.False(location.SubLocations.Any(loc => string.IsNullOrEmpty(loc.Name)));
            }
            Assert.NotNull(service.Context.ReceivedCorrelation);
        }

        [Test]
        public void GetProducts_Successful()
        {
            var products = service.GetProducts();

            foreach (var product in products)
            {
                AssertProductPropertiesAreSet(product);
            }
            Assert.NotNull(service.Context.ReceivedCorrelation);
        }

        [Test]
        public void GetProductById_Successful()
        {
            var productId = configuration["Content:TestProductId"];

            var product = service.GetProductById(productId);

            AssertProductPropertiesAreSet(product);
            Assert.NotNull(service.Context.ReceivedCorrelation);
        }

        [Test]
        public void GetProductById_Exception404()
        {
            var productId = "invalid";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var product = service.GetProductById(productId);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
            Assert.NotNull(service.Context.ReceivedCorrelation);
        }

        private void AssertProductPropertiesAreSet(Product product)
        {
            Assert.False(string.IsNullOrEmpty(product.Name));
            Assert.False(string.IsNullOrEmpty(product.Id));
            if (product.Venue != null)
            {
                Assert.False(string.IsNullOrEmpty(product.Venue.Name));
            }
        }
    }
}
