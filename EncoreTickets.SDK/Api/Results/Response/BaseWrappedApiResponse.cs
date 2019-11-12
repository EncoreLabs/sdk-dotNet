namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The API response for data wrapped with extra information.
    /// </summary>
    /// <typeparam name="TResponse">The type of wrapped response data.</typeparam>
    /// <typeparam name="TData">The type of requested data.</typeparam>
    public abstract class BaseWrappedApiResponse<TResponse, TData>
        where TResponse : class
        where TData : class
    {
        /// <summary>
        /// Gets unwrapped response data.
        /// </summary>
        public abstract TData Data { get; }

        /// <summary>
        /// Gets or sets request.
        /// </summary>
        public Request request { get; set; }

        /// <summary>
        /// Gets or sets response.
        /// </summary>
        public TResponse response { get; set; }

        /// <summary>
        /// Gets or sets the response context.
        /// </summary>
        public Context context { get; set; }
    }
}