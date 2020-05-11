namespace EncoreTickets.SDK.Checkout.Models.RequestModels
{
    public class ConfirmBookingParameters
    {
        /// <summary>
        /// Gets or sets channel ID for this order.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets payment ID.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Gets or sets agent payment reference.
        /// </summary>
        public string AgentPaymentReference { get; set; }
    }
}