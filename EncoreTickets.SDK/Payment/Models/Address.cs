﻿namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// Address entity stores an address with country code.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Gets or sets Address Line 1.
        /// </summary>
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets Address Line 2.
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
        /// Gets or sets country code in legacy format.
        /// </summary>
        public string LegacyCountryCode { get; set; }

        /// <summary>
        /// Gets or sets USA state or Canadian province abbreviation, it should be a 2 characters string.
        /// </summary>
        public string StateOrProvince { get; set; }
    }
}