using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Helpers.RestClientWrapper;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Interfaces;
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

        /// <summary>
        /// Get an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <param name="endpoint">API resource endpoint.</param>
        /// <param name="method">Request method.</param>
        /// <param name="wrapped"><c>true</c> if expected data should be wrapped with extra data on API side, <see cref="ApiResponse{T}"/>; otherwise, <c>false</c>.</param>
        /// <param name="body">Request body.</param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResult<T> ExecuteApi<T>(string endpoint, RequestMethod method, bool wrapped, object body = null)
            where T : class
        {
            return ExecuteApi<T, ApiResult<T>>(endpoint, method, wrapped, body,
                (response, apiResponse) => new ApiResult<T>(context, response, apiResponse));
        }

        /// <summary>
        /// Get a list of <typeparamref name="T"/> objects.
        /// </summary>
        /// <typeparam name="T">Type of objects of an expected list.</typeparam>
        /// <param name="endpoint">API resource endpoint.</param>
        /// <param name="method">Request method.</param>
        /// <param name="wrapped"><c>true</c> if expected data should be wrapped with extra data on API side, <see cref="ApiResponse{T}"/>; otherwise, <c>false</c>.</param>
        /// <param name="body">Request body.</param>
        /// <returns>Result of request execution.</returns>
        public virtual ApiResultList<T> ExecuteApiList<T>(string endpoint, RequestMethod method, bool wrapped, object body = null)
            where T : class, IEnumerable<IObject>
        {
            return ExecuteApi<T, ApiResultList<T>>(endpoint, method, wrapped, body,
                (response, apiResponse) => new ApiResultList<T>(context, response, apiResponse));
        }

        private TResult ExecuteApi<T, TResult>(string endpoint, RequestMethod method, bool wrapped, object body,
            Func<IRestResponse, ApiResponse<T>, TResult> createResultFunc)
            where T : class
            where TResult : ApiResultBase
        {
            var clientWrapper = ApiClientWrapperBuilder.CreateClientWrapper(context);
            var parameters = ApiClientWrapperBuilder.CreateClientWrapperParameters(context, baseUrl, endpoint, method, body);
            var client = clientWrapper.GetRestClient(parameters);
            var request = clientWrapper.GetRestRequest(parameters);
            return wrapped
                ? ExecuteApiToGetWrappedData(clientWrapper, client, request, createResultFunc)
                : ExecuteApiToGetData(clientWrapper, client, request, createResultFunc);
        }

        private TResult ExecuteApiToGetWrappedData<T, TResult>(RestClientWrapper clientWrapper, IRestClient client, IRestRequest request,
            Func<IRestResponse, ApiResponse<T>, TResult> createResultFunc)
            where T : class
            where TResult : ApiResultBase
        {
            var restWrappedResponse = clientWrapper.Execute<ApiResponse<T>>(client, request);
            return createResultFunc(restWrappedResponse, restWrappedResponse.Data);
        }

        private TResult ExecuteApiToGetData<T, TResult>(RestClientWrapper clientWrapper, IRestClient client, IRestRequest request,
            Func<IRestResponse, ApiResponse<T>, TResult> createResultFunc)
            where T : class
            where TResult : ApiResultBase
        {
            var restResponse = clientWrapper.Execute(client, request);
            var rawData = clientWrapper.IsGoodResponse(restResponse)
                ? SimpleJson.SimpleJson.DeserializeObject<T>(restResponse.Content)
                : null;
            var apiResponse = new ApiResponse<T>(rawData);
            return createResultFunc(restResponse, apiResponse);
        }
    }
}
