using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results.Response;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    /// <summary>
    /// Class representing result of Api call.
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    public class ApiResult<T>
        where T : class
    {
        private T apiData;

        /// <summary>
        /// Gets a value indicating whether this call was a success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool IsSuccessful => RestResponse.IsSuccessful;

        public T DataOrException => IsSuccessful ? apiData : throw Exception;

        public T DataOrDefault => IsSuccessful ? apiData : default;

        /// <summary>
        /// Gets a context object for which the request was made.
        /// </summary>
        public ApiContext Context { get; set; }

        public IRestResponse RestResponse{ get; set; }

        public Response.Context ResponseContext { get; set; }

        public Request RequestInResponse { get; set; }

        public ApiException Exception { get; set; }

        public ApiResult(T data, IRestResponse response, ApiContext context, Response.Context responseContext,
            Request requestInResponse)
        {
            ResponseContext = responseContext;
            RequestInResponse = requestInResponse;
            InitializeCommonParameters(data, response, context);
        }

        public ApiResult(T data, IRestResponse response, ApiContext context, UnwrappedError error)
        {
            ResponseContext = error != null
                ? new Response.Context { errors = new List<Error> { new Error { message = error.message } } }
                : null;
            InitializeCommonParameters(data, response, context);
        }

        public ApiResult(T data, IRestResponse response, ApiContext context)
        {
            InitializeCommonParameters(data, response, context);
        }

        private void InitializeCommonParameters(T data, IRestResponse response, ApiContext context)
        {
            apiData = data;
            RestResponse = response;
            Context = context;
            Exception = IsSuccessful
                ? null
                : new ApiException(RestResponse, Context, ResponseContext, RequestInResponse);
        }
    }
}
