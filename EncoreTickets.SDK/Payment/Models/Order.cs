using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// Order Entity.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Gets or sets order ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets time of the order creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets channel ID for this order.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets external ID for this order. This could be the booking reference and must be unique per channel.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets redirect URL.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets origin.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets billing address.
        /// </summary>
        public Address BillingAddress { get; set; }

        /// <summary>
        /// Gets or sets shopper.
        /// </summary>
        public Shopper Shopper { get; set; }

        /// <summary>
        /// Gets or sets payments.
        /// </summary>
        public List<Payment> Payments { get; set; }

        /// <summary>
        /// Gets or sets items.
        /// </summary>
        public List<OrderItem> Items { get; set; }

        /// <summary>
        /// Gets or sets risk data of the order.
        /// </summary>
        public RiskData RiskData { get; set; }
    }
}