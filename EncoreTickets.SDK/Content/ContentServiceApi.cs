using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EncoreTickets.SDK.Content
{
    /// <summary>
    /// Wrapper class for the inventory service API
    /// </summary>
    public class ContentServiceApi : BaseCapabilityServiceApi
    {
        /// <summary>
        /// Default constructor for the Inventory service
        /// </summary>
        /// <param name="context"></param>
        public ContentServiceApi(ApiContext context) : base(context, "content.{0}.aws.encoretix.co.uk/api/")
        {
        }

        /// <summary>
        /// Search for a product
        /// </summary>
        /// <param name="text"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IList<Location> GetLocations(IProgressCallback callback)
        {
            ApiResultList<List<Location>> results =
            this.ExecuteApiList<List<Location>>("v1/locations", HttpMethod.Get, true, null);

            return results.GetList<Location>();
        }

        /// <summary>
        /// Get the available products
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IList<Product> GetProducts(IProgressCallback callback)
        {
            ApiResultList<List<Product>> result =
                this.ExecuteApiList<List<Product>>(
                    "v1/products?page=1&limit=1000",
                    HttpMethod.Get,
                    true,
                    null);

            return result.GetList<Product>();
        }

        /// <summary>
        /// Get the product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProductById(string id)
        {
            ApiResult<Product> result =
                    this.ExecuteApi<Product>(
                        string.Format("v1/products/{0}", id),
                        HttpMethod.Get,
                        true,
                        null);

            return result.Data;
        }

        /// <summary>
        /// Get the product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProductById(int id)
        {
            return this.GetProductById(id.ToString());
        }
    }
}
