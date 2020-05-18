using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Utilities.RestClientWrapper;
using EncoreTickets.SDK.Utilities.Serializers;
using EncoreTickets.SDK.Utilities.Serializers.Converters;
using Moq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using DataFormat = EncoreTickets.SDK.Utilities.Enums.DataFormat;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.RestClientWrapper
{
    internal class RestClientWrapperTests
    {
        private const string TestValidUrl = @"Http://test.com";

        [Test]
        public void GetRestClient_IfParametersIsNull_ThrowsException()
        {
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            Assert.Catch(() => restClientWrapper.GetRestClient(null));
        }

        [TestCase(@"Http://test.com")]
        [TestCase(@"Https://test.com")]
        [TestCase(@"Https://test.com/")]
        public void GetRestClient_IfUrlIsValid_ReturnsCorrectlyWithInitializedUrl(string baseUrl)
        {
            var wrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(It.IsAny<RestClientCredentials>());
            var restClientParameters = new RestClientParameters {BaseUrl = baseUrl};

            var result = wrapper.GetRestClient(restClientParameters);

            Assert.AreEqual(restClientParameters.BaseUrl, result.BaseUrl.OriginalString);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase("\t")]
        [TestCase("test")]
        [TestCase("test.com")]
        [TestCase("/")]
        public void GetRestClient_IfUrlIsInvalid_ThrowsException(string baseUrl)
        {
            var wrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(It.IsAny<RestClientCredentials>());
            var restClientParameters = new RestClientParameters { BaseUrl = baseUrl };

            Assert.Catch<Exception>(() => wrapper.GetRestClient(restClientParameters));
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestClient_IfAuthenticatorCannotBeCreated_ReturnsCorrectlyWithAuthenticatorAsNull))]
        public void GetRestClient_IfAuthenticatorCannotBeCreated_ReturnsCorrectlyWithAuthenticatorAsNull(
            RestClientCredentials credentials)
        {
            var wrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(credentials);
            var restClientParameters = new RestClientParameters {BaseUrl = TestValidUrl};

            var result = wrapper.GetRestClient(restClientParameters);

            Assert.Null(result.Authenticator);
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestClient_IfAuthenticatorCanBeCreated_ReturnsCorrectlyWithAuthenticator))]
        public void GetRestClient_IfAuthenticatorCanBeCreated_ReturnsCorrectlyWithAuthenticator(
            RestClientCredentials credentials,
            IAuthenticator expectedAuthenticator,
            string expectedHeader)
        {
            var wrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(credentials);
            var restClientParameters = new RestClientParameters { BaseUrl = TestValidUrl };
            var request = new RestRequest();

            var result = wrapper.GetRestClient(restClientParameters);
            result.Authenticator?.Authenticate(It.IsAny<IRestClient>(), request);

            Assert.IsInstanceOf(expectedAuthenticator.GetType(), result.Authenticator);
            var authParam = request.Parameters.FirstOrDefault(x =>
                x.Type == ParameterType.HttpHeader && x.Name == "Authorization");
            Assert.NotNull(authParam);
            Assert.AreEqual(expectedHeader, authParam.Value);
        }

        [TestCase(100000)]
        public void GetRestClient_IDataFormatDoesNotExist_ThrowsException(DataFormat responseDataFormat)
        {
            var wrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(It.IsAny<RestClientCredentials>());
            var restClientParameters = new RestClientParameters
            {
                BaseUrl = TestValidUrl,
                ResponseDataFormat = responseDataFormat
            };

            Assert.Catch(() => wrapper.GetRestClient(restClientParameters));
        }

        [Test]
        public void GetRestRequest_IfParametersIsNull_ThrowsException()
        {
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            Assert.Catch(() => restClientWrapper.GetRestRequest(null));
        }

        [TestCase("v1/attributes/standard", "v1/attributes/standard")]
        [TestCase("", "")]
        [TestCase(null, "")]
        public void GetRestRequest_ReturnsRequestWithCorrectUrl(string url, string expectedResource)
        {
            var restClientParameters = new RestClientParameters { RequestUrl = url, };
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            var result = restClientWrapper.GetRestRequest(restClientParameters);

            Assert.AreEqual(expectedResource, result.Resource); 
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestRequest_IfRequestMethodExists_ReturnsRequestWithCorrectMethod))]
        public void GetRestRequest_IfRequestMethodExists_ReturnsRequestWithCorrectMethod(
            RestClientParameters restClientParameters,
            Method expectedMethod)
        {
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            var result = restClientWrapper.GetRestRequest(restClientParameters);

            Assert.AreEqual(expectedMethod, result.Method);
        }

        [TestCase(12901)]
        public void GetRestRequest_IfRequestMethodDoesNotExist_ThrowsException(
            RequestMethod method)
        {
            var restClientParameters = new RestClientParameters { RequestMethod = method };
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            Assert.Catch(() => restClientWrapper.GetRestRequest(restClientParameters));
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestRequest_IfDataFormatExists_ReturnsRequestWithCorrectRequestFormat))]
        public void GetRestRequest_IfDataFormatExists_ReturnsRequestWithCorrectRequestFormat(
            RestClientParameters restClientParameters,
            RestSharp.DataFormat expectedFormat)
        {
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            var result = restClientWrapper.GetRestRequest(restClientParameters);

            Assert.AreEqual(expectedFormat, result.RequestFormat);
        }

        [TestCase(10000)]
        public void GetRestRequest_IfDataFormatDoesNotExist_ThrowsException(
            DataFormat format)
        {
            var restClientParameters = new RestClientParameters{RequestDataFormat = format};
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            Assert.Catch(() => restClientWrapper.GetRestRequest(restClientParameters));
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestRequest_ReturnsRequestWithCorrectHeaders))]
        public void GetRestRequest_ReturnsRequestWithCorrectHeaders(RestClientParameters restClientParameters)
        {
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            var result = restClientWrapper.GetRestRequest(restClientParameters);

            var parameters = result.Parameters.Where(x => x.Type == ParameterType.HttpHeader);
            var expectedCount = restClientParameters.RequestHeaders?.Count ?? 0;
            Assert.AreEqual(expectedCount, parameters.Count());
            if (expectedCount <= 0)
            {
                return;
            }

            foreach (var (key, value) in restClientParameters.RequestHeaders)
            {
                var requestHeader = parameters.FirstOrDefault(x => x.Name == key);
                Assert.NotNull(requestHeader);
                Assert.AreEqual(value, requestHeader.Value);
            }
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestRequest_ReturnsRequestWithCorrectUrlSegments))]
        public void GetRestRequest_ReturnsRequestWithCorrectUrlSegments(RestClientParameters restClientParameters)
        {
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            var result = restClientWrapper.GetRestRequest(restClientParameters);

            var parameters = result.Parameters.Where(x => x.Type == ParameterType.UrlSegment);
            var expectedCount = restClientParameters.RequestUrlSegments?.Count ?? 0;
            Assert.AreEqual(expectedCount, parameters.Count());
            if (expectedCount <= 0)
            {
                return;
            }

            foreach (var (key, value) in restClientParameters.RequestUrlSegments)
            {
                var requestParameter = parameters.FirstOrDefault(x => x.Name == key);
                Assert.NotNull(requestParameter);
                Assert.AreEqual(value, requestParameter.Value);
            }
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestRequest_ReturnsRequestWithCorrectQueryParams))]
        public void GetRestRequest_ReturnsRequestWithCorrectQueryParams(RestClientParameters restClientParameters)
        {
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            var result = restClientWrapper.GetRestRequest(restClientParameters);

            var parameters = result.Parameters.Where(x => x.Type == ParameterType.QueryString);
            var expectedCount = restClientParameters.RequestQueryParameters?.Count ?? 0;
            Assert.AreEqual(expectedCount, parameters.Count());
            if (expectedCount <= 0)
            {
                return;
            }

            foreach (var (key, value) in restClientParameters.RequestQueryParameters)
            {
                var requestParameter = parameters.FirstOrDefault(x => x.Name == key);
                Assert.NotNull(requestParameter);
                Assert.AreEqual(value, requestParameter.Value);
            }
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestRequest_ReturnsRequestWithCorrectBody))]
        public void GetRestRequest_ReturnsRequestWithCorrectBody(
            RestClientParameters restClientParameters,
            string expectedBodyType)
        {
            var expectedBodyCount = restClientParameters.RequestBody != null ? 1 : 0;
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            var result = restClientWrapper.GetRestRequest(restClientParameters);

            var body = result.Parameters.Where(x => x.Type == ParameterType.RequestBody);
            Assert.AreEqual(expectedBodyCount, body.Count());
            Assert.AreEqual(expectedBodyType, body.FirstOrDefault()?.ContentType);
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestRequest_IfJsonSerializerIsSet_ReturnsRequestWithCustomSerializer))]
        public void GetRestRequest_IfJsonSerializerIsSet_ReturnsRequestWithCustomSerializer(RestClientParameters restClientParameters)
        {
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            var result = restClientWrapper.GetRestRequest(restClientParameters);

            Assert.AreEqual(RestSharp.DataFormat.Json, result.RequestFormat);
            Assert.AreEqual(restClientParameters.RequestDataSerializer,result.JsonSerializer);
            Assert.Null(result.XmlSerializer);
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GetRestRequest_IfJsonSerializerIsNotSet_ReturnsRequestWithDefaultSerializer))]
        public void GetRestRequest_IfJsonSerializerIsNotSet_ReturnsRequestWithDefaultSerializer(RestClientParameters restClientParameters)
        {
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            var result = restClientWrapper.GetRestRequest(restClientParameters);

            Assert.Null(result.JsonSerializer);
            Assert.Null(result.XmlSerializer);
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.Execute_IfMaxExecutionsCountIsNotSet_IfResponseSuccess_TriesToExecuteOnce))]
        public void Execute_IfResponseSuccess_TriesToExecuteOnce(RestResponse response)
        {
            var request = It.IsAny<IRestRequest>();
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute(request)).Returns(response);
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(100);

            restClientWrapper.Execute(clientMock.Object, request);

            clientMock.Verify(mock => mock.Execute(request), Times.Once);
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.Execute_IfMaxAttemptsCountIsNotSet_IfResponseFailed_TriesToExecuteAtLeastTwice))]
        public void Execute_IfMaxAttemptsCountIsNotSet_IfResponseFailed_TriesToExecuteAtLeastTwice(RestResponse response)
        {
            var request = It.IsAny<IRestRequest>();
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute(request)).Returns(response);
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            restClientWrapper.Execute(clientMock.Object, request);

            clientMock.Verify(mock => mock.Execute(request), Times.AtLeast(2));
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.Execute_IfMaxAttemptsCountIsSet_IfResponseFailed_TriesToExecuteMaxCountTimes))]
        public void Execute_IfMaxAttemptsCountIsSet_IfResponseFailed_TriesToExecuteMaxCountTimes(RestResponse response, int maxCount, int expectedCount)
        {
            var request = It.IsAny<IRestRequest>();
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute(request)).Returns(response);
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(maxCount);

            restClientWrapper.Execute(clientMock.Object, request);

            clientMock.Verify(mock => mock.Execute(request), Times.Exactly(expectedCount));
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GenericExecute_IfResponseSuccess_TriesToExecuteOnce))]
        public void GenericExecute_IfResponseSuccess_TriesToExecuteOnce<T>(RestResponse<T> response)
            where T : class, new()
        {
            var request = It.IsAny<IRestRequest>();
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute<T>(request)).Returns(response);
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(100);

            restClientWrapper.Execute<T>(clientMock.Object, request);

            clientMock.Verify(mock => mock.Execute<T>(request), Times.Once);
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GenericExecute_IfMaxAttemptsCountIsNotSet_IfResponseFailed_TriesToExecuteAtLeastTwice))]
        public void GenericExecute_IfMaxAttemptsCountIsNotSet_IfResponseFailed_TriesToExecuteAtLeastTwice<T>(RestResponse<T> response)
            where T : class, new()
        {
            var request = It.IsAny<IRestRequest>();
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute<T>(request)).Returns(response);
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            restClientWrapper.Execute<T>(clientMock.Object, request);

            clientMock.Verify(mock => mock.Execute<T>(request), Times.AtLeast(2));
        }

        [TestCaseSource(typeof(RestClientWrapperTestsSource), nameof(RestClientWrapperTestsSource.GenericExecute_IfMaxAttemptsCountIsSet_IfResponseFailed_TriesToExecuteMaxCountTimes))]
        public void GenericExecute_IfMaxAttemptsCountIsSet_IfResponseFailed_TriesToExecuteMaxCountTimes<T>(
            RestResponse<T> response,
            int maxCount,
            int expectedCount)
            where T : class, new()
        {
            var request = It.IsAny<IRestRequest>();
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute<T>(request)).Returns(response);
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(maxCount);

            restClientWrapper.Execute<T>(clientMock.Object, request);

            clientMock.Verify(mock => mock.Execute<T>(request), Times.Exactly(expectedCount));
        }

        [Test]
        public void GenericExecute_IfMaxAttemptsCountIsNotSet_IfExceptionInsteadOfResponse_TriesToExecuteAtLeastTwiceAndThrowsException()
        {
            var request = It.IsAny<IRestRequest>();
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute<object>(request)).Callback(() => throw new Exception());
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper();

            Assert.Catch(() => restClientWrapper.Execute<object>(clientMock.Object, request));

            clientMock.Verify(mock => mock.Execute<object>(request), Times.AtLeast(2));
        }

        [TestCase(1, 1)]
        [TestCase(0, 1)]
        [TestCase(10, 10)]
        [TestCase(-1, 1)]
        public void GenericExecute_IfMaxAttemptsCountIsSet_IfExceptionInsteadOfResponse_TriesToExecuteMaxCountTimesAndThrowsException(
            int maxCount,
            int expectedCount)
        {
            var request = It.IsAny<IRestRequest>();
            var clientMock = new Mock<RestClient>();
            clientMock.Setup(x => x.Execute<object>(request)).Callback(() => throw new Exception());
            var restClientWrapper = new SDK.Utilities.RestClientWrapper.RestClientWrapper(maxCount);

            Assert.Catch(() => restClientWrapper.Execute<object>(clientMock.Object, request));

            clientMock.Verify(mock => mock.Execute<object>(request), Times.Exactly(expectedCount));
        }
    }

    public static class RestClientWrapperTestsSource
    {
        public static IEnumerable<TestCaseData> GetRestClient_IfAuthenticatorCannotBeCreated_ReturnsCorrectlyWithAuthenticatorAsNull = new[]
        {
            new TestCaseData(
                null
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = null,
                    AuthenticationMethod = AuthenticationMethod.JWT
                }
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "",
                    AuthenticationMethod = AuthenticationMethod.JWT
                }
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "   \n",
                    AuthenticationMethod = AuthenticationMethod.JWT
                }
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = null,
                    AuthenticationMethod = AuthenticationMethod.PredefinedJWT
                }
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "",
                    AuthenticationMethod = AuthenticationMethod.PredefinedJWT
                }
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "   \n",
                    AuthenticationMethod = AuthenticationMethod.PredefinedJWT
                }
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "token",
                    AuthenticationMethod = (AuthenticationMethod)190000000
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetRestClient_IfAuthenticatorCanBeCreated_ReturnsCorrectlyWithAuthenticator = new[]
        {
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "token",
                    AuthenticationMethod = AuthenticationMethod.JWT
                },
                new JwtAuthenticator("token"),
                "Bearer token"
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "token",
                    AuthenticationMethod = AuthenticationMethod.PredefinedJWT
                },
                new JwtAuthenticator("token"),
                "Bearer token"
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = "username",
                    Password = "password",
                    AccessToken = "token",
                    AuthenticationMethod = AuthenticationMethod.Basic
                },
                new HttpBasicAuthenticator("username", "password"),
                "Basic dXNlcm5hbWU6cGFzc3dvcmQ="
            ),
            new TestCaseData(
                new RestClientCredentials
                {
                    Username = null,
                    Password = null,
                    AccessToken = "token",
                    AuthenticationMethod = AuthenticationMethod.Basic
                },
                new HttpBasicAuthenticator(null, null),
                "Basic Og=="
            ),
        };

        public static IEnumerable<TestCaseData> GetRestRequest_IfRequestMethodExists_ReturnsRequestWithCorrectMethod = new[]
        {
            new TestCaseData(
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Get,
                },
                Method.GET
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Post,
                },
                Method.POST
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Put,
                },
                Method.PUT
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Delete,
                },
                Method.DELETE
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestMethod = RequestMethod.Patch,
                },
                Method.PATCH
            ),
            new TestCaseData(
                new RestClientParameters(),
                Method.GET
            ),
        };

        public static IEnumerable<TestCaseData> GetRestRequest_IfDataFormatExists_ReturnsRequestWithCorrectRequestFormat = new[]
        {
            new TestCaseData(
                new RestClientParameters
                {
                    RequestDataFormat = DataFormat.Json
                },
                RestSharp.DataFormat.Json
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestDataFormat = DataFormat.Xml
                },
                RestSharp.DataFormat.Xml
            ),
            new TestCaseData(
                new RestClientParameters(),
                RestSharp.DataFormat.Json
            ),
        };

        public static IEnumerable<TestCaseData> GetRestRequest_ReturnsRequestWithCorrectHeaders = new[]
        {
            new TestCaseData(
                new RestClientParameters
                {
                    RequestHeaders = new Dictionary<string, string>
                    {
                        { "x-SDK", "EncoreTickets.SDK.NET" },
                        { "x-apply-price-engine", "true" },
                        { "x-market", "broadway" },
                    },
                }
            ),
            new TestCaseData(
                new RestClientParameters()
            ),
        };

        public static IEnumerable<TestCaseData> GetRestRequest_ReturnsRequestWithCorrectUrlSegments = new[]
        {
            new TestCaseData(
                new RestClientParameters
                {
                    RequestUrlSegments = new Dictionary<string, string>
                    {
                        { "version", "v1" },
                        { "id", "1000" },
                        { "productId", "2000" },
                    },
                }
            ),
            new TestCaseData(
                new RestClientParameters()
            ),
        };

        public static IEnumerable<TestCaseData> GetRestRequest_ReturnsRequestWithCorrectQueryParams = new[]
        {
            new TestCaseData(
                new RestClientParameters
                {
                    RequestQueryParameters = new Dictionary<string, string>
                    {
                        { "date", "12/12/2000" },
                        { "time", "10:00" },
                        { "page", "1" },
                    },
                }
            ),
            new TestCaseData(
                new RestClientParameters()
            ),
        };

        public static IEnumerable<TestCaseData> GetRestRequest_ReturnsRequestWithCorrectBody = new[]
        {
            new TestCaseData(
                new RestClientParameters
                {
                    RequestBody = new object(),
                    RequestDataFormat = DataFormat.Json
                },
                "application/json"
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestBody = 4,
                    RequestDataFormat = DataFormat.Xml
                },
                "application/xml"
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestBody = null,
                    RequestDataFormat = DataFormat.Xml
                },
                null
            ),
            new TestCaseData(
                new RestClientParameters(),
                null
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestBody = 100
                },
                "application/json"
            ),
        };

        public static IEnumerable<TestCaseData> GetRestRequest_IfJsonSerializerIsSet_ReturnsRequestWithCustomSerializer = new[]
        {
            new TestCaseData(
                new RestClientParameters
                {
                    RequestDataFormat = DataFormat.Json,
                    RequestDataSerializer = new DefaultJsonSerializer()
                }
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestDataSerializer = new DefaultJsonSerializer()
                }
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestDataFormat = DataFormat.Json,
                    RequestDataSerializer = new DefaultJsonSerializer(new[] {new SingleOrListToListConverter<string>()})
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetRestRequest_IfJsonSerializerIsNotSet_ReturnsRequestWithDefaultSerializer = new[]
        {
            new TestCaseData(
                new RestClientParameters
                {
                    RequestDataFormat = DataFormat.Json,
                }
            ),
            new TestCaseData(
                new RestClientParameters
                {
                    RequestDataFormat = DataFormat.Xml,
                    RequestDataSerializer = new DefaultJsonSerializer(new[] {new SingleOrListToListConverter<string>()})
                }
            ),
            new TestCaseData(
                new RestClientParameters()
            ),
        };

        public static IEnumerable<TestCaseData> Execute_IfMaxExecutionsCountIsNotSet_IfResponseSuccess_TriesToExecuteOnce = new[]
        {
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.OK
                }
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.Moved
                }
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.NoContent
                }
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.PaymentRequired
                }
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.Redirect
                }
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.RedirectMethod
                }
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.TemporaryRedirect
                }
            ),
        };

        public static IEnumerable<TestCaseData> Execute_IfMaxAttemptsCountIsNotSet_IfResponseFailed_TriesToExecuteAtLeastTwice = new[]
        {
            new TestCaseData(
                new RestResponse { }
            ),
            new TestCaseData(
                new RestResponse {ErrorException = new Exception()}
            ),
            new TestCaseData(
                new RestResponse {ErrorMessage = "error"}
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.NotFound
                }
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError
                }
            ),
        };

        public static IEnumerable<TestCaseData> Execute_IfMaxAttemptsCountIsSet_IfResponseFailed_TriesToExecuteMaxCountTimes = new[]
        {
            new TestCaseData(
                new RestResponse { },
                4,
                4
            ),
            new TestCaseData(
                new RestResponse {ErrorException = new Exception()},
                1,
                1
            ),
            new TestCaseData(
                new RestResponse {ErrorMessage = "error"},
                0,
                1
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.NotFound
                },
                100,
                100
            ),
            new TestCaseData(
                new RestResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError
                },
                5,
                5
            ),
        };

        public static IEnumerable<TestCaseData> GenericExecute_IfResponseSuccess_TriesToExecuteOnce = new[]
        {
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.OK
                }
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.Moved
                }
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.NoContent
                }
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.PaymentRequired
                }
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.Redirect
                }
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.RedirectMethod
                }
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.TemporaryRedirect
                }
            ),
        };

        public static IEnumerable<TestCaseData> GenericExecute_IfMaxAttemptsCountIsNotSet_IfResponseFailed_TriesToExecuteAtLeastTwice = new[]
        {
            new TestCaseData(
                new RestResponse<object> { }
            ),
            new TestCaseData(
                new RestResponse<object> {ErrorException = new Exception()}
            ),
            new TestCaseData(
                new RestResponse<object> {ErrorMessage = "error"}
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.NotFound
                }
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.InternalServerError
                }
            ),
        };

        public static IEnumerable<TestCaseData> GenericExecute_IfMaxAttemptsCountIsSet_IfResponseFailed_TriesToExecuteMaxCountTimes = new[]
        {
            new TestCaseData(
                new RestResponse<object> { },
                4,
                4
            ),
            new TestCaseData(
                new RestResponse<object> {ErrorException = new Exception()},
                1,
                1
            ),
            new TestCaseData(
                new RestResponse<object> {ErrorMessage = "error"},
                0,
                1
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.NotFound
                },
                100,
                100
            ),
            new TestCaseData(
                new RestResponse<object>
                {
                    StatusCode = HttpStatusCode.InternalServerError
                },
                5,
                5
            ),
        };
    }
}
