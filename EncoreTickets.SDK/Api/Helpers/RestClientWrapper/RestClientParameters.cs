using System.Collections.Generic;

namespace EncoreTickets.SDK.Api.Helpers.RestClientWrapper
{
    internal class RestClientParameters
    {
        public string BaseUrl { get; set; }

        public string RequestUrl { get; set; }

        public Dictionary<string, string> RequestHeaders { get; set; }

        public Dictionary<string, string> RequestUrlSegments { get; set; }

        public Dictionary<string, string> RequestQueryParameters { get; set; }

        public object RequestBody { get; set; }

        public RequestMethod RequestMethod { get; set; }

        public RequestFormat RequestFormat { get; set; }

        public RestClientParameters()
        {
            RequestFormat = RequestFormat.Xml;
        }
    }
}