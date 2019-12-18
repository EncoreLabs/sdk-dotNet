namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for API infos returned in a <see cref="Context"/>
    /// </summary>
    public class Info
    {
        /// <summary>
        /// Gets or sets easily read message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a code of the information.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a type of the information.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a name of the information.
        /// </summary>
        public string Name { get; set; }
    }
}