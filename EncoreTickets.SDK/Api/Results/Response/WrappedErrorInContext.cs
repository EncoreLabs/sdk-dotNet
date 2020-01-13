namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The model for failed API responses when an empty string is returned in the response section instead of null object.
    /// </summary>
    /// <inheritdoc/>
    internal class WrappedErrorInContext : ApiResponse<object>
    {
    }
}