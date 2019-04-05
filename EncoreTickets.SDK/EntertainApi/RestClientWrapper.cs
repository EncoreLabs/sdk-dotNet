using RestSharp;
using RestSharp.Authenticators;
using System.Configuration;
using System.Net;

namespace EncoreTickets.SDK.EntertainApi
{
    public class RestClientWrapper 
    {
        private readonly string _username;
        private readonly string _password;

        public RestClientWrapper(RestClientWrapperCredentials restClientWrapperCredentials)
        {
            _username = restClientWrapperCredentials.Username;
            _password = restClientWrapperCredentials.Password;
        }

        public IRestResponse Execute(RestClientParameters restClientParameters)
        {
            var attempts = 0;
            var client = new RestClient { Authenticator = new HttpBasicAuthenticator(_username, _password) };
            var request = new RestRequest(restClientParameters.RequestUrl);

 
            if (restClientParameters.RequestParams != null)
            {
                foreach (var param in restClientParameters.RequestParams)
                    request.AddUrlSegment(param.Key, param.Value);
            }

            if (restClientParameters.RequestHeaders != null)
            {
                foreach (var header in restClientParameters.RequestHeaders)
                    request.AddHeader(header.Key, header.Value);
            }

            if (restClientParameters.RequestBody != null)
                request.AddBody(restClientParameters.RequestBody);

            switch (restClientParameters.RequestMethod)
            {
                case RequestMethod.Get:
                    request.Method = Method.GET;
                    break;
                case RequestMethod.Post:
                    request.Method = Method.POST;
                    break;
                case RequestMethod.Put:
                    request.Method = Method.PUT;
                    break;
                case RequestMethod.Delete:
                    request.Method = Method.DELETE;
                    break;
            }

            request.RequestFormat = restClientParameters.RequestFormat == RequestFormat.Xml ? DataFormat.Xml : DataFormat.Json;

            IRestResponse response = null;

            while (attempts < 2)
            {
                response = client.Execute(request);
                attempts++;

                // Keep trying until we get a good response, quit after 2 tries and just return what was returned
                if (IsGoodResponse(response))
                    break;
            }

            return response;
        }

        public bool IsGoodResponse(IRestResponse response)
        {
            if (response.ErrorException != null || !string.IsNullOrEmpty(response.ErrorMessage))
                return false;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Moved:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.PaymentRequired:
                case HttpStatusCode.Redirect:
                case HttpStatusCode.RedirectMethod:
                case HttpStatusCode.TemporaryRedirect:
                    return true;
                case HttpStatusCode.Ambiguous:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Conflict:
                case HttpStatusCode.ExpectationFailed:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.GatewayTimeout:
                case HttpStatusCode.Gone:
                case HttpStatusCode.HttpVersionNotSupported:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.LengthRequired:
                case HttpStatusCode.MethodNotAllowed:
                case HttpStatusCode.NotAcceptable:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.NotImplemented:
                case HttpStatusCode.NotModified:
                case HttpStatusCode.PartialContent:
                case HttpStatusCode.PreconditionFailed:
                case HttpStatusCode.ProxyAuthenticationRequired:
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                case HttpStatusCode.RequestEntityTooLarge:
                case HttpStatusCode.RequestTimeout:
                case HttpStatusCode.RequestUriTooLong:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.UnsupportedMediaType:
                case HttpStatusCode.Unused:
                case HttpStatusCode.UpgradeRequired:
                case HttpStatusCode.UseProxy:
                    return false;
                default:
                    return false;
            }
        }
    }
}