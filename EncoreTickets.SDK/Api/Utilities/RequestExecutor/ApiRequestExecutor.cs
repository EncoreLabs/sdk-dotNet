using System;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RestClientBuilder;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using EncoreTickets.SDK.Utilities.RestClientWrapper;
using RestSharp;

namespace EncoreTickets.SDK.Api.Utilities.RequestExecutor
{
    /// <summary>
    /// API requests executor.
    /// </summary>
    public class ApiRequestExecutor
    {
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
        /// <returns>Result of request execution.</returns>
        public ApiResult<T> ExecuteApiWithNotWrappedResponse<T>(ExecuteApiRequestParameters requestParameters)
            where T : class, new()
        {
            var restResponse = GetRestResponse<T>(requestParameters);
            return CreateApiResult(restResponse);
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should be standard wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <param name="requestParameters">Parameters for initializing an API request</param>
        /// <returns>Result of request execution.</returns>
        public ApiResult<T> ExecuteApiWithWrappedResponse<T>(
            ExecuteApiRequestParameters requestParameters)
            where T : class
        {
            var restWrappedResponse = GetRestResponse<ApiResponse<T>>(requestParameters);
            return CreateApiResult<T, ApiResponse<T>, T>(restWrappedResponse);
        }

        /// <summary>
        /// Get an object of <typeparamref name="T"/> from API when expected data should be non-standard wrapped with extra data on API side.
        /// </summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <typeparam name="TApiResponse">Type of the response object.</typeparam>
        /// <typeparam name="TResponse">The type of data in a "response" section of the response object.</typeparam>
        /// <param name="requestParameters">Parameters for initializing an API request</param>
        /// <returns>Result of request execution.</returns>
        public ApiResult<T> ExecuteApiWithWrappedResponse<T, TApiResponse, TResponse>(
            ExecuteApiRequestParameters requestParameters)
            where T : class
            where TApiResponse : BaseWrappedApiResponse<TResponse, T>, new()
            where TResponse : class
        {
            var restWrappedResponse = GetRestResponse<TApiResponse>(requestParameters);
            return CreateApiResult<T, TApiResponse, TResponse>(restWrappedResponse);
        }

        private IRestResponse<T> GetRestResponse<T>(ExecuteApiRequestParameters requestParameters)
            where T : class, new()
        {
            var clientWrapper = restClientBuilder.CreateClientWrapper(Context);
            var clientParameters = restClientBuilder.CreateClientWrapperParameters(Context, BaseUrl, requestParameters);
            var client = clientWrapper.GetRestClient(clientParameters);
            var request = clientWrapper.GetRestRequest(clientParameters);
            var response = clientWrapper.Execute<T>(client, request);
            SaveResponseInfoInContext(response);
            return response;
        }

        private void SaveResponseInfoInContext(IRestResponse response)
        {
            var responseInfo = new RestResponseInformation
            {
                ResponseHeaders = response.Headers?.ToDictionary(x => x.Name, x => x.Value)
            };
            restClientBuilder.SaveResponseInfoInApiContext(responseInfo, Context);
        }

        private ApiResult<T> CreateApiResult<T>(
            IRestResponse<T> restResponse)
            where T : class
        {
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                return new ApiResult<T>(restResponse.Data, restResponse, Context);
            }

            return TryToCreateApiResultForError<T>(restResponse);
        }

        private ApiResult<T> CreateApiResult<T, TApiResponse, TResponse>(
            IRestResponse<TApiResponse> restWrappedResponse)
            where T : class
            where TResponse : class
            where TApiResponse : BaseWrappedApiResponse<TResponse, T>, new()
        {
            if (restWrappedResponse.StatusCode == HttpStatusCode.OK)
            {
                var data = restWrappedResponse.Data;
                return new ApiResult<T>(data?.Data, restWrappedResponse, Context, data?.Context, data?.Request);
            }

            return TryToCreateApiResultForError<T>(restWrappedResponse);
        }

        private ApiResult<T> TryToCreateApiResultForError<T>(IRestResponse restResponse)
            where T : class
        {
            var errorWrappings = EnumExtension.GetEnumValues<ErrorWrapping>();
            foreach (var errorWrapping in errorWrappings)
            {
                try
                {
                    return ApiResultForErrorFactory.Create<T>(errorWrapping, restResponse, Context);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            throw new ApiException($"Cannot convert API error correctly.\r\n\r\n{restResponse.Content}", restResponse, Context);
        }
    }
}
