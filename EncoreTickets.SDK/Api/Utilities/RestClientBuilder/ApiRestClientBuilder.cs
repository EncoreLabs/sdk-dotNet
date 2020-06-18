using System;
using System.Collections.Generic;
using System.Dynamic;
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
        private const string SdkVersionHeader = "x-SDK";
        private const string AffiliateHeader = "affiliateId";
        private const string CorrelationHeader = "X-Correlation-ID";
        private const string MarketHeader = "x-market";
        private const string DisplayCurrencyHeader = "x-display-currency";
        private const string AgentIdHeader = "X-AGENT-ID";
        private const string AgentPasswordHeader = "X-AGENT-PASSWORD";

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
                    Password = context.Password,
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

        /// <inheritdoc/>
        public void SaveResponseInfoInApiContext(RestResponseInformation responseInformation, ApiContext context)
        {
            var responseHeaderKey = responseInformation?.ResponseHeaders?.Keys.FirstOrDefault(x =>
                x.Equals(CorrelationHeader, StringComparison.InvariantCultureIgnoreCase));
            context.ReceivedCorrelation = responseHeaderKey != null
                ? responseInformation.ResponseHeaders[responseHeaderKey].ToString()
                : null;
        }

        private static Dictionary<string, string> GetHeaders(ApiContext context)
        {
            var buildNumber = GetBuildNumber();
            var headers = new Dictionary<string, string>
            {
                { SdkVersionHeader, $"EncoreTickets.SDK.NET {buildNumber}" },
            };

            if (!string.IsNullOrWhiteSpace(context?.Affiliate))
            {
                headers.Add(AffiliateHeader, context.Affiliate);
            }

            if (!string.IsNullOrWhiteSpace(context?.Correlation))
            {
                headers.Add(CorrelationHeader, context.Correlation);
            }

            if (context?.Market != null)
            {
                headers.Add(MarketHeader, context.Market.ToString());
            }

            if (!string.IsNullOrWhiteSpace(context?.DisplayCurrency))
            {
                headers.Add(DisplayCurrencyHeader, context.DisplayCurrency);
            }

            if (context?.AgentCredentials != null)
            {
                headers.Add(AgentIdHeader, context.AgentCredentials.Username);
                headers.Add(AgentPasswordHeader, context.AgentCredentials.Password);
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

            var notFilteredQueryParameters = GetAllQueryParameters(queryObject);
            var queryParameters = notFilteredQueryParameters
                .Where(x => !string.IsNullOrWhiteSpace(x.Item1) && !string.IsNullOrWhiteSpace(x.Item2))
                .ToDictionary(x => x.Item1, x => x.Item2);
            return queryParameters.Count == 0 ? null : queryParameters;
        }

        private static IEnumerable<(string, string)> GetAllQueryParameters(object queryObject)
        {
            if (queryObject is ExpandoObject expandoObject)
            {
                return expandoObject.Select(x => GetQueryParameter(x.Key, x.Value));
            }

            var type = queryObject.GetType();
            var properties = type.GetProperties();
            return properties.Select(x => GetQueryParameter(queryObject, x));
        }

        private static (string, string) GetQueryParameter(object queryObject, PropertyInfo property)
        {
            var propertyValue = property.GetValue(queryObject, null);
            return propertyValue == null ? (null, null) : GetQueryParameter(property.Name, propertyValue);
        }

        private static (string, string) GetQueryParameter(string name, object value)
        {
            var parameterName = name.ToLower();
            var parameterValue = Convert.ToString(value, CultureInfo.InvariantCulture);
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
