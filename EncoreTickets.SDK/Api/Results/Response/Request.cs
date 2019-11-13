namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for a request returned in some API responses.
    /// </summary>
    public class Request
    {
        public string body { get; set; }

        public dynamic query { get; set; }

        public dynamic urlParams { get; set; }
    }
}
