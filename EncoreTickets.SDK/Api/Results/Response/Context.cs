using System.Collections.Generic;

namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for a response context returned in some API responses.
    /// </summary>
    public class Context
    {
        public List<Error> errors { get; set; }

        public List<Info> info { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
    }

    public class Info
    {
    }
}
