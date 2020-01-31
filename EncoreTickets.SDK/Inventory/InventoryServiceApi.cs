using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Inventory.Models.ResponseModels;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Utilities.Exceptions;

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
            if (string.IsNullOrEmpty(text))
            {
                throw new BadArgumentsException("search text must be set");
            }

            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = "v2/search",
                Method = RequestMethod.Get,
                Query = new
                {
                    query = text
                }
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<Product>, ProductSearchResponse, List<Product>>(requestParameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<Performance> GetPerformances(int productId, int quantity, DateTime fromDate, DateTime toDate)
        {
            return GetPerformances(productId.ToString(), quantity, fromDate, toDate);
        }

        /// <inheritdoc />
        public IList<Performance> GetPerformances(string productId, int quantity, DateTime from, DateTime to)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new BadArgumentsException("Product ID must be set");
            }

            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v2/availability/products/{productId}/quantity/{quantity}/from/{from.ToEncoreDate()}/to/{to.ToEncoreDate()}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithNotWrappedResponse<List<Performance>>(requestParameters);
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
                throw new BadArgumentsException("Product ID must be set");
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

        /// <inheritdoc />
        public BookingRange GetBookingRange(int productId)
        {
            return GetBookingRange(productId.ToString());
        }

        /// <inheritdoc />
        public BookingRange GetBookingRange(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new BadArgumentsException("Product ID must be set");
            }

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