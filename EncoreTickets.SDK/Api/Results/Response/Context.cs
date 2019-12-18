using System.Collections.Generic;

namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for a response context returned in some API responses.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Gets or sets an errors collection.
        /// </summary>
        public List<Error> Errors { get; set; }

        /// <summary>
        /// Gets or sets an infos collection.
        /// </summary>
        public List<Info> Info { get; set; }
    }
}
