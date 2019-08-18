using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using RestSharp;

namespace EncoreTickets.SDK.Api
{
    /// <summary>
    /// The base api class to be extended by concrete implementations
    /// </summary>
    public abstract class BaseApi
    {
        /// <summary>
        /// The host
        /// </summary>
        private readonly string host;

        /// <summary>
        /// The api context
        /// </summary>
        protected ApiContext Context;

        protected string BaseUrl => "https://" + string.Format(host, Context.Environment);

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApi"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="host">The host.</param>
        protected BaseApi(ApiContext context, string host)
        {
            Context = context;
            this.host = host;
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
        protected ApiResult<T> ExecuteApi<T>(string endpoint, RequestMethod method, bool wrapped, object body)
            where T : class
        {
            return ExecuteApi<T, ApiResult<T>>(endpoint, method, wrapped, body,
                (request, restResponse, apiResponse) =>
                    new ApiResult<T>(Context, request, restResponse, apiResponse));
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
        protected ApiResultList<T> ExecuteApiList<T>(string endpoint, RequestMethod method, bool wrapped, object body)
            where T : class
        {
            return ExecuteApi<T, ApiResultList<T>>(endpoint, method, wrapped, body,
                (request, restResponse, apiResponse) =>
                    new ApiResultList<T>(Context, request, restResponse, apiResponse));
        }

        private TResult ExecuteApi<T, TResult>(string endpoint, RequestMethod method, bool wrapped, object body,
            Func<IRestRequest, IRestResponse, ApiResponse<T>, TResult> createResultFunc)
            where T : class
            where TResult : ApiResultBase<T>
        {
            var clientWrapper = ApiClientWrapperBuilder.CreateClientWrapper(Context);
            var parameters = ApiClientWrapperBuilder.CreateClientWrapperParameters(Context, BaseUrl, endpoint, method, body);
            var client = clientWrapper.GetRestClient(parameters);
            var request = clientWrapper.GetRestRequest(parameters);

            IRestResponse restResponse;
            ApiResponse<T> apiResponse;

            if (wrapped)
            {
                restResponse = clientWrapper.Execute<ApiResponse<T>>(client, request);
                apiResponse = ((IRestResponse<ApiResponse<T>>) restResponse).Data;
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
