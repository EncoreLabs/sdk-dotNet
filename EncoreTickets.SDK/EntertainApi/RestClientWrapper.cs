using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;

namespace EncoreTickets.SDK.EntertainApi
{
    public class RestClientWrapper
    {
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

        private readonly string username;
        private readonly string password;

        public RestClientWrapper(RestClientWrapperCredentials restClientWrapperCredentials)
        {
            username = restClientWrapperCredentials.Username;
            password = restClientWrapperCredentials.Password;
        }

        public IRestResponse Execute(RestClientParameters restClientParameters)
        {
            var client = new RestClient { Authenticator = new HttpBasicAuthenticator(username, password) };
            var request = GetRestRequest(restClientParameters);
            return Execute(client, request);
        }

        public IRestRequest GetRestRequest(RestClientParameters restClientParameters)
        {
            var request = new RestRequest(restClientParameters.RequestUrl);
            SetRequestParameters(request, restClientParameters);
            SetRequestHeaders(request, restClientParameters);
            SetRequestBody(request, restClientParameters);
            request.Method = GetRequestMethod(restClientParameters);
            request.RequestFormat = GetDataFormat(restClientParameters);
            return request;
        }

        public IRestResponse Execute(IRestClient client, IRestRequest request)
        {
            const int maxAttemptsCount = 2;
            var attempts = 0;
            IRestResponse response = null;

            while (attempts < maxAttemptsCount)
            {
                response = client.Execute(request);
                attempts++;

                // Keep trying until we get a good response, quit after 2 tries and just return what was returned
                if (IsGoodResponse(response))
                {
                    break;
                }
            }
            return response;
        }

        public bool IsGoodResponse(IRestResponse response)
        {
            if (response.ErrorException != null || !string.IsNullOrEmpty(response.ErrorMessage))
            {
                return false;
            }
            return SuccessfulStatusCodes.Contains(response.StatusCode);
        }

        private void SetRequestParameters(IRestRequest request, RestClientParameters restClientParameters)
        {
            if (restClientParameters.RequestParams == null)
            {
                return;
            }

            foreach (var param in restClientParameters.RequestParams)
            {
                request.AddUrlSegment(param.Key, param.Value);
            }
        }

        private void SetRequestHeaders(IRestRequest request, RestClientParameters restClientParameters)
        {
            if (restClientParameters.RequestHeaders == null)
            {
                return;
            }

            foreach (var header in restClientParameters.RequestHeaders)
            {
                request.AddHeader(header.Key, header.Value);
            }
        }

        private void SetRequestBody(IRestRequest request, RestClientParameters restClientParameters)
        {
            if (restClientParameters.RequestBody != null)
            {
                request.AddBody(restClientParameters.RequestBody);
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
    }
}