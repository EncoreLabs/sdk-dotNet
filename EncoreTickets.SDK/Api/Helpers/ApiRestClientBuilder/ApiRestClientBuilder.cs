using System.Collections.Generic;
using System.Reflection;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Utilities.Common.RestClientWrapper;
using EncoreTickets.SDK.Utilities.Common.Serializers;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Api.Helpers.ApiRestClientBuilder
{
    /// <summary>
    /// Helper class for creating entities for the rest client wrapper of API services.
    /// </summary>
    /// <inheritdoc/>
    internal class ApiRestClientBuilder : IApiRestClientBuilder
    {
        /// <inheritdoc/>
        public virtual RestClientWrapper CreateClientWrapper(ApiContext context)
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

        /// <inheritdoc/>
        public RestClientParameters CreateClientWrapperParameters(
            ApiContext context,
            string baseUrl,
            ExecuteApiRequestParameters requestParameters)
        {
            return new RestClientParameters
            {
                BaseUrl = baseUrl,
                RequestUrl = requestParameters.Endpoint,
                RequestMethod = requestParameters.Method,
                RequestBody = requestParameters.Body,
                RequestFormat = RequestFormat.Json,
                RequestHeaders = GetHeaders(context),
                RequestQueryParameters = GetQueryParameters(requestParameters.Query),
                Serializer = GetInitializedSerializer(requestParameters.Serializer, requestParameters.DateFormat),
                Deserializer = GetInitializedSerializer(requestParameters.Deserializer, requestParameters.DateFormat),
            };
        }

        private static Dictionary<string, string> GetHeaders(ApiContext context)
        {
            var buildNumber = GetBuildNumber();
            var headers = new Dictionary<string, string>
            {
                {"x-SDK", $"EncoreTickets.SDK.NET {buildNumber}"}
            };

            if (!string.IsNullOrWhiteSpace(context?.Affiliate))
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

        private static ISerializerWithDateFormat GetInitializedSerializer(ISerializerWithDateFormat sourceSerializer, string dateFormat)
        {
            var serializer = sourceSerializer ?? new DefaultJsonSerializer();
            serializer.DateFormat = dateFormat;
            return serializer;
        }
    }
}
