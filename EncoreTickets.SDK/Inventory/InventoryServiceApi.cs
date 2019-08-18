using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Interfaces;
using RestSharp;

namespace EncoreTickets.SDK.Inventory
{
    /// <summary>
    /// Wrapper class for the inventory service API
    /// </summary>
    public class InventoryServiceApi : BaseCapabilityServiceApi
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
        /// <param name="callback"></param>
        /// <returns></returns>
        public IList<Product> Search(string text, IProgressCallback callback)
        {
            ApiResultList<SearchResponse> result =
            this.ExecuteApiList<SearchResponse>(
                string.Format("v2/search?query={0}", text),
                Method.GET,
                false,
                null);

            return result.GetList<Product>();
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
            return this.GetPerformances(productId.ToString(), quantity, from, to);
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
            var path = string.Format("v2/availability/products/{0}/quantity/{1}/from/{2}/to/{3}", productId, quantity, from.ToEncoreDate(), to.ToEncoreDate());

            ApiResultList<List<Performance>> result =
                this.ExecuteApiList<List<Performance>>(
                    path,
                    Method.GET,
                    false,
                    null);

            return result.GetList<Performance>();
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
            // https://inventory-service.tixuk.io/api/v1/availability/products/1587/quantity/2/seats?date=20181220&time=1930&t=1544138058971
            var path = string.Format("v1/availability/products/{0}/quantity/{1}/seats?date={2}&time={3}", productId, quantity, performance.ToEncoreDate(), performance.ToEncoreTime());

            ApiResult<Availability> result =
           this.ExecuteApi<Availability>(
               path,
               Method.GET,
               false,
               null);

            return result.Data;
        }

        /// <summary>
        /// Get the first and last bookable dates for a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public BookingRange GetBookingRange(string productId)
        {
            ApiResult<BookingRange> result = this.ExecuteApi<BookingRange>(string.Format("v3/products/{0}/availability-range", productId), Method.GET, true, null);

            return result.Data;
        }
    }
}