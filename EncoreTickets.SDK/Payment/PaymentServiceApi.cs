using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Payment.Models;
using EncoreTickets.SDK.Payment.Models.RequestModels;
using EncoreTickets.SDK.Payment.Serializers;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Payment
{
    public class PaymentServiceApi : BaseApiWithAuthentication, IPaymentServiceApi
    {
        private const string PaymentApiHost = "payment-service.{0}tixuk.io/api/";

        /// <inheritdoc/>
        public override int? ApiVersion => 1;

        public PaymentServiceApi(ApiContext context, bool automaticAuthentication = false)
            : base(context, PaymentApiHost, automaticAuthentication)
        {
        }

        /// <inheritdoc />
        public Order GetOrder(string channelId, string externalId)
        {
            if (string.IsNullOrWhiteSpace(channelId))
            {
                throw new ArgumentException("channel ID must be set");
            }

            if (string.IsNullOrWhiteSpace(externalId))
            {
                throw new ArgumentException("order external ID must be set");
            }

            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/orders/{channelId}/{externalId}",
                Method = RequestMethod.Get,
                Deserializer = new JsonResponseToOrderDeserializer()
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Order>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Order CreateOrder(CreateOrderRequest orderRequest)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/orders",
                Method = RequestMethod.Post,
                Body = orderRequest,
                Deserializer = new JsonResponseToOrderDeserializer()
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Order>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Order UpdateOrder(string orderId, UpdateOrderRequest orderRequest)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("order ID must be set");
            }

            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/orders/{orderId}",
                Method = RequestMethod.Patch,
                Body = orderRequest,
                Deserializer = new JsonResponseToOrderDeserializer()
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Order>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Models.Payment CreateNewPayment(CreatePaymentRequest paymentRequest)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/payments",
                Method = RequestMethod.Post,
                Body = paymentRequest
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Payment>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public List<CountryTerritorialUnit> GetUsStates()
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/countries/usa/states",
                Method = RequestMethod.Get,
                Deserializer = new JsonResponseToTerritorialUnitsDeserializer()
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<CountryTerritorialUnit>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public List<CountryTerritorialUnit> GetCanadaProvinces()
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/countries/canada/provinces",
                Method = RequestMethod.Get,
                Deserializer = new JsonResponseToTerritorialUnitsDeserializer()
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<CountryTerritorialUnit>>(parameters);
            return result.DataOrException;
        }
    }
}