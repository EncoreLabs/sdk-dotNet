namespace EncoreTickets.SDK.Basket.Models
{
    /// <summary>
    /// Basket delivery option.
    /// </summary>
    public class Delivery
    {
        /// <summary>
        /// Gets or sets a delivery method for a basket.
        /// </summary>
        public DeliveryMethod Method { get; set; }

        /// <summary>
        /// Gets or sets a delivery price.
        /// </summary>
        public Price Charge { get; set; }
    }
}
