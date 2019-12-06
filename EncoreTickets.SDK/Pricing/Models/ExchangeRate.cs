using System;

namespace EncoreTickets.SDK.Pricing.Models
{
    /// <summary>
    /// Exchange rate DTO
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Gets or sets an internal API ID.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Gets or sets a code of the source currency.
        /// </summary>
        public string baseCurrency { get; set; }

        /// <summary>
        /// Gets or sets a code of the target currency.
        /// </summary>
        public string targetCurrency { get; set; }

        /// <summary>
        /// Gets or sets the rate without margin.
        /// </summary>
        public decimal rate { get; set; }

        /// <summary>
        /// Gets or sets the rate with margin.
        /// encoreFxRate = fxRate * protection margin
        /// </summary>
        public decimal encoreRate { get; set; }

        /// <summary>
        /// Gets or sets the protection margin.
        /// This is in percentages.
        /// </summary>
        public int protectionMargin { get; set; }

        /// <summary>
        /// Gets or sets the time for the current exchange rate.
        /// </summary>
        public DateTime datetimeOfSourcing { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime updatedAt { get; set; }

        public int sourced { get; set; }
    }
}
