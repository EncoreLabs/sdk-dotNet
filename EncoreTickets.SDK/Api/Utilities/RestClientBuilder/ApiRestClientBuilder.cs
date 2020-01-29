using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Utilities.RestClientWrapper;
using EncoreTickets.SDK.Utilities.Serializers;

namespace EncoreTickets.SDK.Api.Utilities.RestClientBuilder
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
                : new RestClientCredentials
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
            var serializer = GetInitializedSerializer(requestParameters.Serializer, requestParameters.DateFormat);
            var deserializer = GetInitializedSerializer(requestParameters.Deserializer, requestParameters.DateFormat);
            return new RestClientParameters
            {
                BaseUrl = baseUrl,
                RequestUrl = requestParameters.Endpoint,
                RequestMethod = requestParameters.Method,
                RequestBody = requestParameters.Body,
                RequestHeaders = GetHeaders(context),
                RequestQueryParameters = GetQueryParameters(requestParameters.Query),
                RequestDataFormat = serializer.SerializedDataFormat,
                RequestDataSerializer = serializer,
                ResponseDataFormat = deserializer.SerializedDataFormat,
                ResponseDataDeserializer = deserializer,
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

            var type = queryObject.GetType();
            var properties = type.GetProperties();
            var queryParameters = properties
                .Select(x => GetQueryParameter(queryObject, x))
                .Where(x => x.Item1 != null)
                .ToDictionary(x => x.Item1, x => x.Item2);
            return queryParameters.Count == 0 ? null : queryParameters;
        }

        private static (string, string) GetQueryParameter(object queryObject, PropertyInfo property)
        {
            var propertyValue = property.GetValue(queryObject, null);
            if (propertyValue == null)
            {
                return (null, null);
            }

            var parameterName = property.Name.ToLower();
            var parameterValue = Convert.ToString(propertyValue, CultureInfo.InvariantCulture);
            return (parameterName, parameterValue);
        }

        private static ISerializerWithDateFormat GetInitializedSerializer(ISerializerWithDateFormat sourceSerializer, string dateFormat)
        {
            var serializer = sourceSerializer ?? new DefaultJsonSerializer();
            serializer.DateFormat = dateFormat;
            return serializer;
        }
    }
}
