namespace EncoreTickets.SDK.Checkout.Models
{
    /// <summary>
    /// Address entity stores an address with country code.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Gets or sets address line 1.
        /// </summary>
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets address line 2.
        /// </summary>
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets post code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets country code in ISO format.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets country name.
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets state for United-States or Province for Canada. Format is 2 letters abbreviation..
        /// </summary>
        public string StateOrProvince { get; set; }
    }
}