using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Payment.Models;
using EncoreTickets.SDK.Payment.Models.RequestModels;

namespace EncoreTickets.SDK.Payment
{
    public interface IPaymentServiceApi : IServiceApi
    {
        /// <summary>
        /// Get details of an order by its channelId and externalId.
        /// </summary>
        /// <param name="channelId">Unique identifier of a website processing a payment.</param>
        /// <param name="externalId">Unique reference representing an order for.</param>
        /// <returns>Order details.</returns>
        /// <exception cref="ApiException">If request is invalid return 404 status code with error message.</exception>
        Order GetOrder(string channelId, string externalId);

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns>If request is correct return data about created order.</returns>
        /// <exception cref="ApiException">If request is invalid return 400 status code with error message.</exception>
        Order CreateOrder(CreateOrderRequest orderRequest);

        /// <summary>
        /// Update partially an order when this is possible, you cannot update an order that has a payment authorised, captured, refunded or partially_refunded.
        /// </summary>
        /// <param name="orderId">Order ID.</param>
        /// <param name="orderRequest">Request body can update billing address, shopper and/or line items. If nothing provided, nothing updated.</param>
        /// <returns>Return the current state of the updated order.</returns>
        /// <exception cref="ApiException">If request is invalid return 400 status code with error message.</exception>
        Order UpdateOrder(string orderId, UpdateOrderRequest orderRequest);

        /// <summary>
        /// Create a new payment for an order.
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns>Return the created payment.</returns>
        Models.Payment CreateNewPayment(CreatePaymentRequest paymentRequest);

        /// <summary>
        /// Get the list of all United-States states.
        /// </summary>
        /// <returns> List of United-states states.</returns>
        List<CountryTerritorialUnit> GetUsStates();

        /// <summary>
        /// Get the list of all Canadian provinces.
        /// </summary>
        /// <returns> List of Canadian provinces.</returns>
        List<CountryTerritorialUnit> GetCanadaProvinces();
    }
}
