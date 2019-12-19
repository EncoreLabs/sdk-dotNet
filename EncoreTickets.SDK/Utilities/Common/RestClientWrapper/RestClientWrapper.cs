using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Utilities.Enums;
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
        private const int DefaultMaxExecutionsCount = 2;

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

        private static readonly Dictionary<RequestMethod, Method> MethodMaps = new Dictionary<RequestMethod, Method>
        {
            {RequestMethod.Get, Method.GET},
            {RequestMethod.Post, Method.POST},
            {RequestMethod.Patch, Method.PATCH},
            {RequestMethod.Put, Method.PUT},
            {RequestMethod.Delete, Method.DELETE},
        };

        public int MaxExecutionsCount { get; }

        public RestClientWrapperCredentials Credentials { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="RestClientWrapper"/>
        /// </summary>
        /// <param name="restClientWrapperCredentials">Credentials for requests.</param>
        public RestClientWrapper(RestClientWrapperCredentials restClientWrapperCredentials)
            : this(DefaultMaxExecutionsCount)
        {
            Credentials = restClientWrapperCredentials;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RestClientWrapper"/>
        /// </summary>
        public RestClientWrapper() : this(DefaultMaxExecutionsCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RestClientWrapper"/>
        /// </summary>
        /// <param name="executionsCount">Optional: maximum number of additional retries if a request failed</param>
        public RestClientWrapper(int executionsCount)
        {
            MaxExecutionsCount = executionsCount;
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
            client.AddHandler(ContentTypes.ApplicationJson, () => restClientParameters.Deserializer);
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
                JsonSerializer = restClientParameters.Serializer
            };
            SetRequestParameters(request, restClientParameters.RequestHeaders, ParameterType.HttpHeader);
            SetRequestParameters(request, restClientParameters.RequestUrlSegments, ParameterType.UrlSegment);
            SetRequestParameters(request, restClientParameters.RequestQueryParameters, ParameterType.QueryString);
            SetRequestBody(request, restClientParameters);
            return request;
        }

        /// <summary>
        /// Executes a request.
        /// </summary>
        /// <param name="client">The prepared rest client.</param>
        /// <param name="request">The prepared rest request.</param>
        /// <returns>Rest response.</returns>
        public virtual IRestResponse Execute(IRestClient client, IRestRequest request)
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
        public virtual IRestResponse<T> Execute<T>(IRestClient client, IRestRequest request)
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
            if (Credentials == null)
            {
                return null;
            }

            switch (Credentials.AuthenticationMethod)
            {
                case AuthenticationMethod.JWT:
                    return string.IsNullOrEmpty(Credentials.AccessToken)
                        ? null
                        : new JwtAuthenticator(Credentials.AccessToken);
                case AuthenticationMethod.Basic:
                    return new HttpBasicAuthenticator(Credentials.Username, Credentials.Password);
                default:
                    return null;
            }
        }

        private Method GetRequestMethod(RestClientParameters restClientParameters)
        {
            return MethodMaps[restClientParameters.RequestMethod];
        }

        private static DataFormat GetDataFormat(RestClientParameters restClientParameters)
        {
            return restClientParameters.RequestFormat == RequestFormat.Xml
                ? DataFormat.Xml
                : DataFormat.Json;
        }

        private static void SetRequestParameters(IRestRequest request, Dictionary<string, string> parameters, ParameterType type)
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

        private static void SetRequestBody(IRestRequest request, RestClientParameters restClientParameters)
        {
            if (restClientParameters.RequestBody != null)
            {
                request.AddBody(restClientParameters.RequestBody);
            }
        }
    }
}