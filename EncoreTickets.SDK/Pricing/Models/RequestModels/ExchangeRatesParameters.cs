namespace EncoreTickets.SDK.Pricing.Models.RequestModels
{
    /// <summary>
    /// The helper entity for collecting parameters for obtaining exchange rates.
    /// </summary>
    public class ExchangeRatesParameters
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Gets or sets a number of results you want per page.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets ASC or DESC.
        /// Default value : DESC
        /// </summary>
        public Direction? Direction { get; set; }

        /// <summary>
        /// Gets or sets sort by.
        /// Default value : datetimeOfSourcing
        /// </summary>
        public string Sort { get; set; }
    }
}
