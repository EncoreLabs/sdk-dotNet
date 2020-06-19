using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Utilities.Serializers;
using RestSharp;

namespace EncoreTickets.SDK.Api.Utilities
{
    /// <summary>
    /// The factory for creating a specific API result depending on the format of the error wrapper.
    /// </summary>
    internal static class ApiResultForErrorFactory
    {
        private static readonly RestSharp.Deserializers.IDeserializer ErrorsDeserializer = new DefaultJsonSerializer();

        /// <summary>
        /// Creates an instance of <see cref="ApiResult{T}"/> for API errors in a certain format.
        /// </summary>
        /// <typeparam name="T">Type of expected data.</typeparam>
        /// <param name="errorWrapping">Wrapper format for errors.</param>
        /// <param name="restResponse">Response.</param>
        /// <param name="context">API context.</param>
        /// <returns>API result.</returns>
        public static ApiResult<T> Create<T>(ErrorWrapping errorWrapping, IRestResponse restResponse, ApiContext context)
        {
            switch (errorWrapping)
            {
                case ErrorWrapping.Context:
                    return CreateApiResultIfWrappedErrorInContext<T>(restResponse, context);
                case ErrorWrapping.MessageWithCode:
                    return CreateApiResultIfUnwrappedError<T>(restResponse, context);
                case ErrorWrapping.Errors:
                    return CreateApiResultIfWrappedError<T>(restResponse, context);
                default:
                    return CreateApiResultIfNotParsedContent<T>(restResponse, context);
            }
        }

        private static ApiResult<T> CreateApiResultIfWrappedErrorInContext<T>(IRestResponse restResponse, ApiContext context)
        {
            var errorData = ErrorsDeserializer.Deserialize<WrappedErrorInContext>(restResponse);
            return new ApiResult<T>(default, restResponse, context, errorData?.Context, errorData?.Request);
        }

        private static ApiResult<T> CreateApiResultIfUnwrappedError<T>(IRestResponse restResponse, ApiContext context)
        {
            var apiError = ErrorsDeserializer.Deserialize<UnwrappedError>(restResponse);
            return new ApiResult<T>(default, restResponse, context, apiError?.Message);
        }

        private static ApiResult<T> CreateApiResultIfWrappedError<T>(IRestResponse restResponse, ApiContext context)
        {
            var apiError = ErrorsDeserializer.Deserialize<WrappedError>(restResponse);
            return new ApiResult<T>(default, restResponse, context, apiError.Errors);
        }

        private static ApiResult<T> CreateApiResultIfNotParsedContent<T>(IRestResponse restResponse, ApiContext context)
        {
            var errorMessage = $"Cannot convert API error correctly.\r\n\r\n{restResponse.Content}";
            return new ApiResult<T>(default, restResponse, context, errorMessage);
        }
    }
}
