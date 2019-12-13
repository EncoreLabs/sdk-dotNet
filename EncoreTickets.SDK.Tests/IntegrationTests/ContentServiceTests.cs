using System.Linq;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Content;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    class ContentServiceTests
    {
        private ContentServiceApi service;

        [SetUp]
        public void SetupState()
        {
            var context = new ApiContext(Environments.QA);
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
        }

        [Test]
        public void GetProducts_Successful()
        {
            var products = service.GetProducts();

            foreach (var product in products)
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
}
