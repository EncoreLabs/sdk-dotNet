using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using RestSharp;

namespace EncoreTickets.SDK.Api
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
            var clientWrapper = ApiClientWrapperBuilder.CreateClientWrapper(context);
            var parameters = ApiClientWrapperBuilder.CreateClientWrapperParameters(context, baseUrl, endpoint, method,
                body, query, dateFormat);
            var client = clientWrapper.GetRestClient(parameters);
            var request = clientWrapper.GetRestRequest(parameters);
            return clientWrapper.Execute<T>(client, request);
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
            return new ApiResult<T>(default, restResponse, context, apiError);
        }

        private T DeserializeResponse<T>(IRestResponse response)
        {
            try
            {
                return SimpleJson.SimpleJson.DeserializeObject<T>(response.Content);
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
