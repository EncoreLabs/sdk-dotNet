namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for a request returned in some API responses.
    /// </summary>
    public class Request
    {
        public string Body { get; set; }

        public dynamic Query { get; set; }

        public dynamic UrlParams { get; set; }
    }
}
