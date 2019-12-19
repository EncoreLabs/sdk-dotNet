using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers.ApiRestClientBuilder;
using EncoreTickets.SDK.Utilities.Common.RestClientWrapper;
using Moq;
using RestSharp;

namespace EncoreTickets.SDK.Tests.Helpers
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

        public void VerifyExecution<T>(string baseUrl, string resource, Method method)
            where T : class, new()
        {
            RestClientWrapperMock.Verify(
                x => x.Execute<T>(
                    It.Is<IRestClient>(client =>
                        client.BaseUrl.ToString() == baseUrl
                    ),
                    It.Is<IRestRequest>(request =>
                        request.Method == method &&
                        request.Resource == resource &&
                        request.RequestFormat == DataFormat.Json)
                ), Times.Once());
        }

        private Mock<RestClientWrapper> GetRestClientWrapperMock()
        {
            return new Mock<RestClientWrapper>();
        }

        private Mock<ApiRestClientBuilder> GetApiRestClientBuilderMock(Mock<RestClientWrapper> restClientWrapperMock)
        {
            var restClientBuilderMock = new Mock<ApiRestClientBuilder>();
            restClientBuilderMock
                .Setup(x => x.CreateClientWrapper(It.IsAny<ApiContext>()))
                .Returns(() => restClientWrapperMock.Object);
            return restClientBuilderMock;
        }
    }
}