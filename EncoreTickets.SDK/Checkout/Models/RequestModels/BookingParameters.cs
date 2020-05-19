namespace EncoreTickets.SDK.Checkout.Models.RequestModels
{
    /// <summary>
    /// Request to checkout service to create a new order
    /// </summary>
    public class BookingParameters : BookingCommonParameters
    {
        /// <summary>
        /// Gets or sets delivery method used in the booking.
        /// </summary>
        public DeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets payment type.
        /// </summary>
        public PaymentType PaymentType { get; set; }
    }
}
