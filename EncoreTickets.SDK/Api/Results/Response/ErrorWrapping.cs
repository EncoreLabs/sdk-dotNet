namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The enum to determine the wrapper format of the returned errors from the API.
    /// </summary>
    public enum ErrorWrapping
    {
        MessageWithCode,
        Errors,
        Context,
        NotParsedContent,
    }
}
