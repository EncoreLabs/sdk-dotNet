namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for API errors returned in a <see cref="Context"/>
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets or sets easily read message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a code of the error.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a name of field that is a cause of the error.
        /// </summary>
        public string Field { get; set; }
    }
}