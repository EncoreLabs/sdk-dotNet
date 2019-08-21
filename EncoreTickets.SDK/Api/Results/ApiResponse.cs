namespace EncoreTickets.SDK.Api.Results
{
    /// <summary>
    /// The API response for data wrapped with extra information.
    /// </summary>
    /// <typeparam name="T">The type of wrapped data.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Gets unwrapped response data.
        /// </summary>
        public T Data => response;

        /// <summary>
        /// Gets or sets response.
        /// </summary>
        /// <typeparam name="T">The type of wrapped data.</typeparam>
        public T response { get; set; }

        /// <summary>
        /// Gets or sets request.
        /// </summary>
        public Request request { get; set; }

        /// <summary>
        /// Gets or sets the response context.
        /// </summary>
        public object context { get; set; }

        /// <summary>
        /// Empty constructor to create a new instance of <see cref="ApiResponse"/>
        /// </summary>
        public ApiResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="data">The wrapped data.</param>
        /// <typeparam name="T">The type of wrapped data.</typeparam>
        public ApiResponse(T data)
        {
            response = data;
        }
    }
}