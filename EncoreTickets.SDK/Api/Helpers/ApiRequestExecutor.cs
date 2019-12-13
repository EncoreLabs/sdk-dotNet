using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Utilities.Common.Serializers;
using EncoreTickets.SDK.Utilities.Enums;
using RestSharp;

namespace EncoreTickets.SDK.Api.Helpers
{
    /// <summary>
    /// API requests executor.
    /// </summary>
    public class ApiRequestExecutor
    {
        private readonly ApiContext context;
        private readonly string baseUrl;

        /// <summary>
        /// Initializes a new instance of <see cref="ApiRequestExecutor"/>
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        /// <param name="baseUrl">The site URL.</param>
        public ApiRequestExecutor(ApiContext context, string baseUrl)
        {
            this.context = context;
            this.baseUrl = baseUrl;
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
        /// <param name="serializer">Serializer to use in the request.</param>
        /// <param name="deserializer">Deserializer to use in the response.</param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApiWithNotWrappedResponse<T>(
            string endpoint,
            RequestMethod method,
            object body = null,
            object query = null,
            string dateFormat = null,
            bool wrappedError = false,
            ISerializerWithDateFormat serializer = null,
            IDeserializerWithDateFormat deserializer = null)
            where T : class, new()
        {
            var restResponse = GetRestResponse<T>(endpoint, method, body, query, dateFormat, serializer, deserializer);
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
        /// <param name="serializer">Serializer to be used in the request.</param>
        /// <param name="deserializer">Deserializer to be used in the response.</param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApiWithWrappedResponse<T>(
            string endpoint,
            RequestMethod method,
            object body = null,
            object query = null,
            string dateFormat = null,
            bool wrappedError = true,
            ISerializerWithDateFormat serializer = null,
            IDeserializerWithDateFormat deserializer = null)
            where T : class
        {
            var restWrappedResponse = GetRestResponse<ApiResponse<T>>(endpoint, method, body, query, dateFormat, serializer, deserializer);
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
        /// <param name="serializer">JsonSerializer for a request.</param>
        /// <param name="deserializer">JsonDeserializer for a response.</param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApiWithWrappedResponse<T, TApiResponse, TResponse>(
            string endpoint,
            RequestMethod method,
            object body = null,
            object query = null,
            string dateFormat = null,
            bool wrappedError = true,
            ISerializerWithDateFormat serializer = null,
            IDeserializerWithDateFormat deserializer = null)
            where T : class
            where TResponse : class
            where TApiResponse : BaseWrappedApiResponse<TResponse, T>, new()
        {
            var restWrappedResponse = GetRestResponse<TApiResponse>(endpoint, method, body, query, dateFormat, serializer, deserializer);
            return CreateApiResult<T, TApiResponse, TResponse>(restWrappedResponse, wrappedError);
        }

        private IRestResponse<T> GetRestResponse<T>(
            string endpoint,
            RequestMethod method,
            object body,
            object query,
            string dateFormat,
            ISerializerWithDateFormat serializer,
            IDeserializerWithDateFormat deserializer)
            where T : class, new()
        {
            var clientWrapper = ApiClientWrapperBuilder.CreateClientWrapper(context);
            var parameters = ApiClientWrapperBuilder.CreateClientWrapperParameters(context, baseUrl, endpoint, method,
                body, query, dateFormat, serializer, deserializer);
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
                : new ApiResult<T>(restResponse.Data, restResponse, context);
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
            return new ApiResult<T>(data?.Data, restWrappedResponse, context, data?.Context, data?.Request);
        }

        private ApiResult<T> CreateApiResultForError<T>(IRestResponse restResponse, bool wrappedError)
            where T : class
        {
            if (wrappedError)
            {
                var errorData = DeserializeResponse<WrappedError>(restResponse);
                return new ApiResult<T>(default, restResponse, context, errorData?.Context, errorData?.Request);
            }

            var apiError = DeserializeResponse<UnwrappedError>(restResponse);
            return new ApiResult<T>(default, restResponse, context, apiError?.Message);
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
