using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using RestSharp;

namespace EncoreTickets.SDK.Api
{
    public class ApiRequestExecutor
    {
        private readonly ApiContext context;
        private readonly string baseUrl;

        public ApiRequestExecutor(ApiContext context, string baseUrl)
        {
            this.context = context;
            this.baseUrl = baseUrl;
        }

        /// <summary>
        /// Execute an API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="wrapped"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public virtual ApiResult<T> ExecuteApi<T>(string endpoint, RequestMethod method, bool wrapped, object body)
            where T : class
        {
            return ExecuteApi<T, ApiResult<T>>(endpoint, method, wrapped, body,
                (request, restResponse, apiResponse) =>
                    new ApiResult<T>(context, request, restResponse, apiResponse));
        }

        /// <summary>
        /// Get a list of <typeparamref name="T"/> objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="wrapped"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public virtual ApiResultList<T> ExecuteApiList<T>(string endpoint, RequestMethod method, bool wrapped, object body)
            where T : class
        {
            return ExecuteApi<T, ApiResultList<T>>(endpoint, method, wrapped, body,
                (request, restResponse, apiResponse) =>
                    new ApiResultList<T>(context, request, restResponse, apiResponse));
        }

        private TResult ExecuteApi<T, TResult>(string endpoint, RequestMethod method, bool wrapped, object body,
            Func<IRestRequest, IRestResponse, ApiResponse<T>, TResult> createResultFunc)
            where T : class
            where TResult : ApiResultBase<T>
        {
            var clientWrapper = ApiClientWrapperBuilder.CreateClientWrapper(context);
            var parameters = ApiClientWrapperBuilder.CreateClientWrapperParameters(context, baseUrl, endpoint, method, body);
            var client = clientWrapper.GetRestClient(parameters);
            var request = clientWrapper.GetRestRequest(parameters);

            IRestResponse restResponse;
            ApiResponse<T> apiResponse;

            if (wrapped)
            {
                restResponse = clientWrapper.Execute<ApiResponse<T>>(client, request);
                apiResponse = ((IRestResponse<ApiResponse<T>>)restResponse).Data;
            }
            else
            {
                restResponse = clientWrapper.Execute(client, request);
                var rawData = clientWrapper.IsGoodResponse(restResponse)
                    ? SimpleJson.SimpleJson.DeserializeObject<T>(restResponse.Content)
                    : null;
                apiResponse = new ApiResponse<T>(rawData);
            }

            return createResultFunc(request, restResponse, apiResponse);
        }
    }
}
