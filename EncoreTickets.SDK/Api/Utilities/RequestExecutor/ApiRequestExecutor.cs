using System;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RestClientBuilder;
using EncoreTickets.SDK.Utilities.Serializers;
using RestSharp;
using RestSharp.Deserializers;

namespace EncoreTickets.SDK.Api.Utilities.RequestExecutor
{
    /// <summary>
    /// API requests executor.
    /// </summary>
    public class ApiRequestExecutor
    {
        private static readonly IDeserializer SourceDeserializer = new DefaultJsonSerializer();

        private readonly IApiRestClientBuilder restClientBuilder;

        public string BaseUrl { get; }

        public ApiContext Context { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiRequestExecutor"/>
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        /// <param name="baseUrl">The site URL.</param>
        /// <param name="clientBuilder">The builder for objects that initialize RestSharp requests</param>
        public ApiRequestExecutor(ApiContext context, string baseUrl, IApiRestClientBuilder clientBuilder)
        {
            Context = context;
            BaseUrl= baseUrl;
            restClientBuilder = clientBuilder;
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should not be wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <param name="requestParameters">Parameters for initializing an API request</param>
        /// <param name="wrappedError"><c>true</c> if possible API exception should be wrapped with extra data on API side, <see cref="ApiResponse{T}"/>; otherwise, <c>false</c>.</param>
        /// <returns>Result of request execution.</returns>
        public ApiResult<T> ExecuteApiWithNotWrappedResponse<T>(ExecuteApiRequestParameters requestParameters, bool wrappedError = false)
            where T : class, new()
        {
            var restResponse = GetRestResponse<T>(requestParameters);
            return CreateApiResult(restResponse, wrappedError);
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should be standard wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <param name="requestParameters">Parameters for initializing an API request</param>
        /// <param name="wrappedError"><c>true</c> if possible API exception should be wrapped with extra data on API side, <see cref="ApiResponse{T}"/>; otherwise, <c>false</c>.</param>
        /// <returns>Result of request execution.</returns>
        public ApiResult<T> ExecuteApiWithWrappedResponse<T>(ExecuteApiRequestParameters requestParameters, bool wrappedError = true)
            where T : class
        {
            var restWrappedResponse = GetRestResponse<ApiResponse<T>>(requestParameters);
            return CreateApiResult<T, ApiResponse<T>, T>(restWrappedResponse, wrappedError);
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should be non-standard wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <typeparam name="TApiResponse">Type of the response object.</typeparam>
        /// <typeparam name="TResponse">The type of data in a "response" section of the response object.</typeparam>
        /// <param name="requestParameters">Parameters for initializing an API request</param>
        /// <param name="wrappedError"><c>true</c> if possible API exception should be wrapped with extra data on API side, <see cref="ApiResponse{T}"/>; otherwise, <c>false</c>.</param>
        /// <returns>Result of request execution.</returns>
        public ApiResult<T> ExecuteApiWithWrappedResponse<T, TApiResponse, TResponse>(ExecuteApiRequestParameters requestParameters, bool wrappedError = true)
            where T : class
            where TApiResponse : BaseWrappedApiResponse<TResponse, T>, new()
            where TResponse : class
        {
            var restWrappedResponse = GetRestResponse<TApiResponse>(requestParameters);
            return CreateApiResult<T, TApiResponse, TResponse>(restWrappedResponse, wrappedError);
        }

        private IRestResponse<T> GetRestResponse<T>(ExecuteApiRequestParameters requestParameters)
            where T : class, new()
        {
            var clientWrapper = restClientBuilder.CreateClientWrapper(Context);
            var clientParameters = restClientBuilder.CreateClientWrapperParameters(Context, BaseUrl, requestParameters);
            var client = clientWrapper.GetRestClient(clientParameters);
            var request = clientWrapper.GetRestRequest(clientParameters);
            var response = clientWrapper.Execute<T>(client, request);
            return response;
        }

        private ApiResult<T> CreateApiResult<T>(IRestResponse<T> restResponse, bool wrappedError)
            where T : class
        {
            return !restResponse.IsSuccessful
                ? CreateApiResultForError<T>(restResponse, wrappedError)
                : new ApiResult<T>(restResponse.Data, restResponse, Context);
        }

        private ApiResult<T> CreateApiResult<T, TApiResponse, TResponse>(
            IRestResponse<TApiResponse> restWrappedResponse, bool wrappedError)
            where T : class
            where TResponse : class
            where TApiResponse : BaseWrappedApiResponse<TResponse, T>, new()
        {
            if (!restWrappedResponse.IsSuccessful && restWrappedResponse.Data?.Context == null)
            {
                return CreateApiResultForError<T>(restWrappedResponse, wrappedError);
            }

            var data = restWrappedResponse.Data;
            return new ApiResult<T>(data?.Data, restWrappedResponse, Context, data?.Context, data?.Request);
        }

        private ApiResult<T> CreateApiResultForError<T>(IRestResponse restResponse, bool wrappedError)
            where T : class
        {
            if (wrappedError)
            {
                var errorData = DeserializeResponse<WrappedError>(restResponse);
                return new ApiResult<T>(default, restResponse, Context, errorData?.Context, errorData?.Request);
            }

            var apiError = DeserializeResponse<UnwrappedError>(restResponse);
            return new ApiResult<T>(default, restResponse, Context, apiError?.Message);
        }

        private static T DeserializeResponse<T>(IRestResponse response)
        {
            try
            {
                return SourceDeserializer.Deserialize<T>(response);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
