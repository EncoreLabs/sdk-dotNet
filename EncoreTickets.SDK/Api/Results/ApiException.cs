using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results.Response;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    /// <summary>
    /// The base exception for failed API requests.
    /// </summary>
    public class ApiException : Exception
    {
        /// <inheritdoc/>
        public override string Message => Errors.Any() ? string.Join("; ", Errors) : null;

        /// <summary>
        /// Gets HTTP response status code.
        /// </summary>
        public HttpStatusCode ResponseCode => Response.StatusCode;

        /// <summary>
        /// Gets the API response errors.
        /// </summary>
        public List<string> Errors => GetErrors();

        /// <summary>
        /// Gets the details of the sent request.
        /// </summary>
        public Dictionary<string, object> Details => GetRequestDetails();

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
        public ApiException(IRestResponse response, ApiContext requestContext, Response.Context contextInResponse,
            Request requestInResponse)
        {
            RequestInResponse = requestInResponse;
            ContextInResponse = contextInResponse;
            Context = requestContext;
            Response = response;
        }

        private List<string> GetErrors()
        {
            return ContextInResponse?.errors == null
                ? new List<string> {Response.StatusDescription}
                : ContextInResponse.errors.Select(x => x.message).ToList();
        }

        private Dictionary<string, object> GetRequestDetails()
        {
            if (RequestInResponse == null)
            {
                return null;
            }

            var details = new Dictionary<string, object>();
            AddDynamicToDictionary(details, RequestInResponse.query);
            AddDynamicToDictionary(details, RequestInResponse.urlParams);
            if (!string.IsNullOrEmpty(RequestInResponse.body))
            {
                details.Add(nameof(RequestInResponse.body), RequestInResponse.body);
            }

            return details;
        }

        private void AddDynamicToDictionary(IDictionary<string, object> sourceDictionary, dynamic dynamicObject)
        {
            if (!(dynamicObject is IDictionary<string, object> objectDictionary))
            {
                return;
            }

            foreach (var keyValuePair in objectDictionary)
            {
                sourceDictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
