namespace EncoreTickets.SDK.Checkout.Models
{
    /// <summary>
    /// Shopper entity stores customer details like name and email address.
    /// </summary>
    public class Shopper
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets title of the shopper.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets firstname of the shopper.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets lastname of the shopper.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets telephone number of the shopper.
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets external Id for this order. This could be the booking reference and must be unique per channel.
        /// </summary>
        public string ExternalId { get; set; }
    }
}