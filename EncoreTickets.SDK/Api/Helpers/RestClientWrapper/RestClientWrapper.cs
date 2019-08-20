using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using Polly;
using RestSharp;
using RestSharp.Authenticators;

namespace EncoreTickets.SDK.Api.Helpers.RestClientWrapper
{
    internal class RestClientWrapper
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

        public RestClientWrapper(RestClientWrapperCredentials restClientWrapperCredentials)
        {
            credentials = restClientWrapperCredentials;
        }

        public IRestClient GetRestClient(RestClientParameters restClientParameters)
        {
            var client = new RestClient(restClientParameters.BaseUrl)
            {
                Authenticator = GetAuthenticator()
            };
            return client;
        }

        public IRestRequest GetRestRequest(RestClientParameters restClientParameters)
        {
            var request = new RestRequest(restClientParameters.RequestUrl)
            {
                Method = GetRequestMethod(restClientParameters),
                RequestFormat = GetDataFormat(restClientParameters)
            };
            SetRequestParameters(request, restClientParameters.RequestHeaders, ParameterType.HttpHeader);
            SetRequestParameters(request, restClientParameters.RequestUrlSegments, ParameterType.UrlSegment);
            SetRequestParameters(request, restClientParameters.RequestQueryParameters, ParameterType.QueryString);
            SetRequestBody(request, restClientParameters);
            return request;
        }

        public IRestResponse Execute(RestClientParameters restClientParameters)
        {
            var client = GetRestClient(restClientParameters);
            var request = GetRestRequest(restClientParameters);
            return Execute(client, request);
        }

        public IRestResponse Execute(IRestClient client, IRestRequest request)
        {
            var response = Policy
                .Handle<Exception>()
                .OrResult<IRestResponse>(resp => !IsGoodResponse(resp))
                .Retry(MaxExecutionsCount)
                .Execute(() => client.Execute(request));
            return response;
        }

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