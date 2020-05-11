using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Content.Models;
using EncoreTickets.SDK.Content.Models.RequestModels;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Content
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IContentServiceApi" />
    /// <summary>
    /// The wrapper class for the content service API.
    /// </summary>
    public class ContentServiceApi : BaseApi, IContentServiceApi
    {
        private const string ContentApiHost = "content-service.{0}tixuk.io/api/";

        /// <inheritdoc/>
        public override int? ApiVersion => 1;

        /// <summary>
        /// Default constructor for the content service
        /// </summary>
        /// <param name="context"></param>
        public ContentServiceApi(ApiContext context) : base(context, ContentApiHost)
        {
        }

        /// <inheritdoc />
        public IList<Location> GetLocations()
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/locations",
                Method = RequestMethod.Get
            };
            var results = Executor.ExecuteApiWithWrappedResponse<List<Location>>(parameters);
            return results.DataOrException;
        }

        /// <inheritdoc />
        public IList<Product> GetProducts(GetProductsParameters requestParameters = null)
        {
            requestParameters ??= new GetProductsParameters();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/products",
                Method = RequestMethod.Get,
                Query = requestParameters
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<Product>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Product GetProductById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("product ID must be set");
            }

            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/products/{id}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Product>(parameters);
            return result.DataOrException;
        }
    }
}