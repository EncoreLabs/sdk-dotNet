namespace EncoreTickets.SDK.Basket.Models
{
    /// <summary>
    /// Possible basket statuses.
    /// </summary>
    public enum BasketStatus
    {
        /// <summary>
        /// A basket is available.
        /// </summary>
        Active,

        /// <summary>
        /// A basket is confirmed.
        /// </summary>
        Confirmed,

        /// <summary>
        /// A basket is cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// A basket is expired.
        /// </summary>
        Expired
    }
}
