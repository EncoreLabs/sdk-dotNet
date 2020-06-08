namespace EncoreTickets.SDK.Utilities.CommonModels
{
    /// <summary>
    /// Interface for a common price model.
    /// </summary>
    public interface IPriceWithCurrency
    {
        /// <summary>
        /// Gets or sets the price value as an integer, including the decimal part.
        /// </summary>
        int? Value { get; set; }

        /// <summary>
        /// Gets or sets the number of decimal digits in the value.
        /// </summary>
        int? DecimalPlaces { get; set; }

        /// <summary>
        /// Gets or sets the price currency.
        /// </summary>
        string Currency { get; set; }
    }
}
