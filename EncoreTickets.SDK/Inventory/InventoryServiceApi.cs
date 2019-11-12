using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Inventory.Models.ResponseModels;
using EncoreTickets.SDK.Utilities;

namespace EncoreTickets.SDK.Inventory
{
    /// <summary>
    /// Wrapper class for the inventory service API
    /// </summary>
    public class InventoryServiceApi : BaseApi
    {
        /// <summary>
        /// Default constructor for the Inventory service
        /// </summary>
        /// <param name="context"></param>
        public InventoryServiceApi(ApiContext context) : base(context, "inventory-service.{0}tixuk.io/api/") { }

        public InventoryServiceApi(ApiContext context, string baseUrl) : base(context, baseUrl) { }

        /// <summary>
        /// Search for a product
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IList<Product> Search(string text)
        {
            var result = Executor.ExecuteApiWithWrappedResponse<List<Product>, ProductSearchResponse, List<Product>>(
                $"v2/search?query={text}",
                RequestMethod.Get,
                wrappedError: false);
            return result.DataOrException;
        }

        /// <summary>
        /// Get the performances for a given product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IList<Performance> GetPerformances(int productId, int quantity, DateTime from, DateTime to)
        {
            return GetPerformances(productId.ToString(), quantity, from, to);
        }

        /// <summary>
        /// Get the performances for a given product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IList<Performance> GetPerformances(string productId, int quantity, DateTime from, DateTime to)
        {
            var path = $"v2/availability/products/{productId}/quantity/{quantity}/from/{from.ToEncoreDate()}/to/{to.ToEncoreDate()}";
            var result = Executor.ExecuteApiWithNotWrappedResponse<List<Performance>>(path, RequestMethod.Get);
            return result.DataOrException;
        }

        /// <summary>
        /// Get the seats for a performance
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="performance"></param>
        /// <returns></returns>
        public Availability GetAvailability(string productId, int quantity, DateTime performance)
        {
            var path = $"v1/availability/products/{productId}/quantity/{quantity}/seats?date={performance.ToEncoreDate()}&time={performance.ToEncoreTime()}";
            var result = Executor.ExecuteApiWithNotWrappedResponse<Availability>(path, RequestMethod.Get);
            return result.DataOrException;
        }

        /// <summary>
        /// Get the first and last bookable dates for a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public BookingRange GetBookingRange(string productId)
        {
            var result = Executor.ExecuteApiWithWrappedResponse<BookingRange>(
                $"v3/products/{productId}/availability-range",
                RequestMethod.Get);
            return result.DataOrException;
        }
    }
}