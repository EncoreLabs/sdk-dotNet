using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Utilities.Common.Serializers;
using EncoreTickets.SDK.Utilities.Enums;
using Newtonsoft.Json.Serialization;
using Polly;
using RestSharp;
using RestSharp.Authenticators;

namespace EncoreTickets.SDK.Utilities.Common.RestClientWrapper
{
    /// <summary>
    /// Helper class for working with RestSharp classes.
    /// </summary>
    public class RestClientWrapper
    {
        private const int MaxExecutionsCount = 2;

        private static readonly List<HttpStatusCode> SuccessfulStatusCodes = new List<HttpStatusCode>
        {
            HttpStatusCode.OK,
            HttpStatusCode.Moved,
            HttpStatusCode.NoContent,
            HttpStatusCode.PaymentRequired,
            HttpStatusCode.Redirect,
            HttpStatusCode.RedirectMethod,
            HttpStatusCode.TemporaryRedirect,
        };

        private readonly RestClientWrapperCredentials credentials;

        /// <summary>
        /// Initializes a new instance of <see cref="RestClientWrapper"/>
        /// </summary>
        /// <param name="restClientWrapperCredentials">Credentials for requests.</param>
        public RestClientWrapper(RestClientWrapperCredentials restClientWrapperCredentials)
        {
            credentials = restClientWrapperCredentials;
        }

        /// <summary>
        /// Returns an initialized client.
        /// </summary>
        /// <param name="restClientParameters">Parameters.</param>
        /// <returns>Rest client.</returns>
        public IRestClient GetRestClient(RestClientParameters restClientParameters)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(restClientParameters.BaseUrl)
            {
                Authenticator = GetAuthenticator()
            };
            client.AddHandler("application/json", () => new JsonSerializer(restClientParameters.RequestDateFormat));
            return client;
        }

        /// <summary>
        /// Returns an initialized request.
        /// </summary>
        /// <param name="restClientParameters">Parameters.</param>
        /// <returns>Rest request.</returns>
        public IRestRequest GetRestRequest(RestClientParameters restClientParameters)
        {
            var request = new RestRequest(restClientParameters.RequestUrl)
            {
                Method = GetRequestMethod(restClientParameters),
                RequestFormat = GetDataFormat(restClientParameters),
                JsonSerializer = new JsonSerializer(restClientParameters.RequestDateFormat)
            };
            SetRequestParameters(request, restClientParameters.RequestHeaders, ParameterType.HttpHeader);
            SetRequestParameters(request, restClientParameters.RequestUrlSegments, ParameterType.UrlSegment);
            SetRequestParameters(request, restClientParameters.RequestQueryParameters, ParameterType.QueryString);
            SetRequestBody(request, restClientParameters);
            return request;
        }

        /// <summary>
        /// Executes a request initialized with parameters.
        /// </summary>
        /// <param name="restClientParameters">Parameters.</param>
        /// <returns>Rest response.</returns>
        public IRestResponse Execute(RestClientParameters restClientParameters)
        {
            var client = GetRestClient(restClientParameters);
            var request = GetRestRequest(restClientParameters);
            return Execute(client, request);
        }

        /// <summary>
        /// Executes a request.
        /// </summary>
        /// <param name="client">The prepared rest client.</param>
        /// <param name="request">The prepared rest request.</param>
        /// <returns>Rest response.</returns>
        public IRestResponse Execute(IRestClient client, IRestRequest request)
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse>(resp => !IsGoodResponse(resp))
                .Retry(MaxExecutionsCount)
                .Execute(() => client.Execute(request));
            return response;
        }

        /// <summary>
        /// Executes a request with expected data of a certain type.
        /// </summary>
        /// <param type="T">The type of an object expected in response.</param>
        /// <param name="client">The prepared rest client.</param>
        /// <param name="request">The prepared rest request.</param>
        /// <returns>Rest response.</returns>
        public IRestResponse<T> Execute<T>(IRestClient client, IRestRequest request)
            where T : class, new()
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse<T>>(resp => !IsGoodResponse(resp))
                .Retry(MaxExecutionsCount)
                .Execute(() => client.Execute<T>(request));
            return response;
        }

        /// <summary>
        /// Returns the state of a response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns><c>true</c> If the response is good ; otherwise, <c>false</c></returns>
        public bool IsGoodResponse(IRestResponse response)
        {
            if (response.ErrorException != null || !string.IsNullOrEmpty(response.ErrorMessage))
            {
                return false;
            }

            return response.IsSuccessful || SuccessfulStatusCodes.Contains(response.StatusCode);
        }

        private IAuthenticator GetAuthenticator()
        {
            if (credentials == null)
            {
                return null;
            }

            switch (credentials.AuthenticationMethod)
            {
                case AuthenticationMethod.JWT:
                    return string.IsNullOrEmpty(credentials.AccessToken)
                        ? null
                        : new JwtAuthenticator(credentials.AccessToken);
                case AuthenticationMethod.Basic:
                    return new HttpBasicAuthenticator(credentials.Username, credentials.Password);
                default:
                    return null;
            }
        }

        private Method GetRequestMethod(RestClientParameters restClientParameters)
        {
            switch (restClientParameters.RequestMethod)
            {
                case RequestMethod.Get:
                    return Method.GET;
                case RequestMethod.Post:
                    return Method.POST;
                case RequestMethod.Put:
                    return Method.PUT;
                case RequestMethod.Patch:
                    return Method.PATCH;
                case RequestMethod.Delete:
                    return Method.DELETE;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private DataFormat GetDataFormat(RestClientParameters restClientParameters)
        {
            return restClientParameters.RequestFormat == RequestFormat.Xml
                ? DataFormat.Xml
                : DataFormat.Json;
        }

        private void SetRequestParameters(RestRequest request, Dictionary<string, string> parameters, ParameterType type)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var param in parameters)
            {
                request.AddParameter(param.Key, param.Value, type);
            }
        }

        private void SetRequestBody(IRestRequest request, RestClientParameters restClientParameters)
        {
            if (restClientParameters.RequestBody != null)
            {
                request.AddBody(restClientParameters.RequestBody);
            }
        }
    }
}