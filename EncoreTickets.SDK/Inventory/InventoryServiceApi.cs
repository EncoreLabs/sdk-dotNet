using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Inventory.Models.ResponseModels;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
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
        public const int Version = 4;

        private static readonly string InventoryApiHost = $"inventory-service.{{0}}tixuk.io/api/v{Version}/";

        /// <summary>
        /// Default constructor for the Inventory service
        /// </summary>
        /// <param name="context"></param>
        public InventoryServiceApi(ApiContext context) : base(context, InventoryApiHost)
        {
        }

        /// <inheritdoc />
        public IList<Product> SearchProducts(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("search text must be set");
            }

            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = "search",
                Method = RequestMethod.Get,
                Query = new
                {
                    query = text
                }
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<Product>, ProductSearchResponse, ProductSearchResponseContent>(requestParameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public AvailabilityRange GetAvailabilityRange(int productId)
        {
            return GetAvailabilityRange(productId.ToString());
        }

        /// <inheritdoc />
        public AvailabilityRange GetAvailabilityRange(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("Product ID must be set");
            }

            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"products/{productId}/availability-range",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<AvailabilityRange>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<Availability> GetAvailabilities(int productId, int quantity, DateTime fromDate, DateTime toDate)
        {
            return GetAvailabilities(productId.ToString(), quantity, fromDate, toDate);
        }

        /// <inheritdoc />
        public IList<Availability> GetAvailabilities(string productId, int quantity, DateTime from, DateTime to)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("Product ID must be set");
            }

            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"availability/products/{productId}/quantity/{quantity}/from/{from.ToEncoreDate()}/to/{to.ToEncoreDate()}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<Availability>>(requestParameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Availability GetAvailability(int productId, int quantity, DateTime? performance = null)
        {
            return GetAvailability(productId.ToString(), quantity, performance);
        }

        /// <inheritdoc />
        public Availability GetAvailability(string productId, int quantity, DateTime? performance = null)
        {
            return GetAvailability(productId, quantity, performance, performance);
        }

        /// <inheritdoc />
        public Availability GetAvailability(string productId, int quantity, DateTime? date, DateTime? time)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("Product ID must be set");
            }

            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/availability/products/{productId}/quantity/{quantity}/seats",
                Method = RequestMethod.Get,
                Query = new
                {
                    date = date?.ToEncoreDate(),
                    time = time?.ToEncoreTime()
                }
            };
            var result = Executor.ExecuteApiWithNotWrappedResponse<Availability>(requestParameters);
            return result.DataOrException;
        }
    }
}