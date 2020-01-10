namespace EncoreTickets.SDK.Api.Models
{
    public class PageRequest
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Gets or sets a number of results you want per page.
        /// </summary>
        public int? Limit { get; set; }
    }
}