using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Inventory.Models.RequestModels;
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
        private const string InventoryApiHost = "inventory-service.{0}tixuk.io/api/";

        /// <inheritdoc/>
        public override int? ApiVersion => 4;

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
                Endpoint = $"v{ApiVersion}/search",
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
                Endpoint = $"v{ApiVersion}/products/{productId}/availability-range",
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
                Endpoint = $"v{ApiVersion}/availability/products/{productId}/quantity/{quantity}/from/{from.ToEncoreDate()}/to/{to.ToEncoreDate()}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<Availability>>(requestParameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public AggregateSeatAvailability GetAggregateSeatAvailability(string productId, int quantity, DateTime performance)
        {
            var parameters = new AggregateSeatAvailabilityParameters
            {
                PerformanceTime = performance,
                Quantity = quantity
            };
            return GetAggregateSeatAvailability(productId, parameters);
        }

        /// <inheritdoc />
        public AggregateSeatAvailability GetAggregateSeatAvailability(string productId, AggregateSeatAvailabilityParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("Product ID must be set");
            }

            if (parameters == null)
            {
                throw new ArgumentException("Parameters must be set");
            }

            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/products/{productId}/areas",
                Method = RequestMethod.Get,
                Query = new AggregateSeatAvailabilityQueryParameters(parameters)
            };
            var result = Executor.ExecuteApiWithWrappedResponse<AggregateSeatAvailability>(requestParameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        [Obsolete("Please use the GetAggregateSeatAvailability method. The data returned by this method is not compatible with the latest basket service.")]
        public SeatAvailability GetSeatAvailability(string productId, int quantity, DateTime? performance = null)
        {
            var optionalParameters = new SeatAvailabilityParameters { PerformanceTime = performance };
            return GetSeatAvailability(productId, quantity, optionalParameters);
        }

        /// <inheritdoc />
        [Obsolete("Please use the GetAggregateSeatAvailability method. The data returned by this method is not compatible with the latest basket service.")]
        public SeatAvailability GetSeatAvailability(string productId, int quantity, SeatAvailabilityParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException("Product ID must be set");
            }

            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/europa/availability/products/{productId}/quantity/{quantity}/seats",
                Method = RequestMethod.Get,
                Query = new SeatAvailabilityQueryParameters(parameters)
            };
            var result = Executor.ExecuteApiWithWrappedResponse<SeatAvailability>(requestParameters);
            return result.DataOrException;
        }
    }
}