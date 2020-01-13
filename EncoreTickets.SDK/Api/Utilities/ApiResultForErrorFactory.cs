﻿using System;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Utilities.Serializers;
using RestSharp;
using RestSharp.Deserializers;

namespace EncoreTickets.SDK.Api.Utilities
{
    /// <summary>
    /// The factory for creating a specific API result depending on the format of the error wrapper
    /// </summary>
    internal static class ApiResultForErrorFactory
    {
        private static readonly IDeserializer ErrorsDeserializer = new DefaultJsonSerializer();

        /// <summary>
        /// Creates an instance of <see cref="ApiResult{T}"/> for API errors in a certain format
        /// </summary>
        /// <typeparam name="T">Type of expected data</typeparam>
        /// <param name="errorWrapping">Wrapper format for errors</param>
        /// <param name="restResponse">Response</param>
        /// <param name="context">API context</param>
        /// <returns>API result</returns>
        public static ApiResult<T> Create<T>(ErrorWrapping errorWrapping, IRestResponse restResponse, ApiContext context)
            where T : class
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
                    throw new ArgumentOutOfRangeException(nameof(errorWrapping), errorWrapping, null);
            }
        }

        private static ApiResult<T> CreateApiResultIfWrappedErrorInContext<T>(IRestResponse restResponse, ApiContext context)
            where T : class
        {
            var errorData = ErrorsDeserializer.Deserialize<WrappedErrorInContext>(restResponse);
            return new ApiResult<T>(default, restResponse, context, errorData?.Context, errorData?.Request);
        }

        private static ApiResult<T> CreateApiResultIfUnwrappedError<T>(IRestResponse restResponse, ApiContext context)
            where T : class
        {
            var apiError = ErrorsDeserializer.Deserialize<UnwrappedError>(restResponse);
            return new ApiResult<T>(default, restResponse, context, apiError?.Message);
        }

        private static ApiResult<T> CreateApiResultIfWrappedError<T>(IRestResponse restResponse, ApiContext context)
            where T : class
        {
            var apiError = ErrorsDeserializer.Deserialize<WrappedError>(restResponse);
            return new ApiResult<T>(default, restResponse, context, apiError.Errors);
        }
    }
}