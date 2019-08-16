using System.Runtime.Serialization;
using RestSharp;

namespace EncoreTickets.SDK
{
    /// <summary>
    /// Class respresenting result of Api call
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    [DataContract]
    public class ApiResult<T> : ApiResultBase<T>
        where T : class
    {
        /// <summary>
        /// The data returned by the API response
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public ApiResult(ApiContext context, IRestRequest request, IRestResponse response, ApiResponse<T> data)
            : base(context, request, response)
        {
            if (response.IsSuccessful)
            {
                Data = data.Data as T;
            }
        }
    }
}
