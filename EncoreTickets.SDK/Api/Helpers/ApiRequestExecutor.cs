using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers.ApiRestClientBuilder;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
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
        private readonly IApiRestClientBuilder restClientBuilder;

        /// <summary>
        /// Initializes a new instance of <see cref="ApiRequestExecutor"/>
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        /// <param name="baseUrl">The site URL.</param>
        public ApiRequestExecutor(ApiContext context, string baseUrl, IApiRestClientBuilder restClientBuilder)
        {
            this.context = context;
            this.baseUrl = baseUrl;
            this.restClientBuilder = restClientBuilder;
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should not be wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <param name="parameters"></param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApiWithNotWrappedResponse<T>(ExecuteApiRequestParameters requestParameters, bool wrappedError = false)
            where T : class, new()
        {
            var restResponse = GetRestResponse<T>(requestParameters);
            return CreateApiResult(restResponse, wrappedError);
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should be standard wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <param name="parameters"></param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApiWithWrappedResponse<T>(ExecuteApiRequestParameters requestParameters, bool wrappedError = true)
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
        /// <param name="parameters"></param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApiWithWrappedResponse<T, TApiResponse, TResponse>(ExecuteApiRequestParameters requestParameters, bool wrappedError = true)
            where T : class
            where TResponse : class
            where TApiResponse : BaseWrappedApiResponse<TResponse, T>, new()
        {
            var restWrappedResponse = GetRestResponse<TApiResponse>(requestParameters);
            return CreateApiResult<T, TApiResponse, TResponse>(restWrappedResponse, wrappedError);
        }

        private IRestResponse<T> GetRestResponse<T>(ExecuteApiRequestParameters requestParameters)
            where T : class, new()
        {
            var clientWrapper = restClientBuilder.CreateClientWrapper(context);
            var clientParameters = restClientBuilder.CreateClientWrapperParameters(context, baseUrl, requestParameters);
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
                : new ApiResult<T>(restResponse.Data, restResponse, context);
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
            return new ApiResult<T>(data?.Data, restWrappedResponse, context, data?.context, data?.request);
        }

        private ApiResult<T> CreateApiResultForError<T>(IRestResponse restResponse, bool wrappedError)
            where T : class
        {
            if (wrappedError)
            {
                var errorData = DeserializeResponse<WrappedError>(restResponse);
                return new ApiResult<T>(default, restResponse, context, errorData?.context, errorData?.request);
            }

            var apiError = DeserializeResponse<UnwrappedError>(restResponse);
            return new ApiResult<T>(default, restResponse, context, apiError?.message);
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
