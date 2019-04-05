using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK
{
    public class Query
    {
    }

    public class UrlParams
    {
        public string searchTerm { get; set; }
    }

    public class Request
    {
        public string body { get; set; }
        public Query query { get; set; }
        public UrlParams urlParams { get; set; }
    }

}
