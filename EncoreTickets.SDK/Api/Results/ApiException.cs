using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results.Response;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    public class ApiException : Exception
    {
        public override string Message => string.Join("; ", Errors);

        public HttpStatusCode ResponseCode => Response.StatusCode;

        /// <summary>
        /// Gets a context object for which the request was made.
        /// </summary>
        public ApiContext Context { get; }

        public IRestResponse Response { get; }

        public Request RequestInResponse { get; }

        public Response.Context ContextInResponse { get; }

        public List<string> Errors => GetErrors();

        public Dictionary<string, object> Details => GetRequestDetails();

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
