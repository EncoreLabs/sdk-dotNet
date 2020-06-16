namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// Stores amount and currency details.
    /// </summary>
    public class Amount
    {
        /// <summary>
        /// Gets or sets amount value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets currency in 3 letter ISO code.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets exchange rate for a currency is different from GBP.
        /// </summary>
        public double ExchangeRate { get; set; }
    }
}