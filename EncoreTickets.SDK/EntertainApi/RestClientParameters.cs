using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi
{
    public class RestClientParameters
    {
        public RestClientParameters()
        {
            RequestFormat = RequestFormat.Xml;
        }

        public string RequestUrl { get; set; }
        public Dictionary<string, string> RequestParams { get; set; }
        public object RequestBody { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
        public RequestMethod RequestMethod { get; set; }
        public RequestFormat RequestFormat { get; set; }
    }

    public enum RequestMethod { Get, Post, Put, Delete }
    public enum RequestFormat { Xml, Json }
}