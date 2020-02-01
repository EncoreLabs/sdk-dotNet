using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Payment.Models;
using EncoreTickets.SDK.Payment.Models.RequestModels;

namespace EncoreTickets.SDK.Payment
{
    public interface IPaymentServiceApi
    {
        /// <summary>
        /// Get details of an order by its channelId and externalId.
        /// </summary>
        /// <param name="channelId">Unique identifier of a website processing a payment</param>
        /// <param name="externalId">Unique reference representing an order for</param>
        /// <returns>Order details</returns>
        /// <exception cref="ApiException">If request is invalid return 404 status code with error message.</exception>
        Order GetOrder(string channelId, string externalId);

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns>If request is correct return data about created order.</returns>
        /// <exception cref="ApiException">If request is invalid return 400 status code with error message.</exception>
        Order CreateOrder(CreateOrderRequest orderRequest);
    }
}
