using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RestClientBuilder;
using EncoreTickets.SDK.Utilities.RestClientWrapper;
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
            string bodyInJson = null,
            Dictionary<string, object> expectedQueryParameters = null,
            Dictionary<string, object> expectedHeaders = null)
            where T : class, new()
        {
            VerifyExecution<T>(Times.Once(), baseUrl, resource, method, bodyInJson, expectedQueryParameters, expectedHeaders);
        }

        public void VerifyExecution<T>(
            Times times,
            string baseUrl,
            string resource,
            Method method,
            string bodyInJson = null,
            Dictionary<string, object> expectedQueryParameters = null,
            Dictionary<string, object> expectedHeaders = null)
            where T : class, new()
        {
            RestClientWrapperMock.Verify(
                x => x.Execute<T>(
                    It.Is<IRestClient>(client =>
                        baseUrl.Equals(client.BaseUrl.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    ),
                    It.Is<IRestRequest>(request =>
                        request.Resource.Equals(resource, StringComparison.InvariantCultureIgnoreCase) &&
                        request.Method == method &&
                        request.RequestFormat == DataFormat.Json &&
                        AreQueryParametersInRequest(request, expectedQueryParameters) &&
                        AreHeadersInRequest(request, expectedHeaders) &&
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
            return AreParametersInRequest(request, expectedQueryParameters,
                p => p.Type == ParameterType.QueryString || p.Type == ParameterType.QueryStringWithoutEncode);
        }

        private bool AreHeadersInRequest(IRestRequest request, Dictionary<string, object> expectedHeaders)
        {
            return AreParametersInRequest(request, expectedHeaders,
                p => p.Type == ParameterType.HttpHeader &&
                     !p.Name.Equals("x-sdk", StringComparison.InvariantCultureIgnoreCase));
        }

        private bool AreParametersInRequest(IRestRequest request, Dictionary<string, object> expectedParameters,
            Func<Parameter, bool> getCertainParamsFromRequestFunc)
        {
            var parameters = request.Parameters.Where(getCertainParamsFromRequestFunc).ToList();
            if (expectedParameters == null)
            {
                return !parameters.Any();
            }

            return expectedParameters.Count == parameters.Count &&
                   expectedParameters.All(x => IsParameterInRequest(parameters, x.Key, x.Value));
        }

        private bool IsParameterInRequest(IEnumerable<Parameter> parameters, string expectedParameterName, object expectedParameterValue)
        {
            var parameter = parameters.FirstOrDefault(x =>
                x.Name.Equals(expectedParameterName, StringComparison.InvariantCultureIgnoreCase));
            if (parameter == null)
            {
                return false;
            }

            var expectedParameter = parameter.Value.ToString();
            return expectedParameter.Equals(expectedParameterValue.ToString());
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