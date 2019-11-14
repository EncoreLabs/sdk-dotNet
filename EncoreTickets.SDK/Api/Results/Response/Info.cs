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
        public string message { get; set; }

        /// <summary>
        /// Gets or sets a code of the information.
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Gets or sets a type of the information.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Gets or sets a name of the information.
        /// </summary>
        public string name { get; set; }
    }
}