using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Inventory.Models.ResponseModels;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Inventory
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IInventoryServiceApi" />
    /// <summary>
    /// Wrapper class for the inventory service API
    /// </summary>
    public class InventoryServiceApi : BaseApi, IInventoryServiceApi
    {
        private const string InventoryApiHost = "inventory-service.{0}tixuk.io/api/";

        /// <summary>
        /// Default constructor for the Inventory service
        /// </summary>
        /// <param name="context"></param>
        public InventoryServiceApi(ApiContext context) : base(context, InventoryApiHost)
        {
        }

        /// <inheritdoc />
        public IList<Product> Search(string text)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v2/search?query={text}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<Product>, ProductSearchResponse, List<Product>>(parameters, false);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<Performance> GetPerformances(int productId, int quantity, DateTime from, DateTime to)
        {
            return GetPerformances(productId.ToString(), quantity, from, to);
        }

        /// <inheritdoc />
        public IList<Performance> GetPerformances(string productId, int quantity, DateTime from, DateTime to)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v2/availability/products/{productId}/quantity/{quantity}/from/{from.ToEncoreDate()}/to/{to.ToEncoreDate()}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithNotWrappedResponse<List<Performance>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Availability GetAvailability(string productId, int quantity, DateTime performance)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/availability/products/{productId}/quantity/{quantity}/seats?date={performance.ToEncoreDate()}&time={performance.ToEncoreTime()}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithNotWrappedResponse<Availability>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public BookingRange GetBookingRange(string productId)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v3/products/{productId}/availability-range",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<BookingRange>(parameters);
            return result.DataOrException;
        }
    }
}