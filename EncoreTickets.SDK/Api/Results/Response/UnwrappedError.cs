namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for simple API errors.
    /// </summary>
    internal class UnwrappedError
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
