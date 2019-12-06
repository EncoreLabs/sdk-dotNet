using System.Collections.Generic;
using System.Reflection;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Utilities.Common.RestClientWrapper;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Api.Helpers
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
        /// <param name="method">Request method.</param>
        /// <param name="body">Request body.</param>
        /// <param name="queryObject">Object for request query.</param>
        /// <param name="dateFormat">Request date format.</param>
        /// <returns>Initialized client wrapper parameters.</returns>
        public static RestClientParameters CreateClientWrapperParameters(ApiContext context, string baseUrl, string endpoint,
            RequestMethod method, object body, object queryObject, string dateFormat)
        {
            return new RestClientParameters
            {
                BaseUrl = baseUrl,
                RequestUrl = endpoint,
                RequestFormat = RequestFormat.Json,
                RequestHeaders = GetHeaders(context),
                RequestMethod = method,
                RequestDateFormat = dateFormat,
                RequestBody = body,
                RequestQueryParameters = GetQueryParameters(queryObject),
            };
        }

        private static Dictionary<string, string> GetHeaders(ApiContext context)
        {
            var buildNumber = GetBuildNumber();
            var headers = new Dictionary<string, string>
            {
                {"x-SDK", $"EncoreTickets.SDK.NET {buildNumber}"}
            };

            if (!string.IsNullOrWhiteSpace(context.Affiliate))
            {
                headers.Add("affiliateId", context.Affiliate);
            }

            return headers;
        }

        private static string GetBuildNumber()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        private static Dictionary<string, string> GetQueryParameters(object queryObject)
        {
            if (queryObject == null)
            {
                return null;
            }

            var result = new Dictionary<string, string>();
            var type = queryObject.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(queryObject, null);
                if (propertyValue != null)
                {
                    result.Add(property.Name.ToLower(), propertyValue.ToString());
                }
            }

            return result.Count == 0 ? null : result;
        }
    }
}
