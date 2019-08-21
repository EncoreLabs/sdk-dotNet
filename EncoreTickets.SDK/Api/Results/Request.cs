namespace EncoreTickets.SDK.Api.Results
{
    /// <summary>
    /// The model for a request returned in some API responses.
    /// </summary>
    public class Request
    {
        public string body { get; set; }

        public Query query { get; set; }

        public UrlParams urlParams { get; set; }
    }
}
