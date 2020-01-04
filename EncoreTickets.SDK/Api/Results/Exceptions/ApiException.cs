using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Utilities.Common.TypeExtensions;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results.Exceptions
{
    /// <summary>
    /// The base exception for failed API requests.
    /// </summary>
    public class ApiException : Exception
    {
        private const string DefaultMessage = "API exception occured";

        private readonly string predefinedMessage;

        /// <inheritdoc/>
        public override string Message => GetMessage();

        /// <summary>
        /// Gets HTTP response status code.
        /// </summary>
        public virtual HttpStatusCode ResponseCode => Response?.StatusCode ?? default;

        /// <summary>
        /// Gets the API response errors as messages.
        /// </summary>
        public virtual List<string> Errors => GetErrors();

        /// <summary>
        /// Gets a context object for which the request was made.
        /// </summary>
        public ApiContext Context { get; }

        /// <summary>
        /// Gets HTTP response.
        /// </summary>
        public IRestResponse Response { get; }

        /// <summary>
        /// Gets the request returned in the API response.
        /// </summary>
        public Request RequestInResponse { get; }

        /// <summary>
        /// Gets the context returned in the API response.
        /// </summary>
        public Response.Context ContextInResponse { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiException"/>
        /// </summary>
        public ApiException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiException"/>
        /// </summary>
        public ApiException(string message)
        {
            predefinedMessage = message;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiException"/>
        /// </summary>
        public ApiException(ApiException sourceException) : this(
            sourceException.Response,
            sourceException.Context,
            sourceException.ContextInResponse,
            sourceException.RequestInResponse)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiException"/>
        /// </summary>
        public ApiException(IRestResponse response, ApiContext requestContext, Response.Context contextInResponse,
            Request requestInResponse)
        {
            RequestInResponse = requestInResponse;
            ContextInResponse = contextInResponse;
            Context = requestContext;
            Response = response;
        }

        /// <summary>
        /// Returns easily read errors that are the cause of the exception.
        /// </summary>
        /// <returns>Collection of strings</returns>
        protected List<string> GetErrors()
        {
            var errors = GetErrorsAsString(Response, ContextInResponse);
            return errors.ExcludeEmptyStrings().ToList();
        }

        /// <summary>
        /// Returns a string contained easily read info about errors that are the cause of the exception.
        /// </summary>
        /// <returns>Message about errors</returns>
        protected string GetMessage()
        {
            if (predefinedMessage != null)
            {
                return predefinedMessage;
            }

            return Errors != null && Errors.Any()
                ? string.Join("; ", Errors)
                : DefaultMessage;
        }

        private static IEnumerable<string> GetErrorsAsString(IRestResponse response, Response.Context context)
        {
            if (context?.Errors != null)
            {
                return context.Errors.Select(ConvertErrorToString);
            }

            if (response == null)
            {
                return null;
            }

            var error = GetErrorAsStringFromRestResponse(response);
            return new List<string> {error};
        }

        private static string GetErrorAsStringFromRestResponse(IRestResponse response)
        {
            return string.IsNullOrEmpty(response.StatusDescription)
                ? response.ErrorMessage
                : response.StatusDescription;
        }

        private static string ConvertErrorToString(Error error)
        {
            var message = error.Message;
            if (!string.IsNullOrEmpty(error.Field))
            {
                message = $"{error.Field} - {message}";
            }

            return message;
        }
    }
}
