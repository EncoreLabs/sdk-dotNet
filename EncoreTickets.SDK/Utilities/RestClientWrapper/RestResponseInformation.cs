using System.Collections.Generic;

namespace EncoreTickets.SDK.Utilities.RestClientWrapper
{
    /// <summary>
    /// Parameters to get information about response in <see cref="RestClientWrapper"/>
    /// </summary>
    public class RestResponseInformation
    {
        /// <summary>
        /// Gets or sets response headers: key - header name; value - header value.
        /// </summary>
        public Dictionary<string, object> ResponseHeaders { get; set; }
    }
}