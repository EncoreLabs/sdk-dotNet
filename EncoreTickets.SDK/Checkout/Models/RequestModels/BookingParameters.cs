namespace EncoreTickets.SDK.Checkout.Models.RequestModels
{
    /// <summary>
    /// Request to checkout service to create a new order
    /// </summary>
    public class BookingParameters
    {
        /// <summary>
        /// Gets or sets basket reference ID/no.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets channel ID for this order.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets shopper entity.
        /// </summary>
        public Shopper Shopper { get; set; }

        /// <summary>
        /// Gets or sets billing address.
        /// </summary>
        public Address BillingAddress { get; set; }

        /// <summary>
        /// Gets or sets URL for the origin.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets redirect URL.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets delivery method used in the booking.
        /// </summary>
        public DeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets delivery charge that will be added for the delivery.
        /// </summary>
        public int DeliveryCharge { get; set; }

        /// <summary>
        /// Gets or sets message that will be added in the gift voucher when delivered to some one else.
        /// </summary>
        public int GiftVoucherMessage { get; set; }

        /// <summary>
        /// Gets or sets delivery address.
        /// </summary>
        public Address DeliveryAddress { get; set; }

        /// <summary>
        /// Gets or sets whether the user selected flexi tickets during booking.
        /// </summary>
        public bool HasFlexiTickets { get; set; }

        /// <summary>
        /// Gets or sets payment type.
        /// </summary>
        public PaymentType PaymentType { get; set; }
    }
}
