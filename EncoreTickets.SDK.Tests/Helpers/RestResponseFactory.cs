using System.Net;
using RestSharp;
using RestSharp.Serialization;

namespace EncoreTickets.SDK.Tests.Helpers
{
    internal static class RestResponseFactory
    {
        public static IRestResponse GetSuccessResponse()
        {
            var response = GetSuccessResponse<object>();
            return response;
        }

        public static IRestResponse<T> GetSuccessJsonResponse<T>(IRestClient client, IRestRequest request, string content)
        {
            var response = GetSuccessResponse<T>();
            return GetJsonResponseWithData(response, client, request, content);
        }

        public static IRestResponse<T> GetFailedJsonResponse<T>(IRestClient client, IRestRequest request, string content, HttpStatusCode code)
        {
            var response = GetFailedResponse<T>(code);
            return GetJsonResponseWithData(response, client, request, content);
        }

        public static IRestResponse GetFailedResponse()
        {
            return GetFailedResponse<object>(HttpStatusCode.InternalServerError);
        }

        private static IRestResponse<T> GetSuccessResponse<T>()
        {
            return new RestResponse<T>
            {
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = HttpStatusCode.OK,
            };
        }

        private static IRestResponse<T> GetFailedResponse<T>(HttpStatusCode code)
        {
            return new RestResponse<T>
            {
                ResponseStatus = ResponseStatus.Error,
                StatusCode = code,
            };
        }

        private static IRestResponse<T> GetJsonResponseWithData<T>(
            IRestResponse<T> response,
            IRestClient client,
            IRestRequest request,
            string content)
        {
            response.Content = content;
            response.Request = request;
            response.ContentType = ContentType.Json;
            var responseWithDeserializedData = client.Deserialize<T>(response);
            responseWithDeserializedData.Content = content;
            return responseWithDeserializedData;
        }
    }
}
