using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    public class ApiException : Exception
    {
        public override string Message => string.Join("; ", Errors);

        /// <summary>
        /// Gets a context object for which the request was made.
        /// </summary>
        public ApiContext Context { get; }

        public List<string> Errors { get; }

        public HttpStatusCode ResponseCode { get; }

        public IRestResponse Response { get; }

        public ApiException(IRestResponse response, ApiContext requestContext, Response.Context responseContext)
        {
            Context = requestContext;
            Errors = responseContext?.errors == null
                ? new List<string> {response.ErrorMessage}
                : responseContext.errors.Select(x => x.message).ToList();
            ResponseCode = response.StatusCode;
            Response = response;
        }
    }
}
