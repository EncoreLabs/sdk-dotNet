using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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
        public InventoryServiceApi(ApiContext context) : base(context, "inventory.{0}.aws.encoretix.co.uk/api/")
        {
        }

        /// <summary>
        /// Search for a product
        /// </summary>
        /// <param name="text"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IList<Product> Search(string text, IProgressCallback callback)
        {
            ApiResultList<SearchResponse> result = 
            this.ExecuteApiList<SearchResponse> (
                string.Format("v2/search?query={0}", text),
                HttpMethod.Get,
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
                    HttpMethod.Get,
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
               HttpMethod.Get,
               false,
               null);

            return result.Data;
        }
    }
}
