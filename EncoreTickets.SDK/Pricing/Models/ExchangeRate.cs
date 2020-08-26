using System;

namespace EncoreTickets.SDK.Pricing.Models
{
    /// <summary>
    /// Exchange rate DTO.
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Gets or sets an internal API ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a code of the source currency.
        /// </summary>
        public string BaseCurrency { get; set; }

        /// <summary>
        /// Gets or sets a code of the target currency.
        /// </summary>
        public string TargetCurrency { get; set; }

        /// <summary>
        /// Gets or sets the rate without margin.
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Gets or sets the rate with margin.
        /// encoreFxRate = fxRate * protection margin.
        /// </summary>
        public decimal EncoreRate { get; set; }

        /// <summary>
        /// Gets or sets the protection margin.
        /// This is in percentages.
        /// </summary>
        public int ProtectionMargin { get; set; }

        /// <summary>
        /// Gets or sets the time for the current exchange rate.
        /// </summary>
        public DateTimeOffset DatetimeOfSourcing { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public int Sourced { get; set; }
    }
}
