using System;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Inventory;
using EncoreTickets.SDK.Inventory.Models.RequestModels;
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
        private ApiContext context;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            context = new ApiContext(Environments.QA)
            {
                Affiliate = configuration["Inventory:TestAffiliateId"],
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
            Assert.IsNotNull(context.ReceivedCorrelation);
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
            Assert.IsNotNull(context.ReceivedCorrelation);
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
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAvailabilityRange_WithIntegerParameter_Successful()
        {
            var productId = int.Parse(configuration["Inventory:TestProductId"]);

            var bookingRange = service.GetAvailabilityRange(productId);

            Assert.NotNull(bookingRange.FirstBookableDate);
            Assert.NotNull(bookingRange.LastBookableDate);
            Assert.IsNotNull(context.ReceivedCorrelation);
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
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        #endregion

        #region GetAvailabilities

        [Test]
        public void GetAvailabilities_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];
            var startDate = new DateTime(2020, 12, 01);

            var availabilities = service.GetAvailabilities(productId, 2, startDate, startDate.AddMonths(1));

            foreach (var availability in availabilities)
            {
                Assert.NotNull(availability.LargestLumpOfTickets);
                Assert.AreNotEqual(availability.DateTime, default);
            }

            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAvailabilities_WithIntegerParameter_Successful()
        {
            var productId = int.Parse(configuration["Inventory:TestProductId"]);
            var startDate = new DateTime(2020, 12, 01);

            var availabilities = service.GetAvailabilities(productId, 2, startDate, startDate.AddMonths(1));

            foreach (var availability in availabilities)
            {
                Assert.NotNull(availability.LargestLumpOfTickets);
                Assert.AreNotEqual(availability.DateTime, default);
            }

            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAvailabilities_IfIntervalTooBig_Exception400()
        {
            var productId = configuration["Inventory:TestProductId"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var availabilities = service.GetAvailabilities(productId, 2, DateTime.Today, DateTime.Today.AddMonths(10));
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAvailabilities_IfStartDateInThePast_Exception400()
        {
            var productId = configuration["Inventory:TestProductId"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var availabilities = service.GetAvailabilities(productId, 2, DateTime.Today.AddDays(-10), DateTime.Today);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAvailabilities_IfProductIdInvalid_Exception400()
        {
            const string productId = "invalid_id";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var availabilities = service.GetAvailabilities(productId, 2, DateTime.Today, DateTime.Today.AddMonths(1));
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAvailabilities_IfProductNotFound_Exception404()
        {
            const string productId = "invalid-id";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var availabilities = service.GetAvailabilities(productId, 2, DateTime.Today, DateTime.Today.AddMonths(1));
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        #endregion

        #region GetAggregateSeatAvailability

        [Test]
        public void GetAggregateSeatAvailability_IfQuantityAndDateTimeAreSet_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];
            var startDate = new DateTime(2020, 10, 11);
            var endDate = new DateTime(2020, 12, 31);
            var availability = service.GetAvailabilities(productId, 1, startDate, endDate).First();

            var seats = service.GetAggregateSeatAvailability(productId, 1, availability.DateTime);

            Assert.IsNotEmpty(seats.Areas);
            foreach (var area in seats.Areas)
            {
                Assert.NotNull(area.AvailableCount);
                Assert.False(string.IsNullOrEmpty(area.Name));
            }

            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAggregateSeatAvailability_IfAllParamsAreSet_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];
            var startDate = new DateTime(2020, 10, 11);
            var endDate = new DateTime(2020, 12, 31);
            var availability = service.GetAvailabilities(productId, 1, startDate, endDate).First();
            var parameters = new AggregateSeatAvailabilityParameters
            {
                Quantity = 1,
                PerformanceTime = availability.DateTime,
                Direction = Direction.Desc,
            };

            var seats = service.GetAggregateSeatAvailability(productId, parameters);

            Assert.IsNotEmpty(seats.Areas);
            foreach (var area in seats.Areas)
            {
                Assert.NotNull(area.AvailableCount);
                Assert.False(string.IsNullOrEmpty(area.Name));
            }

            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAggregateSeatAvailability_IfQuantityLessThan1_Exception400()
        {
            var productId = configuration["Inventory:TestProductId"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var seats = service.GetAggregateSeatAvailability(productId, 0, DateTime.Now);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAggregateSeatAvailability_IfProductIdInvalid_Exception400()
        {
            var productId = "invalid_id";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var seats = service.GetAggregateSeatAvailability(productId, 2, DateTime.Now);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        public void GetAggregateSeatAvailability_IfProductNotFound_Exception404()
        {
            const string productId = "invalidid";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var seats = service.GetAggregateSeatAvailability(productId, 2, DateTime.Now);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        #endregion

        #region GetSeatAvailability

        [Test]
        [Obsolete]
        public void GetSeatAvailability_IfOnlyDateTimeIsSet_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];
            var startDate = new DateTime(2020, 10, 11);
            var endDate = new DateTime(2020, 12, 31);
            var availability = service.GetAvailabilities(productId, 1, startDate, endDate).First();

            var seats = service.GetSeatAvailability(productId, 1, availability.DateTime);

            Assert.IsNotEmpty(seats.Areas);
            foreach (var area in seats.Areas)
            {
                Assert.NotNull(area.AvailableCount);
                Assert.False(string.IsNullOrEmpty(area.Name));
                Assert.False(string.IsNullOrEmpty(area.ItemReference));
            }

            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        [Obsolete]
        public void GetSeatAvailability_IfAllParamsAreSet_Successful()
        {
            var productId = configuration["Inventory:TestProductId"];
            var startDate = new DateTime(2020, 10, 11);
            var endDate = new DateTime(2020, 12, 31);
            var availability = service.GetAvailabilities(productId, 1, startDate, endDate).First();
            var parameters = new SeatAvailabilityParameters
            {
                PerformanceTime = availability.DateTime,
                Direction = Direction.Desc,
                GroupingLimit = 1,
                Sort = "",
            };

            var seats = service.GetSeatAvailability(productId, 1, parameters);

            Assert.IsNotEmpty(seats.Areas);
            foreach (var area in seats.Areas)
            {
                Assert.NotNull(area.AvailableCount);
                Assert.False(string.IsNullOrEmpty(area.Name));
                Assert.False(string.IsNullOrEmpty(area.ItemReference));
            }

            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        [Obsolete]
        public void GetSeatAvailability_IfProductIdInvalid_Exception400()
        {
            var productId = "invalid_id";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var seats = service.GetSeatAvailability(productId, 2, DateTime.Now);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        [Test]
        [Obsolete]
        public void GetSeatAvailability_IfProductNotFound_Exception404()
        {
            const string productId = "invalidid";

            var exception = Assert.Catch<ApiException>(() =>
            {
                var seats = service.GetSeatAvailability(productId, 2, DateTime.Now);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
            Assert.IsNotNull(context.ReceivedCorrelation);
        }

        #endregion
    }
}
