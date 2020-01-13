using System.Collections.Generic;

namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for failed API responses when only errors return
    /// </summary>
    internal class WrappedError
    {
        /// <summary>
        /// Gets or sets an errors collection.
        /// </summary>
        public List<Error> Errors { get; set; }
    }
}