using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Utilities.Enums;
using RestSharp;

namespace EncoreTickets.SDK.Api.Helpers
{
    /// <summary>
    /// API requests executor.
    /// </summary>
    public class ApiRequestExecutor
    {
        public ApiContext Context { get; }

        public string BaseUrl { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiRequestExecutor"/>
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        /// <param name="baseUrl">The site URL.</param>
        public ApiRequestExecutor(ApiContext context, string baseUrl)
        {
            Context = context;
            BaseUrl = baseUrl;
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should not be wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <param name="endpoint">API resource endpoint.</param>
        /// <param name="method">Request method.</param>
        /// <param name="body">Request body.</param>
        /// <param name="query">Object for request query.</param>
        /// <param name="dateFormat">Request date format.</param>
        /// <param name="wrappedError"><c>true</c> if possible API exception should be wrapped with extra data on API side, <see cref="ApiResponse{T}"/>; otherwise, <c>false</c>.</param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApiWithNotWrappedResponse<T>(
            string endpoint,
            RequestMethod method,
            object body = null,
            object query = null,
            string dateFormat = null,
            bool wrappedError = false)
            where T : class, new()
        {
            var restResponse = GetRestResponse<T>(endpoint, method, body, query, dateFormat);
            return CreateApiResult(restResponse, wrappedError);
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should be standard wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <param name="endpoint">API resource endpoint.</param>
        /// <param name="method">Request method.</param>
        /// <param name="body">Request body.</param>
        /// <param name="query">Object for request query.</param>
        /// <param name="dateFormat">Request date format.</param>
        /// <param name="wrappedError"><c>true</c> if possible API exception should be wrapped with extra data on API side, <see cref="ApiResponse{T}"/>; otherwise, <c>false</c>.</param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApiWithWrappedResponse<T>(
            string endpoint,
            RequestMethod method,
            object body = null,
            object query = null,
            string dateFormat = null,
            bool wrappedError = true)
            where T : class
        {
            var restWrappedResponse = GetRestResponse<ApiResponse<T>>(endpoint, method, body, query, dateFormat);
            return CreateApiResult<T, ApiResponse<T>, T>(restWrappedResponse, wrappedError);
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should be non-standard wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <typeparam name="TApiResponse">Type of the response object.</typeparam>
        /// <typeparam name="TResponse">The type of data in a "response" section of the response object.</typeparam>
        /// <param name="endpoint">API resource endpoint.</param>
        /// <param name="method">Request method.</param>
        /// <param name="body">Request body.</param>
        /// <param name="query">Object for request query.</param>
        /// <param name="dateFormat">Request date format.</param>
        /// <param name="wrappedError"><c>true</c> if possible API exception should be wrapped with extra data on API side, <see cref="ApiResponse{T}"/>; otherwise, <c>false</c>.</param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApiWithWrappedResponse<T, TApiResponse, TResponse>(
            string endpoint,
            RequestMethod method,
            object body = null,
            object query = null,
            string dateFormat = null,
            bool wrappedError = true)
            where T : class
            where TResponse : class
            where TApiResponse : BaseWrappedApiResponse<TResponse, T>, new()
        {
            var restWrappedResponse = GetRestResponse<TApiResponse>(endpoint, method, body, query, dateFormat);
            return CreateApiResult<T, TApiResponse, TResponse>(restWrappedResponse, wrappedError);
        }

        private IRestResponse<T> GetRestResponse<T>(
            string endpoint,
            RequestMethod method,
            object body,
            object query,
            string dateFormat)
            where T : class, new()
        {
            var clientWrapper = ApiClientWrapperBuilder.CreateClientWrapper(Context);
            var parameters = ApiClientWrapperBuilder.CreateClientWrapperParameters(Context, BaseUrl, endpoint, method,
                body, query, dateFormat);
            var client = clientWrapper.GetRestClient(parameters);
            var request = clientWrapper.GetRestRequest(parameters);
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
            if (!restWrappedResponse.IsSuccessful && restWrappedResponse.Data?.context == null)
            {
                return CreateApiResultForError<T>(restWrappedResponse, wrappedError);
            }

            var data = restWrappedResponse.Data;
            return new ApiResult<T>(data?.Data, restWrappedResponse, Context, data?.context, data?.request);
        }

        private ApiResult<T> CreateApiResultForError<T>(IRestResponse restResponse, bool wrappedError)
            where T : class
        {
            if (wrappedError)
            {
                var errorData = DeserializeResponse<WrappedError>(restResponse);
                return new ApiResult<T>(default, restResponse, Context, errorData?.context, errorData?.request);
            }

            var apiError = DeserializeResponse<UnwrappedError>(restResponse);
            return new ApiResult<T>(default, restResponse, Context, apiError?.message);
        }

        private T DeserializeResponse<T>(IRestResponse response)
        {
            try
            {
                return SimpleJson.DeserializeObject<T>(response.Content);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
