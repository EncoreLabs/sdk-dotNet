using System;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Inventory;
using EncoreTickets.SDK.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    internal class InventoryServiceTests
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

        #region SearchProducts

        [Test]
        public void SearchProducts_Successful()
        {
            const string searchTerm = "w";

            var products = service.SearchProducts(searchTerm);

            Assert.False(products.Any(p => string.IsNullOrEmpty(p.Name)));
        }

        [Test]
        public void SearchProducts_Exception404()
        {
            const string searchTerm = "verylongstrangesearchterm";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var products = service.SearchProducts(searchTerm);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
        }

        #endregion

        #region GetAvailabilityRange

        [Test]
        public void GetAvailabilityRange_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];

            var bookingRange = service.GetAvailabilityRange(productId);

            Assert.NotNull(bookingRange.FirstBookableDate);
            Assert.NotNull(bookingRange.LastBookableDate);
        }

        [Test]
        public void GetAvailabilityRange_Exception404()
        {
            const string productId = "not_id";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var products = service.GetAvailabilityRange(productId);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
        }

        #endregion

        [Test]
        public void GetPerformances_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];

            var performances = service.GetPerformances(productId, 2, DateTime.Today, DateTime.Today.AddMonths(1));

            foreach (var performance in performances)
            {
                Assert.NotNull(performance.LargestLumpOfTickets);
                Assert.AreNotEqual(performance.Datetime, default);
            }
        }

        [Test]
        public void GetPerformances_IfIntervalTooBig_Exception400()
        {
            var productId = configuration["Inventory:TestProductId"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var performances = service.GetPerformances(productId, 2, DateTime.Today, DateTime.Today.AddMonths(10));
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
        }

        [Test]
        public void GetPerformances_IfProductIdInvalid_Exception400()
        {
            const string productId = "invalid_id";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var performances = service.GetPerformances(productId, 2, DateTime.Today, DateTime.Today.AddMonths(1));
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
        }

        [Test]
        public void GetPerformances_IfProductNotFound_Exception404()
        {
            const string productId = "invalid-id";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var performances = service.GetPerformances(productId, 2, DateTime.Today, DateTime.Today.AddMonths(1));
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
        }

        [Test]
        public void GetAvailability_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];
            var performance = service.GetPerformances(productId, 2, DateTime.Today, DateTime.Today.AddMonths(2)).First();

            var seats = service.GetAvailability(productId, 2, performance.Datetime);

            Assert.IsNotEmpty(seats.Areas);
            foreach (var area in seats.Areas)
            {
                Assert.NotNull(area.AvailableCount);
                Assert.False(string.IsNullOrEmpty(area.Name));
                Assert.False(string.IsNullOrEmpty(area.ItemReference));
            }
        }

        [Test]
        public void GetAvailability_IfProductIdInvalid_Exception400()
        {
            var productId = "invalid_id";
            
            var exception = Assert.Catch<ApiException>(() =>
            {
                var seats = service.GetAvailability(productId, 2, DateTime.Now);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
        }

        [Test]
        public void GetAvailability_IfProductNotFound_Exception404()
        {
            const string productId = "invalidid";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var seats = service.GetAvailability(productId, 2, DateTime.Now);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
        }
    }
}
