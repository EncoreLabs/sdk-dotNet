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

        /// <summary>
        /// Gets <c>data</c> if the API request was successful, <see cref="ApiResponse{T}"/>; otherwise, <c> throws the API exception</c>.
        /// </summary>
        public T DataOrException => IsSuccessful ? apiData : throw Exception;

        /// <summary>
        /// Gets <c>data</c> if the API request was successful, <see cref="ApiResponse{T}"/>; otherwise, <c> default</c>.
        /// </summary>
        public T DataOrDefault => IsSuccessful ? apiData : default;

        /// <summary>
        /// Gets or sets a context object for which the request was made.
        /// </summary>
        public ApiContext Context { get; set; }

        /// <summary>
        /// Gets or sets HTTP response.
        /// </summary>
        public IRestResponse RestResponse{ get; set; }

        /// <summary>
        /// Gets or sets the context returned in the API response.
        /// </summary>
        public Response.Context ResponseContext { get; set; }

        /// <summary>
        /// Gets or sets the request returned in the API response.
        /// </summary>
        public Request RequestInResponse { get; set; }

        /// <summary>
        /// Gets or sets the API exception.
        /// </summary>
        public ApiException Exception { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResult"/>
        /// <typeparam name="T">Type of expected data.</typeparam>
        /// </summary>
        public ApiResult(T data, IRestResponse response, ApiContext context, Response.Context responseContext,
            Request requestInResponse)
        {
            ResponseContext = responseContext;
            RequestInResponse = requestInResponse;
            InitializeCommonParameters(data, response, context);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResult"/>
        /// <typeparam name="T">Type of expected data.</typeparam>
        /// </summary>
        public ApiResult(T data, IRestResponse response, ApiContext context, string error)
        {
            ResponseContext = error != null
                ? new Response.Context { errors = new List<Error> { new Error { message = error } } }
                : null;
            InitializeCommonParameters(data, response, context);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResult"/>
        /// <typeparam name="T">Type of expected data.</typeparam>
        /// </summary>
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
