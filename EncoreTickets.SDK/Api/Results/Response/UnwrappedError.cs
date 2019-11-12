namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for simple API errors.
    /// </summary>
    internal class UnwrappedError
    {
        public string code { get; set; }

        public string message { get; set; }
    }
}
