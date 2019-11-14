﻿using System.Collections.Generic;
using System.Linq;
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
        public Response.Context ResponseContext { get; set; }

        /// <summary>
        /// Gets or sets the request returned in the API response.
        /// </summary>
        public Request RequestInResponse { get; set; }

        /// <summary>
        /// Gets or sets the API exception.
        /// </summary>
        public ApiException ApiException { get; set; }

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

        /// <summary>
        /// Gets <c>data</c> if the API request was successful and response context does not have warnings, <see cref="T"/>;
        /// otherwise, <c> throws the API context exception</c>, <see cref="ContextApiException"/>;.
        /// </summary>
        /// <param name="codesOfInfosAsErrors">Information codes in the context of the response, which are errors.</param>
        /// <returns>Data</returns>
        public T GetDataOrContextException(IEnumerable<string> codesOfInfosAsErrors)
        {
            var data = DataOrException;
            if (ResponseContext == null)
            {
                return data;
            }

            var exception = new ContextApiException(codesOfInfosAsErrors, RestResponse, Context, ResponseContext,
                RequestInResponse);
            return exception.Errors.Any() ? throw exception : data;
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
    }
}
