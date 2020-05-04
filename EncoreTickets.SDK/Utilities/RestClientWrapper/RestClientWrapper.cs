using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Utilities.Mapping;
using Polly;
using RestSharp;
using RestSharp.Authenticators;
using DataFormat = EncoreTickets.SDK.Utilities.Enums.DataFormat;

namespace EncoreTickets.SDK.Utilities.RestClientWrapper
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

        public int MaxExtraAttemptsCount { get; }

        public RestClientCredentials Credentials { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="RestClientWrapper"/>
        /// </summary>
        /// <param name="restClientCredentials">Credentials for requests.</param>
        public RestClientWrapper(RestClientCredentials restClientCredentials)
            : this(DefaultMaxExecutionsCount)
        {
            Credentials = restClientCredentials;
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
            MaxExtraAttemptsCount = executionsCount > 1 ? executionsCount - 1 : 0;
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
            AddResponseHandlers(client, restClientParameters);
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
                Method = restClientParameters.RequestMethod.Map<RequestMethod, Method>(),
                RequestFormat = restClientParameters.RequestDataFormat.Map<DataFormat, RestSharp.DataFormat>()
            };
            SetRequestParameters(request, restClientParameters.RequestHeaders, ParameterType.HttpHeader);
            SetRequestParameters(request, restClientParameters.RequestUrlSegments, ParameterType.UrlSegment);
            SetRequestParameters(request, restClientParameters.RequestQueryParameters, ParameterType.QueryString);
            SetRequestBody(request, restClientParameters);
            SetRequestSerializer(request, restClientParameters);
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
                .Retry(MaxExtraAttemptsCount)
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
                .Retry(MaxExtraAttemptsCount)
                .Execute(() => client.Execute<T>(request));
            return response;
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
                case AuthenticationMethod.PredefinedJWT:
                    return string.IsNullOrWhiteSpace(Credentials.AccessToken)
                        ? null
                        : new JwtAuthenticator(Credentials.AccessToken);
                case AuthenticationMethod.Basic:
                    return new HttpBasicAuthenticator(Credentials.Username, Credentials.Password);
                default:
                    return null;
            }
        }

        private void AddResponseHandlers(IRestClient client, RestClientParameters restClientParameters)
        {
            var contentType = DataFormatHelper.ToContentType(restClientParameters.ResponseDataFormat);
            if (contentType != null)
            {
                client.AddHandler(contentType, () => restClientParameters.ResponseDataDeserializer);
            }
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

        private void SetRequestSerializer(IRestRequest request, RestClientParameters restClientParameters)
        {
            if (restClientParameters.RequestDataFormat == DataFormat.Json &&
                restClientParameters.RequestDataSerializer != default)
            {
                request.JsonSerializer = restClientParameters.RequestDataSerializer;
            }
        }

        private bool IsGoodResponse(IRestResponse response)
        {
            if (response.ErrorException != null || !string.IsNullOrEmpty(response.ErrorMessage))
            {
                return false;
            }

            return response.IsSuccessful || SuccessfulStatusCodes.Contains(response.StatusCode);
        }
    }
}