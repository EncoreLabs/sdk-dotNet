namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// Stores details of items in an order.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Gets or sets ID of the item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description of the item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets quantity of the item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets amount.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Gets or sets original amount of the item.
        /// </summary>
        public Amount AmountOriginal { get; set; }

        /// <summary>
        /// Gets or sets tax for the item.
        /// </summary>
        public Amount Tax { get; set; }

        /// <summary>
        /// Gets or sets external ID for this order. This could be the booking reference and must be unique per channel..
        /// </summary>
        public string ExternalId { get; set; }
    }
}