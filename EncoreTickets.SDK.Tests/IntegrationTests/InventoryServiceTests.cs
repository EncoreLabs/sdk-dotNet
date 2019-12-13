using System;
using System.Linq;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Inventory;
using EncoreTickets.SDK.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    class InventoryServiceTests
    {
        private IConfiguration configuration;
        private InventoryServiceApi service;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            var context = new ApiContext(Environments.QA)
            {
                Affiliate = configuration["Inventory:TestAffiliateId"]
            };
            service = new InventoryServiceApi(context);
        }

        [Test]
        public void SearchProducts_Successful()
        {
            const string searchTerm = "w";

            var products = service.Search(searchTerm);

            Assert.False(products.Any(p => string.IsNullOrEmpty(p.name)));
        }

        [Test]
        public void GetPerformances_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];

            var performances = service.GetPerformances(productId, 2, DateTime.Today, DateTime.Today.AddMonths(1));

            foreach (var performance in performances)
            {
                Assert.NotNull(performance.largestLumpOfTickets);
                Assert.AreNotEqual(performance.datetime, default);
            }
        }

        [Test]
        public void SearchAvailability_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];
            var performance = service.GetPerformances(productId, 2, DateTime.Today, DateTime.Today.AddMonths(1)).First();

            var seats = service.GetAvailability(productId, 2, performance.datetime);

            Assert.IsNotEmpty(seats.areas);
            foreach (var area in seats.areas)
            {
                Assert.NotNull(area.availableCount);
                Assert.False(string.IsNullOrEmpty(area.name));
                Assert.False(string.IsNullOrEmpty(area.itemReference));
            }
        }

    }
}
