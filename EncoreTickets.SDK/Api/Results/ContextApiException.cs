using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results.Response;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    /// <summary>
    /// The base exception for API requests with warnings in response contexts.
    /// </summary>
    public class ContextApiException : ApiException
    {
        private readonly IEnumerable<string> codesOfInfos;

        /// <summary>
        /// Initializes a new instance of <see cref="ContextApiException"/>
        /// </summary>
        internal ContextApiException(
            IEnumerable<string> codesOfInfosAsErrors,
            IRestResponse response,
            ApiContext requestContext,
            Response.Context contextInResponse,
            Request requestInResponse)
            : base(response, requestContext, contextInResponse, requestInResponse)
        {
            codesOfInfos = codesOfInfosAsErrors;
        }

        /// <inheritdoc />
        protected override List<string> GetErrors()
        {
            var infosAsErrors = ContextInResponse?.info?.Where(x => codesOfInfos.Contains(x.code));
            var errors = infosAsErrors?.Select(ConvertInfoToString).ToList() ?? new List<string>();
            return errors;
        }

        private string ConvertInfoToString(Info info)
        {
            return string.IsNullOrEmpty(info.message) ? info.code : info.message;
        }
    }
}