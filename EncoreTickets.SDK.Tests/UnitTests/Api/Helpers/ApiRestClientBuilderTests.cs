using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Helpers.ApiRestClientBuilder;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.Common.RestClientWrapper;
using EncoreTickets.SDK.Utilities.Common.Serializers;
using EncoreTickets.SDK.Utilities.Enums;
using Moq;
using NUnit.Framework;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace EncoreTickets.SDK.Tests.UnitTests.Api.Helpers
{
    internal class ApiRestClientBuilderTests
    {
        [TestCaseSource(typeof(ApiRestClientBuilderTestsSource), nameof(ApiRestClientBuilderTestsSource.CreateClientWrapper_ReturnsExpectedValue))]
        public void CreateClientWrapper_ReturnsExpectedValue(ApiContext context, RestClientWrapper expected)
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapper(context);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCase("http://venue-service.tixuk.io/api/")]
        public void CreateClientWrapperParameters_ReturnsParametersWithInitializedBaseUrl(string baseUrl)
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), baseUrl, new ExecuteApiRequestParameters());

            Assert.AreEqual(baseUrl, actual.BaseUrl);
        }

        [TestCaseSource(typeof(ApiRestClientBuilderTestsSource), nameof(ApiRestClientBuilderTestsSource.CreateClientWrapperParameters_ReturnsParametersWithInitializedBaseProperties))]
        public void CreateClientWrapperParameters_ReturnsParametersWithInitializedBaseProperties(ExecuteApiRequestParameters parameters)
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), It.IsAny<string>(), parameters);

            Assert.AreEqual(parameters.Endpoint, actual.RequestUrl);
            Assert.AreEqual(parameters.Method, actual.RequestMethod);
            Assert.AreEqual(parameters.Body, actual.RequestBody);
        }

        [Test]
        public void CreateClientWrapperParameters_ReturnsParametersWithInitializedRequestFormatAsJson()
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), It.IsAny<string>(), new ExecuteApiRequestParameters());

            Assert.AreEqual(RequestFormat.Json, actual.RequestFormat);
        }

        [TestCaseSource(typeof(ApiRestClientBuilderTestsSource), nameof(ApiRestClientBuilderTestsSource.CreateClientWrapperParameters_ReturnsParametersWithInitializedHeaders))]
        public void CreateClientWrapperParameters_ReturnsParametersWithInitializedHeaders(ApiContext context, Dictionary<string, string> expected)
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(context, It.IsAny<string>(), new ExecuteApiRequestParameters());

            AssertExtension.AreObjectsValuesEqual(expected, actual.RequestHeaders);
        }

        [TestCaseSource(typeof(ApiRestClientBuilderTestsSource), nameof(ApiRestClientBuilderTestsSource.CreateClientWrapperParameters_ReturnsParametersWithInitializedQueryParameters))]
        public void CreateClientWrapperParameters_ReturnsParametersWithInitializedQueryParameters(
            ExecuteApiRequestParameters parameters, Dictionary<string, string> expected)
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), It.IsAny<string>(), parameters);

            AssertExtension.AreObjectsValuesEqual(expected, actual.RequestQueryParameters);
        }

        [TestCaseSource(typeof(ApiRestClientBuilderTestsSource), nameof(ApiRestClientBuilderTestsSource.CreateClientWrapperParameters_ReturnsParametersWithInitializedSerializer))]
        public void CreateClientWrapperParameters_ReturnsParametersWithInitializedSerializer(
            ExecuteApiRequestParameters parameters, ISerializer expected)
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), It.IsAny<string>(), parameters);

            AssertExtension.AreObjectsValuesEqual(expected, actual.Serializer);
        }

        [TestCaseSource(typeof(ApiRestClientBuilderTestsSource), nameof(ApiRestClientBuilderTestsSource.CreateClientWrapperParameters_ReturnsParametersWithInitializedDeserializer))]
        public void CreateClientWrapperParameters_ReturnsParametersWithInitializedDeserializer(
            ExecuteApiRequestParameters parameters, IDeserializer expected)
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), It.IsAny<string>(), parameters);

            AssertExtension.AreObjectsValuesEqual(expected, actual.Deserializer);
        }
    }

    public static class ApiRestClientBuilderTestsSource
    {
        private const string SdkVersion = "2.4.0";

        public static IEnumerable<TestCaseData> CreateClientWrapper_ReturnsExpectedValue = new[]
        {
            new TestCaseData(
                null,
                new RestClientWrapper
                {
                    Credentials = null
                }
            ) {TestName = "CreateClientWrapper_IfApiContextIsNull_ReturnsWrapperWithNullCredentials"},
            new TestCaseData(
                new ApiContext(),
                new RestClientWrapper
                {
                    Credentials = new RestClientWrapperCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.JWT
                    }
                }
            ) {TestName = "CreateClientWrapper_IfApiContextCreatedUsingNothing_ReturnsWrapperWithCredentials"},
            new TestCaseData(
                new ApiContext(Environments.QA),
                new RestClientWrapper
                {
                    Credentials = new RestClientWrapperCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.JWT
                    }
                }
            ) {TestName = "CreateClientWrapper_IfApiContextCreatedUsingEnvironment_ReturnsWrapperWithCredentials"},
            new TestCaseData(
                new ApiContext(Environments.QA, "username", "password"),
                new RestClientWrapper
                {
                    Credentials = new RestClientWrapperCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.JWT,
                        Password = "password",
                        Username = "username"
                    }
                }
            ) {TestName = "CreateClientWrapper_IfApiContextCreatedUsingCredentials_ReturnsWrapperWithCredentials"},
            new TestCaseData(
                new ApiContext(Environments.QA, "dyfuYTI5GLJjkl"),
                new RestClientWrapper
                {
                    Credentials = new RestClientWrapperCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.JWT,
                        AccessToken = "dyfuYTI5GLJjkl"
                    }
                }
            ) {TestName = "CreateClientWrapper_IfApiContextCreatedUsingToken_ReturnsWrapperWithCredentials"},
            new TestCaseData(
                new ApiContext(Environments.QA, "username", "password")
                {
                    AccessToken = "dyfuYTI5GLJjkl",
                    AuthenticationMethod = AuthenticationMethod.Basic
                },
                new RestClientWrapper
                {
                    Credentials = new RestClientWrapperCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.Basic,
                        Password = "password",
                        Username = "username",
                        AccessToken = "dyfuYTI5GLJjkl"
                    }
                }
            ) {TestName = "CreateClientWrapper_IfApiContextCreatedUsingCredentialsAndPropertiesSetup_ReturnsWrapperWithCredentials"},
        };

        public static IEnumerable<TestCaseData> CreateClientWrapperParameters_ReturnsParametersWithInitializedBaseProperties = new[]
        {
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Endpoint = "v1/venues",
                    Method = RequestMethod.Post,
                    Body = new List<string> {"test"}
                }),
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Endpoint = "v1/venues",
                    Method = RequestMethod.Get,
                    Body = -934.32567
                }),
        };

        public static IEnumerable<TestCaseData> CreateClientWrapperParameters_ReturnsParametersWithInitializedHeaders = new[]
        {
            new TestCaseData(
                null,
                new Dictionary<string, string>
                {
                    {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"}
                }
            ) {TestName = "CreateClientWrapperParameters_IfApiContextIsNull_ReturnsParametersWithOnlySdkVersionHeader"},
            new TestCaseData(
                new ApiContext(),
                new Dictionary<string, string>
                {
                    {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"}
                }
            ) {TestName = "CreateClientWrapperParameters_IfApiContextWithoutAffiliate_ReturnsParametersWithOnlySdkVersionHeader"},
            new TestCaseData(
                new ApiContext
                {
                    Affiliate = " "
                },
                new Dictionary<string, string>
                {
                    {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"}
                }
            ) {TestName = "CreateClientWrapperParameters_IfApiContextWithAffiliateWithWhitespaces_ReturnsParametersWithOnlySdkVersionHeader"},
            new TestCaseData(
                new ApiContext
                {
                    Affiliate = "boxoffice"
                },
                new Dictionary<string, string>
                {
                    {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"},
                    {"affiliateId", "boxoffice"},
                }
            ) {TestName = "CreateClientWrapperParameters_IfApiContextWithAffiliate_ReturnsParametersWithSdkVersionAndAffiliateHeaders"},
        };

        public static IEnumerable<TestCaseData> CreateClientWrapperParameters_ReturnsParametersWithInitializedQueryParameters = new[]
        {
            new TestCaseData(
                new ExecuteApiRequestParameters(),
                null
            ) {TestName = "CreateClientWrapperParameters_IfQueryInExecuteRequestParametersIsNull_ReturnsParametersWithNullAsQueryParameters"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new
                    {
                        id = 4
                    }
                },
                new Dictionary<string, string>
                {
                    { "id", "4" }
                }
            ) {TestName = "CreateClientWrapperParameters_IfQueryParametersHasLowerCaseIntProperty_ReturnsParametersWithInitializedQueryParameters"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new
                    {
                        UpperId = 4
                    }
                },
                new Dictionary<string, string>
                {
                    { "upperid", "4" }
                }
            ) {TestName = "CreateClientWrapperParameters_IfQueryParametersHasUpperCaseIntProperty_ReturnsParametersWithInitializedQueryParameters"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new
                    {
                        Id = (string)null
                    }
                },
                null
            ) {TestName = "CreateClientWrapperParameters_IfQueryParametersHasNullProperty_ReturnsParametersWithNullQueryParameters"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new { }
                },
                null
            ) {TestName = "CreateClientWrapperParameters_IfQueryParametersDoesNotHaveProperty_ReturnsParametersWithNullQueryParameters"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new
                    {
                        CompleteObject = new
                        {
                            Id = 8
                        }
                    }
                },
                new Dictionary<string, string>
                {
                    { "completeobject", "{ Id = 8 }" }
                }
            ) {TestName = "CreateClientWrapperParameters_IfQueryParametersHasCompleteAnonymousProperty_ReturnsParametersWithInitializedQueryParameters"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new
                    {
                        CompleteObject = new List<string>{ }
                    }
                },
                new Dictionary<string, string>
                {
                    { "completeobject", "System.Collections.Generic.List`1[System.String]" }
                }
            ) {TestName = "CreateClientWrapperParameters_IfQueryParametersHasCompleteNotAnonymousProperty_ReturnsParametersWithInitializedQueryParameters"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new
                    {
                        Date = new DateTime(2019, 12, 31, 23, 59, 59)
                    }
                },
                new Dictionary<string, string>
                {
                    { "date", "12/31/2019 11:59:59 PM" }
                }
            ) {TestName = "CreateClientWrapperParameters_IfQueryParametersHasDateProperty_ReturnsParametersWithInitializedQueryParameters"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new
                    {
                        id = 4,
                        slug = "9_to_5",
                        Date = new DateTime(2019, 12, 31, 23, 59, 59)
                    }
                },
                new Dictionary<string, string>
                {
                    { "id", "4" },
                    { "slug", "9_to_5" },
                    { "date", "12/31/2019 11:59:59 PM" },
                }
            ) {TestName = "CreateClientWrapperParameters_IfQueryParametersHasProperties_ReturnsParametersWithInitializedQueryParameters"},
        };

        public static IEnumerable<TestCaseData> CreateClientWrapperParameters_ReturnsParametersWithInitializedSerializer = new[]
        {
            new TestCaseData(
                new ExecuteApiRequestParameters(),
                new DefaultJsonSerializer()
            ) {TestName = "CreateClientWrapperParameters_IfSerializerIsNull_IfDateFormatIsNull_ReturnsParametersWithDefaultSerializer"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                },
                new DefaultJsonSerializer
                {
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                }
            ) {TestName = "CreateClientWrapperParameters_IfSerializerIsNull_IfDateFormatIsNotNull_IfReturnsParametersWithDefaultSerializer"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Serializer = new SingleOrListJsonSerializer<string>(),
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                },
                new SingleOrListJsonSerializer<string>
                {
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                }
            ) {TestName = "CreateClientWrapperParameters_IfSerializerIsNotNull_IfReturnsParametersWithInitializedSerializer"},
        };

        public static IEnumerable<TestCaseData> CreateClientWrapperParameters_ReturnsParametersWithInitializedDeserializer = new[]
        {
            new TestCaseData(
                new ExecuteApiRequestParameters(),
                new DefaultJsonSerializer()
            ) {TestName = "CreateClientWrapperParameters_IfDeserializerIsNull_IfDateFormatIsNull_ReturnsParametersWithDefaultDeserializer"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                },
                new DefaultJsonSerializer
                {
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                }
            ) {TestName = "CreateClientWrapperParameters_IfDeserializerIsNull_IfDateFormatIsNotNull_IfReturnsParametersWithDefaultDeserializer"},
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Deserializer = new SingleOrListJsonSerializer<string>(),
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                },
                new SingleOrListJsonSerializer<string>
                {
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                }
            ) {TestName = "CreateClientWrapperParameters_IfDeserializerIsNotNull_IfReturnsParametersWithInitializedDeserializer"},
        };
    }
}
