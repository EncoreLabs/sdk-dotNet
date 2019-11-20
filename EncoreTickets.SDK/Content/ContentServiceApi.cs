using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Content.Models;

namespace EncoreTickets.SDK.Content
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IContentServiceApi" />
    /// <summary>
    /// The wrapper class for the content service API.
    /// </summary>
    public class ContentServiceApi : BaseApi, IContentServiceApi
    {
        /// <summary>
        /// Default constructor for the content service
        /// </summary>
        /// <param name="context"></param>
        public ContentServiceApi(ApiContext context) : base(context, "content-service.{0}tixuk.io/api/")
        {
        }

        /// <summary>
        /// Constructor for the content service
        /// </summary>
        public ContentServiceApi(ApiContext context, string baseUrl) : base(context, baseUrl)
        {
        }

        /// <inheritdoc />
        public IList<Location> GetLocations()
        {
            var results = Executor.ExecuteApiWithWrappedResponse<List<Location>>(
                "v1/locations",
                RequestMethod.Get);
            return results.DataOrException;
        }

        /// <inheritdoc />
        public IList<Product> GetProducts()
        {
            var result = Executor.ExecuteApiWithWrappedResponse<List<Product>>(
                "v1/products?page=1&limit=1000",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Product GetProductById(string id)
        {
            var result = Executor.ExecuteApiWithWrappedResponse<Product>(
                $"v1/products/{id}",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Product GetProductById(int id)
        {
            return GetProductById(id.ToString());
        }
    }
}