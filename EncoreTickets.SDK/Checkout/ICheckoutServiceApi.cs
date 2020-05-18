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

        /// <summary>
        /// Confirms whether a booking is successful.
        /// </summary>
        /// <param name="bookingReference">Basket reference ID/no.</param>
        /// <param name="bookingParameters">Parameters for confirmation</param>
        /// <returns>True if the confirmation is successful.</returns>
        bool ConfirmBooking(string bookingReference, ConfirmBookingParameters bookingParameters);

        /// <summary>
        /// Confirms by agent whether an agent booking is successful.
        /// </summary>
        /// <param name="agentId">Id of an agent who confirms the booking.</param>
        /// <param name="agentPassword">Password of an agent who confirms the booking.</param>
        /// <param name="bookingReference">Basket reference ID/no.</param>
        /// <param name="bookingParameters">Parameters for confirmation</param>
        /// <returns>True if the confirmation is successful.</returns>
        bool ConfirmBooking(string agentId, string agentPassword, string bookingReference, ConfirmBookingParameters bookingParameters);
    }
}
