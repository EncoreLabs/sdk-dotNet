using System.Collections.Generic;
using System.Reflection;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Helpers.RestClientWrapper;

namespace EncoreTickets.SDK.Api
{
    /// <summary>
    /// Helper class for creating entities for the rest client wrapper of API services.
    /// </summary>
    internal static class ApiClientWrapperBuilder
    {
        /// <summary>
        /// Creates <see cref="RestClientWrapper"></see> for requests to API./>
        /// </summary>
        /// <param name="context">API context.</param>
        /// <returns>Initialized client wrapper.</returns>
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

        /// <summary>
        /// Creates <see cref="RestClientParameters"></see> for requests to API./>
        /// </summary>
        /// <param name="context">API context.</param>
        /// <param name="baseUrl">Site URL.</param>
        /// <param name="endpoint">Resource endpoint.</param>
        /// <returns>Initialized client wrapper parameters.</returns>
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
            var buildNumber = GetBuildNumber();
            var headers = new Dictionary<string, string>
            {
                { "x-SDK", $"EncoreTickets.SDK.NET {buildNumber}" }
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

        private static string GetBuildNumber()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }
    }
}
