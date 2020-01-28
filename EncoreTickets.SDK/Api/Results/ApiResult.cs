using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
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
        /// Gets <c>data</c> if the API request was successful, <see cref="T"/>; otherwise, <c> throws the API exception</c>, <see cref="ApiException"/>;.
        /// </summary>
        public T DataOrException => IsSuccessful ? apiData : throw ApiException;

        /// <summary>
        /// Gets <c>data</c> if the API request was successful, <see cref="T"/>; otherwise, <c> default</c>.
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
        public Context ResponseContext { get; set; }

        /// <summary>
        /// Gets or sets the request returned in the API response.
        /// </summary>
        public Request RequestInResponse { get; set; }

        /// <summary>
        /// Gets or sets the API exception.
        /// </summary>
        public ApiException ApiException { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResult{T}"/>
        /// <typeparam name="T">Type of expected data.</typeparam>
        /// </summary>
        public ApiResult(T data, IRestResponse response, ApiContext context, Context responseContext,
            Request requestInResponse)
        {
            ResponseContext = responseContext;
            RequestInResponse = requestInResponse;
            InitializeCommonParameters(data, response, context);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResult{T}"/>
        /// <typeparam name="T">Type of expected data.</typeparam>
        /// </summary>
        public ApiResult(T data, IRestResponse response, ApiContext context, IEnumerable<Error> errors)
        {
            ResponseContext = errors != null
                ? new Context {Errors = new List<Error>(errors)}
                : null;
            InitializeCommonParameters(data, response, context);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResult{T}"/>
        /// <typeparam name="T">Type of expected data.</typeparam>
        /// </summary>
        public ApiResult(T data, IRestResponse response, ApiContext context, string error)
        {
            ResponseContext = error != null
                ? new Context { Errors = new List<Error> { new Error { Message = error } } }
                : null;
            InitializeCommonParameters(data, response, context);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResult{T}"/>
        /// <typeparam name="T">Type of expected data.</typeparam>
        /// </summary>
        public ApiResult(T data, IRestResponse response, ApiContext context)
        {
            InitializeCommonParameters(data, response, context);
        }

        /// <summary>
        /// Gets <c>data</c> if the API request was successful and response context does not have warnings, <see cref="T"/>;
        /// otherwise, <c> throws the API context exception</c>, <see cref="ContextApiException"/>;.
        /// </summary>
        /// <param name="codeOfInfoAsError">Information code in the context of the response, which is an error.</param>
        /// <returns>Data</returns>
        public T GetDataOrContextException(string codeOfInfoAsError)
        {
            return GetDataOrContextException(new[] {codeOfInfoAsError});
        }

        private void InitializeCommonParameters(T data, IRestResponse response, ApiContext context)
        {
            apiData = data;
            RestResponse = response;
            Context = context;
            ApiException = IsSuccessful
                ? null
                : new ApiException(RestResponse, Context, ResponseContext, RequestInResponse);
        }

        private T GetDataOrContextException(IEnumerable<string> codesOfInfosAsErrors)
        {
            var data = DataOrException;
            if (ResponseContext?.Info == null)
            {
                return data;
            }

            var infosAsErrors = ResponseContext.Info.Where(x => codesOfInfosAsErrors.Contains(x.Code)).ToList();
            if (!infosAsErrors.Any())
            {
                return data;
            }

            throw new ContextApiException(infosAsErrors, RestResponse, Context, ResponseContext, RequestInResponse);
        }
    }
}
