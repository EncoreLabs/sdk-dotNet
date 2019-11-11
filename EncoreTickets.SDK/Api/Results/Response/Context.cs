using System.Collections.Generic;

namespace EncoreTickets.SDK.Api.Results.Response
{
    public class Context
    {
        public List<Error> errors { get; set; }

        public List<Info> info { get; set; }
    }
}
