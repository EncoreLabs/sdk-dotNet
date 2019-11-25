using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Helpers.RestClientWrapper;
using Moq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiRestClientWrapperTests
    {
        private static readonly object[] SourceForGetRestClientTest =
        {
            new object[]
            {
                new RestClientWrapperCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "token",
                    AuthenticationMethod = AuthenticationMethod.JWT
                },
                new RestClientParameters
                {
                    BaseUrl = @"Http://test.com"
                },
                new JwtAuthenticator("token"), 
            },
            new object[]
            {
                new RestClientWrapperCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = null,
                    AuthenticationMethod = AuthenticationMethod.JWT
                },
                new RestClientParameters
                {
                    BaseUrl = @"Http://test.com"
                },
                null,
            },
            new object[]
            {
                new RestClientWrapperCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "token",
                    AuthenticationMethod = AuthenticationMethod.Basic
                },
                new RestClientParameters
                {
                    BaseUrl = @"Http://test.com"
                },
                new HttpBasicAuthenticator("username", "password"),
            },
            new object[]
            {
                new RestClientWrapperCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "token",
                    AuthenticationMethod = (AuthenticationMethod)190000000
                },
                new RestClientParameters
                {
                    BaseUrl = @"Http://test.com"
                },
                null,
            },
            new object[]
            {
                null,
                new RestClientParameters
                {
                    BaseUrl = @"Http://test.com"
                },
                null,
            },
        };

        private static readonly object[] SourceForGetRestRequestToCheckMethodTest =
        {
            new object[]
            {
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Get,
                },
                "Get",
            },
            new object[]
            {
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Post,
                },
                "Post",
            },
            new object[]
            {
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Put,
                },
                "Put",
            },
            new object[]
            {
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Delete,
                },
                "Delete",
            },
            new object[]
            {
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Patch,
                },
                "Patch",
            },
        };

        private static readonly object[] SourceForGetRestRequestToCheckFormatTest =
        {
            new object[]
            {
                new RestClientParameters
                {
                    RequestFormat = RequestFormat.Json
                },
                "Json",
            },
            new object[]
            {
                new RestClientParameters
                {
                    RequestFormat = RequestFormat.Xml
                },
                "Xml",
            },
            new object[]
            {
                new RestClientParameters
                {
                    RequestFormat = (RequestFormat) 1900909
                },
                "Json",
            },
        };

        private static readonly object[] SourceForGetRestRequestToCheckHeadersTest =
        {
            new RestClientParameters
            {
                RequestHeaders = new Dictionary<string, string>
                {
                    { "x-SDK", "EncoreTickets.SDK.NET" },
                    { "x-apply-price-engine", "true" },
                    { "x-market", "broadway" },
                },
            },
            new RestClientParameters
            {
                RequestHeaders = null,
            },
        };

        private static readonly object[] SourceForGetRestRequestToCheckUrlSegmentsTest =
        {
            new RestClientParameters
            {
                RequestUrlSegments = new Dictionary<string, string>
                {
                    { "version", "v1" },
                    { "id", "1000" },
                    { "productId", "2000" },
                },
            },
            new RestClientParameters
            {
                RequestUrlSegments = null,
            },
        };

        private static readonly object[] SourceForGetRestRequestToCheckQueryParamsTest =
        {
            new RestClientParameters
            {
                RequestQueryParameters = new Dictionary<string, string>
                {
                    { "date", "12/12/2000" },
                    { "time", "10:00" },
                    { "page", "1" },
                },
            },
            new RestClientParameters
            {
                RequestQueryParameters = null,
            },
        };

        private static readonly object[] SourceForGetRestRequestToCheckBodyTest =
        {
            new object[]
            {
                new RestClientParameters
                {
                    RequestBody = new object(),
                    RequestFormat = RequestFormat.Json
                },
                "application/json"
            },
            new object[]
            {
                new RestClientParameters
                {
                    RequestBody = 4,
                    RequestFormat = RequestFormat.Xml
                },
                "application/xml"
            },
            new object[]
            {
                new RestClientParameters
                {
                    RequestBody = null,
                    RequestFormat = RequestFormat.Xml
                },
                null
            },
        };

        private static readonly object[] SourceForIsGoodResponseTest =
        {
            new object[]
            {
                new RestResponse
                {
                    ErrorException = new Exception(),
                    StatusCode = It.IsAny<HttpStatusCode>()
                },
                false
            },
            new object[]
            {
                new RestResponse
                {
                    ErrorMessage = "test",
                    StatusCode = It.IsAny<HttpStatusCode>()
                },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.OK },
                true
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.Moved },
                true
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.NoContent },
                true
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.PaymentRequired },
                true
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.Redirect },
                true
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.RedirectMethod },
                true
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.TemporaryRedirect },
                true
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.Accepted },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.Ambiguous },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.AlreadyReported },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.BadGateway },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.BadRequest },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.ExpectationFailed },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.Forbidden },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.GatewayTimeout },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.Gone },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.HttpVersionNotSupported },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.InternalServerError },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.LengthRequired },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.MethodNotAllowed },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.NotAcceptable },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.NotFound },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.NotImplemented },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.NotModified },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.PartialContent },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.PreconditionFailed },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.ProxyAuthenticationRequired },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.RequestedRangeNotSatisfiable },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.RequestEntityTooLarge },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.RequestTimeout },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.RequestUriTooLong },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.ServiceUnavailable },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.Unauthorized },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.UnsupportedMediaType },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.Unused },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.UpgradeRequired },
                false
            },
            new object[]
            {
                new RestResponse { StatusCode = HttpStatusCode.UseProxy },
                false
            },
        };

        private static readonly object[] SourceForExecuteTest =
        {
            new object[]
            {
                new RestResponse
                {
                    StatusCode = HttpStatusCode.OK
                },
                true
            },
            new object[]
            {
                new RestResponse
                {
                    StatusCode = HttpStatusCode.BadRequest
                },
                false
            },
        };

        private static readonly object[] SourceForGenericExecuteTest =
        {
            new object[]
            {
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.OK
                },
                true
            },
            new object[]
            {
                new RestResponse<TestObject>
                {
                    StatusCode = HttpStatusCode.BadRequest
                },
                false
            },
        };

        [TestCaseSource(nameof(SourceForGetRestClientTest))]
        public void Api_RestClientWrapper_GetRestClient_ReturnsCorrectRestClient(
            RestClientWrapperCredentials credentials, RestClientParameters restClientParameters, IAuthenticator expectedAuthenticator)
        {
            var wrapper = new RestClientWrapper(credentials);
            var result = wrapper.GetRestClient(restClientParameters);
            Assert.AreEqual(restClientParameters.BaseUrl, result.BaseUrl.OriginalString);

            if (expectedAuthenticator == null)
            {
                Assert.AreEqual(expectedAuthenticator, result.Authenticator);
            }
            else
            {
                Assert.AreEqual(expectedAuthenticator.GetType(), result.Authenticator.GetType());
            }
        }

        [Test]
        public void Api_RestClientWrapper_GetRestRequest_ReturnsCorrectRequestWithCorrectUrl()
        {
            var restClientParameters = new RestClientParameters {RequestUrl = "v1/attributes/standard",};
            var restClientWrapper = new RestClientWrapper(new RestClientWrapperCredentials());
            var result = restClientWrapper.GetRestRequest(restClientParameters);
            Assert.AreEqual(restClientParameters.RequestUrl, result.Resource);
        }

        [TestCaseSource(nameof(SourceForGetRestRequestToCheckMethodTest))]
        public void Api_RestClientWrapper_GetRestRequest_ReturnsCorrectRequestWithCorrectMethod(RestClientParameters restClientParameters,
            string expectedMethod)
        {
            var restClientWrapper = new RestClientWrapper(new RestClientWrapperCredentials());
            var result = restClientWrapper.GetRestRequest(restClientParameters);
            Assert.AreEqual(expectedMethod.ToLower(), result.Method.ToString().ToLower());
        }

        [TestCaseSource(nameof(SourceForGetRestRequestToCheckFormatTest))]
        public void Api_RestClientWrapper_GetRestRequest_ReturnsCorrectRequestWithCorrectFormat(RestClientParameters restClientParameters,
            string expectedFormat)
        {
            var restClientWrapper = new RestClientWrapper(new RestClientWrapperCredentials());
            var result = restClientWrapper.GetRestRequest(restClientParameters);
            Assert.AreEqual(expectedFormat.ToLower(), result.RequestFormat.ToString().ToLower());
        }

        [TestCaseSource(nameof(SourceForGetRestRequestToCheckHeadersTest))]
        public void Api_RestClientWrapper_GetRestRequest_ReturnsCorrectRequestWithCorrectHeaders(RestClientParameters restClientParameters)
        {
            var restClientWrapper = new RestClientWrapper(new RestClientWrapperCredentials());
            var result = restClientWrapper.GetRestRequest(restClientParameters);

            var headers = result.Parameters.Where(x => x.Type == ParameterType.HttpHeader);
            var expectedHeadersCount = restClientParameters.RequestHeaders?.Count ?? 0;
            Assert.AreEqual(expectedHeadersCount, headers.Count());
            foreach (var header in headers)
            {
                Assert.IsTrue(restClientParameters.RequestHeaders.Keys.Contains(header.Name));
                Assert.AreEqual(restClientParameters.RequestHeaders[header.Name], header.Value);
            }
        }

        [TestCaseSource(nameof(SourceForGetRestRequestToCheckUrlSegmentsTest))]
        public void Api_RestClientWrapper_GetRestRequest_ReturnsCorrectRequestWithCorrectUrlSegments(RestClientParameters restClientParameters)
        {
            var restClientWrapper = new RestClientWrapper(new RestClientWrapperCredentials());
            var result = restClientWrapper.GetRestRequest(restClientParameters);

            var urlSegments = result.Parameters.Where(x => x.Type == ParameterType.UrlSegment);
            var expectedParamsCount = restClientParameters.RequestUrlSegments?.Count ?? 0;
            Assert.AreEqual(expectedParamsCount, urlSegments.Count());
            foreach (var urlSegment in urlSegments)
            {
                Assert.IsTrue(restClientParameters.RequestUrlSegments.Keys.Contains(urlSegment.Name));
                Assert.AreEqual(restClientParameters.RequestUrlSegments[urlSegment.Name], urlSegment.Value);
            }
        }

        [TestCaseSource(nameof(SourceForGetRestRequestToCheckQueryParamsTest))]
        public void Api_RestClientWrapper_GetRestRequest_ReturnsCorrectRequestWithCorrectQueryParams(RestClientParameters restClientParameters)
        {
            var restClientWrapper = new RestClientWrapper(new RestClientWrapperCredentials());
            var result = restClientWrapper.GetRestRequest(restClientParameters);

            var queryParams = result.Parameters.Where(x => x.Type == ParameterType.QueryString);
            var expectedQueryParamsCount = restClientParameters.RequestQueryParameters?.Count ?? 0;
            Assert.AreEqual(expectedQueryParamsCount, queryParams.Count());
            foreach (var param in queryParams)
            {
                Assert.IsTrue(restClientParameters.RequestQueryParameters.Keys.Contains(param.Name));
                Assert.AreEqual(restClientParameters.RequestQueryParameters[param.Name], param.Value);
            }
        }

        [TestCaseSource(nameof(SourceForGetRestRequestToCheckBodyTest))]
        public void Api_RestClientWrapper_GetRestRequest_ReturnsCorrectRequestWithCorrectBody(
            RestClientParameters restClientParameters, string expectedBodyType)
        {
            //Arrange
            var expectedBodyCount = restClientParameters.RequestBody != null ? 1 : 0;
            var credentials = new RestClientWrapperCredentials();
            var restClientWrapper = new RestClientWrapper(credentials);

            //Act
            var result = restClientWrapper.GetRestRequest(restClientParameters);

            //Assert
            var body = result.Parameters.Where(x => x.Type == ParameterType.RequestBody);
            Assert.AreEqual(expectedBodyCount, body.Count());
            Assert.AreEqual(expectedBodyType, body.FirstOrDefault()?.ContentType);
        }

        [TestCaseSource(nameof(SourceForIsGoodResponseTest))]
        public void Api_RestClientWrapper_IsGoodResponse_ReturnsCorrectly(RestResponse response, bool expected)
        {
            var restClientWrapper = new RestClientWrapper(new RestClientWrapperCredentials());
            var result = restClientWrapper.IsGoodResponse(response);
            Assert.AreEqual(expected, result);
        }

        [TestCaseSource(nameof(SourceForExecuteTest))]
        public void Api_RestClientWrapper_Execute_TriesToExecute(RestResponse response, bool expectedFromOneAttempt)
        {
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response);
            var restClientWrapper = new RestClientWrapper(new RestClientWrapperCredentials());

            restClientWrapper.Execute(clientMock.Object, It.IsAny<IRestRequest>());

            var times = expectedFromOneAttempt ? Times.Once() : Times.AtLeastOnce();
            clientMock.Verify(mock => mock.Execute(It.IsAny<IRestRequest>()), times);
        }

        [TestCaseSource(nameof(SourceForGenericExecuteTest))]
        public void Api_RestClientWrapper_GenericExecute_TriesToExecute<T>(RestResponse<T> response, bool expectedFromOneAttempt)
            where T : class, new()
        {
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute<T>(It.IsAny<IRestRequest>())).Returns(response);
            var restClientWrapper = new RestClientWrapper(new RestClientWrapperCredentials());

            restClientWrapper.Execute<T>(clientMock.Object, It.IsAny<IRestRequest>());

            var times = expectedFromOneAttempt ? Times.Once() : Times.AtLeastOnce();
            clientMock.Verify(mock => mock.Execute<T>(It.IsAny<IRestRequest>()), times);
        }
    }
}
