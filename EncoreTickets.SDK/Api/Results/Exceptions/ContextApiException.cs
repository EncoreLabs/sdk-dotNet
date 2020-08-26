using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results.Exceptions
{
    /// <summary>
    /// The base exception for API requests with warnings in response contexts.
    /// </summary>
    public class ContextApiException : ApiException
    {
        /// <inheritdoc />
        public override List<string> Errors => GetContextErrorsAsStrings();

        /// <summary>
        /// Gets infos that should be errors.
        /// </summary>
        public IEnumerable<Info> ContextErrors { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ContextApiException"/> class.
        /// </summary>
        public ContextApiException(IEnumerable<Info> infosAsErrors)
        {
            ContextErrors = infosAsErrors;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ContextApiException"/> class.
        /// </summary>
        public ContextApiException(ContextApiException sourceException)
            : this(
            sourceException?.ContextErrors,
            sourceException?.Response,
            sourceException?.Context,
            sourceException?.ContextInResponse,
            sourceException?.RequestInResponse)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ContextApiException"/> class.
        /// </summary>
        public ContextApiException(
            IEnumerable<Info> infosAsErrors,
            IRestResponse response,
            ApiContext requestContext,
            Context contextInResponse,
            Request requestInResponse)
            : base(response, requestContext, contextInResponse, requestInResponse)
        {
            ContextErrors = infosAsErrors;
        }

        private static string ConvertInfoToString(Info info)
        {
            return string.IsNullOrEmpty(info.Message) ? info.Code : info.Message;
        }

        private List<string> GetContextErrorsAsStrings()
        {
            var errors = ContextErrors?.Select(ConvertInfoToString);
            return errors.ExcludeEmptyStrings().NullIfEmptyEnumerable();
        }
    }
}