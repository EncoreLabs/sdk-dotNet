using EncoreTickets.SDK.Api.Context;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    /// <summary>
    /// Class representing result of Api call.
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    public class ApiResult<T> : ApiResultBase
        where T : class
    {
        /// <summary>
        /// The data returned by the API response.
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResult"/>
        /// </summary>
        /// <param name="context">Api context.</param>
        /// <param name="response">Response.</param>
        /// <param name="data">Response data.</param>
        public ApiResult(ApiContext context, IRestResponse response, ApiResponse<T> data)
            : base(context, response)
        {
            Data = response.IsSuccessful ? data.Data : null;
        }
    }
}
