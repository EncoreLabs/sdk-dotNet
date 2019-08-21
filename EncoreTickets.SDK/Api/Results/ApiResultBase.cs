using EncoreTickets.SDK.Api.Context;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    /// <summary>
    /// The base class for providing the result of a executes request.
    /// </summary>
    public abstract class ApiResultBase
    {
        /// <summary>
        /// Gets a value indicating whether this call was a success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Result { get; }

        /// <summary>
        /// Gets a context object for which the request was made.
        /// </summary>
        public ApiContext Context { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResultBase"/>
        /// </summary>
        /// <param name="context">Api context.</param>
        /// <param name="response">Response.</param>
        protected ApiResultBase(ApiContext context, IRestResponse response)
        {
            Context = context;
            Result = response.IsSuccessful;
        }
    }
}
