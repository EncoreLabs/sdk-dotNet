using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Helpers.RestClientWrapper;

namespace EncoreTickets.SDK.Api
{
    internal static class ApiClientWrapperBuilder
    {
        public static RestClientWrapper CreateClientWrapper(ApiContext context)
        {
            var credentials = context == null
                ? null
                : new RestClientWrapperCredentials
                {
                    AuthenticationMethod = context.AuthenticationMethod,
                    AccessToken = context.AccessToken,
                    Username = context.UserName,
                    Password = context.Password
                };
            return new RestClientWrapper(credentials);
        }

        public static RestClientParameters CreateClientWrapperParameters(ApiContext context, string baseUrl, string endpoint,
            RequestMethod method, object body)
        {
            return new RestClientParameters
            {
                BaseUrl = baseUrl,
                RequestUrl = endpoint,
                RequestBody = body,
                RequestFormat = RequestFormat.Json,
                RequestHeaders = GetHeaders(context),
                RequestMethod = method,
                RequestQueryParameters = GetQueryParameters(context, method),
                RequestUrlSegments = null,
            };
        }

        private static Dictionary<string, string> GetHeaders(ApiContext context)
        {
            var headers = new Dictionary<string, string>
            {
                {"x-SDK", "EncoreTickets.SDK.NET" } // todo: add build numbers
            };

            if (!string.IsNullOrWhiteSpace(context.Affiliate))
            {
                headers.Add("affiliateId", context.Affiliate);
            }

            if (context.UseBroadway)
            {
                headers.Add("x-apply-price-engine", "true");
                headers.Add("x-market", "broadway");
            }
            return headers;
        }

        private static Dictionary<string, string> GetQueryParameters(ApiContext context, RequestMethod method)
        {
            var queryParameters = new Dictionary<string, string>();
            if (context.UseBroadway && method == RequestMethod.Get)
            {
                queryParameters.Add("countryCode", "US");
            }

            return queryParameters;
        }
    }
}
