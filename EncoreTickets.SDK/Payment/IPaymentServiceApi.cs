using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Payment.Models;
using EncoreTickets.SDK.Payment.Models.RequestModels;

namespace EncoreTickets.SDK.Payment
{
    public interface IPaymentServiceApi
    {
        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns>If request is correct return data about created order.</returns>
        /// <exception cref="ApiException">If request is invalid return 400 status code with error message.</exception>
        Order CreateOrder(CreateOrderRequest orderRequest);
    }
}
