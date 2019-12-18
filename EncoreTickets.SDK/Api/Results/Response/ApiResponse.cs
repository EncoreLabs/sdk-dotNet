namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The common API response for wrapped data.
    /// </summary>
    /// <typeparam name="T">The type of requested data.</typeparam>
    /// <inheritdoc/>
    internal class ApiResponse<T> : BaseWrappedApiResponse<T, T>
        where T : class
    {
        /// <inheritdoc/>>
        public override T Data => Response;
    }
}