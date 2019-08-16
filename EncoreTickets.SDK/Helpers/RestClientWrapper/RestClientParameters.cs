using System.Collections.Generic;

namespace EncoreTickets.SDK.Helpers.RestClientWrapper
{
    internal class RestClientParameters
    {
        public string RequestUrl { get; set; }

        public Dictionary<string, string> RequestParams { get; set; }

        public object RequestBody { get; set; }

        public Dictionary<string, string> RequestHeaders { get; set; }

        public RequestMethod RequestMethod { get; set; }

        public RequestFormat RequestFormat { get; set; }

        public RestClientParameters()
        {
            RequestFormat = RequestFormat.Xml;
        }
    }
}