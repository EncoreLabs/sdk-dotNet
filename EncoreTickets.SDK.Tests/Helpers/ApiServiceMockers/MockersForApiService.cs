using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers.ApiRestClientBuilder;
using EncoreTickets.SDK.Utilities.Common.RestClientWrapper;
using Moq;
using RestSharp;

namespace EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers
{
    internal class MockersForApiService
    {
        public Mock<ApiRestClientBuilder> RestClientBuilderMock;

        public Mock<RestClientWrapper> RestClientWrapperMock;

        public MockersForApiService()
        {
            RestClientWrapperMock = GetRestClientWrapperMock();
            RestClientBuilderMock = GetApiRestClientBuilderMock(RestClientWrapperMock);
        }

        public void SetupAnyExecution<T>()
            where T : class, new()
        {
            RestClientWrapperMock
                .Setup(x => x.Execute<T>(It.IsAny<IRestClient>(), It.IsAny<IRestRequest>()))
                .Returns((IRestClient client, IRestRequest request) => new RestResponse<T>());
        }

        public void SetupSuccessfulExecution<T>(string responseContent)
            where T : class, new()
        {
            RestClientWrapperMock
                .Setup(x => x.Execute<T>(It.IsAny<IRestClient>(), It.IsAny<IRestRequest>()))
                .Returns((IRestClient client, IRestRequest request) =>
                    RestResponseFactory.GetSuccessJsonResponse<T>(client, request, responseContent));
        }

        public void SetupFailedExecution<T>(string responseContent, HttpStatusCode code)
            where T : class, new()
        {
            RestClientWrapperMock
                .Setup(x => x.Execute<T>(It.IsAny<IRestClient>(), It.IsAny<IRestRequest>()))
                .Returns((IRestClient client, IRestRequest request) =>
                    RestResponseFactory.GetFailedJsonResponse<T>(client, request, responseContent, code));
        }

        public void VerifyExecution<T>(
            string baseUrl,
            string resource,
            Method method,
            Dictionary<string, object> expectedQueryParameters = null,
            string bodyInJson = null)
            where T : class, new()
        {
            VerifyExecution<T>(Times.Once(), baseUrl, resource, method, expectedQueryParameters, bodyInJson);
        }

        public void VerifyExecution<T>(
            Times times,
            string baseUrl,
            string resource,
            Method method,
            Dictionary<string, object> expectedQueryParameters = null,
            string bodyInJson = null)
            where T : class, new()
        {
            RestClientWrapperMock.Verify(
                x => x.Execute<T>(
                    It.Is<IRestClient>(client =>
                        baseUrl.Equals(client.BaseUrl.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    ),
                    It.Is<IRestRequest>(request =>
                        request.Method == method &&
                        request.Resource.Equals(resource, StringComparison.InvariantCultureIgnoreCase) &&
                        request.RequestFormat == DataFormat.Json &&
                        AreQueryParametersInRequest(request, expectedQueryParameters) &&
                        IsJsonBodyInRequest(request, bodyInJson))
                ), times);
        }

        private Mock<RestClientWrapper> GetRestClientWrapperMock()
        {
            return new Mock<RestClientWrapper>(MockBehavior.Loose);
        }

        private Mock<ApiRestClientBuilder> GetApiRestClientBuilderMock(Mock<RestClientWrapper> restClientWrapperMock)
        {
            var restClientBuilderMock = new Mock<ApiRestClientBuilder>();
            restClientBuilderMock
                .Setup(x => x.CreateClientWrapper(It.IsAny<ApiContext>()))
                .Returns(() => restClientWrapperMock.Object);
            return restClientBuilderMock;
        }

        private bool AreQueryParametersInRequest(IRestRequest request,
            Dictionary<string, object> expectedQueryParameters)
        {
            var queryParameters = request.Parameters.Where(p => p.Type == ParameterType.QueryString || p.Type == ParameterType.QueryStringWithoutEncode);
            if (expectedQueryParameters == null)
            {
                return !queryParameters.Any();
            }

            if (expectedQueryParameters.Count != queryParameters.Count())
            {
                return false;
            }

            foreach (var (key, value) in expectedQueryParameters)
            {
                var parameter = queryParameters.FirstOrDefault(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                if (parameter == null || !parameter.Value.ToString().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                var expectedParameter = parameter.Value.ToString();
                if (!expectedParameter.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsJsonBodyInRequest(IRestRequest request, string expectedBodyInJson)
        {
            var bodyParameter = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);
            if (expectedBodyInJson == null)
            {
                return bodyParameter == null;
            }

            if (bodyParameter == null)
            {
                return false;
            }

            var serializedBody = request.JsonSerializer.Serialize(bodyParameter.Value);
            return serializedBody == expectedBodyInJson;
        }
    }
}