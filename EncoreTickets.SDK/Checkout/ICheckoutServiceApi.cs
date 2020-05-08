using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Checkout.Models;
using EncoreTickets.SDK.Checkout.Models.RequestModels;

namespace EncoreTickets.SDK.Checkout
{
    /// <summary>
    /// The interface of a checkout service.
    /// </summary>
    public interface ICheckoutServiceApi : IServiceApi
    {
        /// <summary>
        /// Creates an order from basketReference and channelId
        /// </summary>
        /// <param name="bookingParameters">Parameters to create a new order</param>
        /// <returns>Payment information</returns>
        PaymentInfo Checkout(BookingParameters bookingParameters);
    }
}
