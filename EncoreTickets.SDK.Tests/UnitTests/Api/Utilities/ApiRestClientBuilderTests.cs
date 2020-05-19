using System;
using System.Collections.Generic;
using System.Threading;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Api.Utilities.RestClientBuilder;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Utilities.RestClientWrapper;
using EncoreTickets.SDK.Utilities.Serializers;
using EncoreTickets.SDK.Utilities.Serializers.Converters;
using Moq;
using NUnit.Framework;
using IDeserializer = RestSharp.Deserializers.IDeserializer;
using ISerializer = RestSharp.Serializers.ISerializer;

namespace EncoreTickets.SDK.Tests.UnitTests.Api.Utilities
{
    [TestFixture]
    internal class ApiRestClientBuilderTests
    {
        [SetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = TestHelper.Culture;
        }

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

        [Test]
        public void CreateClientWrapperParameters_IfSerializerIsNotSet_ReturnsParametersWithInitializedRequestDataFormatAsJson()
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), It.IsAny<string>(), new ExecuteApiRequestParameters());

            Assert.AreEqual(DataFormat.Json, actual.RequestDataFormat);
        }

        [TestCaseSource(typeof(ApiRestClientBuilderTestsSource), nameof(ApiRestClientBuilderTestsSource.CreateClientWrapperParameters_ReturnsParametersWithInitializedSerializer))]
        public void CreateClientWrapperParameters_IfSerializerIsSet_ReturnsParametersWithInitializedSerializerAndRequestDataFormat(
            ExecuteApiRequestParameters parameters,
            ISerializer expectedSerializer,
            DataFormat expectedDataFormat)
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), It.IsAny<string>(), parameters);

            AssertExtension.AreObjectsValuesEqual(expectedSerializer, actual.RequestDataSerializer);
            Assert.AreEqual(expectedDataFormat, actual.RequestDataFormat);
        }

        [Test]
        public void CreateClientWrapperParameters_IfDeserializerIsNotSet_ReturnsParametersWithInitializedResponseDataFormatAsJson()
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), It.IsAny<string>(), new ExecuteApiRequestParameters());

            Assert.AreEqual(DataFormat.Json, actual.ResponseDataFormat);
        }

        [TestCaseSource(typeof(ApiRestClientBuilderTestsSource), nameof(ApiRestClientBuilderTestsSource.CreateClientWrapperParameters_ReturnsParametersWithInitializedDeserializer))]
        public void CreateClientWrapperParameters_ReturnsParametersWithInitializedDeserializer(
            ExecuteApiRequestParameters parameters,
            IDeserializer expectedDeserializer,
            DataFormat expectedDataFormat)
        {
            var builder = new ApiRestClientBuilder();

            var actual = builder.CreateClientWrapperParameters(It.IsAny<ApiContext>(), It.IsAny<string>(), parameters);

            AssertExtension.AreObjectsValuesEqual(expectedDeserializer, actual.ResponseDataDeserializer);
            Assert.AreEqual(expectedDataFormat, actual.ResponseDataFormat);
        }

        [TestCaseSource(typeof(ApiRestClientBuilderTestsSource), nameof(ApiRestClientBuilderTestsSource.SaveResponseInfoInApiContext_SetsInfoInApiContext))]
        public void SaveResponseInfoInApiContext_SetsInfoInApiContext(
            ApiContext sourceContext,
            RestResponseInformation information,
            ApiContext expectedContext)
        {
            var builder = new ApiRestClientBuilder();

            builder.SaveResponseInfoInApiContext(information, sourceContext);

            AssertExtension.AreObjectsValuesEqual(expectedContext, sourceContext);
        }
    }

    public static class ApiRestClientBuilderTestsSource
    {
        private const string SdkVersion = "4.0.1";

        public static IEnumerable<TestCaseData> CreateClientWrapper_ReturnsExpectedValue = new[]
        {
            new TestCaseData(
                null,
                new RestClientWrapper
                {
                    Credentials = null
                }
            ),
            new TestCaseData(
                new ApiContext(),
                new RestClientWrapper
                {
                    Credentials = new RestClientCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.PredefinedJWT
                    }
                }
            ),
            new TestCaseData(
                new ApiContext(Environments.QA),
                new RestClientWrapper
                {
                    Credentials = new RestClientCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.PredefinedJWT
                    }
                }
            ),
            new TestCaseData(
                new ApiContext(Environments.QA, "username", "password"),
                new RestClientWrapper
                {
                    Credentials = new RestClientCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.JWT,
                        Password = "password",
                        Username = "username"
                    }
                }
            ),
            new TestCaseData(
                new ApiContext(Environments.QA, "dyfuYTI5GLJjkl"),
                new RestClientWrapper
                {
                    Credentials = new RestClientCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.PredefinedJWT,
                        AccessToken = "dyfuYTI5GLJjkl"
                    }
                }
            ),
            new TestCaseData(
                new ApiContext(Environments.QA, "dyfuYTI5GLJjkl", AuthenticationMethod.JWT),
                new RestClientWrapper
                {
                    Credentials = new RestClientCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.JWT,
                        AccessToken = "dyfuYTI5GLJjkl"
                    }
                }
            ),
            new TestCaseData(
                new ApiContext(Environments.QA, "username", "password", AuthenticationMethod.Basic)
                {
                    AccessToken = "dyfuYTI5GLJjkl"
                },
                new RestClientWrapper
                {
                    Credentials = new RestClientCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.Basic,
                        Password = "password",
                        Username = "username",
                        AccessToken = "dyfuYTI5GLJjkl"
                    }
                }
            ),
            new TestCaseData(
                new ApiContext(Environments.QA, "username", "password")
                {
                    AccessToken = "dyfuYTI5GLJjkl",
                    AuthenticationMethod = AuthenticationMethod.Basic
                },
                new RestClientWrapper
                {
                    Credentials = new RestClientCredentials
                    {
                        AuthenticationMethod = AuthenticationMethod.Basic,
                        Password = "password",
                        Username = "username",
                        AccessToken = "dyfuYTI5GLJjkl"
                    }
                }
            ),
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

        public static IEnumerable<TestCaseData> CreateClientWrapperParameters_ReturnsParametersWithInitializedHeaders =
            new[]
            {
                new TestCaseData(
                    null,
                    new Dictionary<string, string>
                    {
                        {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"}
                    }
                ),
                new TestCaseData(
                    new ApiContext(),
                    new Dictionary<string, string>
                    {
                        {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"}
                    }
                ),
                new TestCaseData(
                    new ApiContext
                    {
                        Affiliate = " ",
                        Correlation = "",
                        Market = null
                    },
                    new Dictionary<string, string>
                    {
                        {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"}
                    }
                ),
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
                ),
                new TestCaseData(
                    new ApiContext
                    {
                        Correlation = "30435ee1-c0ce-4664-85b9-cf5402f20e83"
                    },
                    new Dictionary<string, string>
                    {
                        {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"},
                        {"X-Correlation-ID", "30435ee1-c0ce-4664-85b9-cf5402f20e83"},
                    }
                ),
                new TestCaseData(
                    new ApiContext
                    {
                        Market = Market.Uk
                    },
                    new Dictionary<string, string>
                    {
                        {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"},
                        {"x-market", "Uk"},
                    }
                ),
                new TestCaseData(
                    new ApiContext
                    {
                        Market = Market.Broadway
                    },
                    new Dictionary<string, string>
                    {
                        {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"},
                        {"x-market", "Broadway"},
                    }
                ),
            };

        public static IEnumerable<TestCaseData> CreateClientWrapperParameters_ReturnsParametersWithInitializedQueryParameters = new[]
        {
            new TestCaseData(
                new ExecuteApiRequestParameters(),
                null
            ),
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
            ),
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
            ),
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new
                    {
                        Id = (string)null
                    }
                },
                null
            ),
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Query = new { }
                },
                null
            ),
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
            ),
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
            ),
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
                    { "date", "12/31/2019 23:59:59" }
                }
            ),
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
                    { "date", "12/31/2019 23:59:59" },
                }
            ),
        };

        public static IEnumerable<TestCaseData> CreateClientWrapperParameters_ReturnsParametersWithInitializedSerializer = new[]
            {
                new TestCaseData(
                    new ExecuteApiRequestParameters(),
                    new DefaultJsonSerializer(),
                    DataFormat.Json
                ),
                new TestCaseData(
                    new ExecuteApiRequestParameters
                    {
                        DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                    },
                    new DefaultJsonSerializer
                    {
                        DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                    },
                    DataFormat.Json
                ),
                new TestCaseData(
                    new ExecuteApiRequestParameters
                    {
                        Serializer = new DefaultJsonSerializer(new[] {new SingleOrListToListConverter<string>()}),
                        DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                    },
                    new DefaultJsonSerializer(new[] {new SingleOrListToListConverter<string>()})
                    {
                        DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                    },
                    DataFormat.Json
                ),
            };

        public static IEnumerable<TestCaseData> CreateClientWrapperParameters_ReturnsParametersWithInitializedDeserializer = new[]
        {
            new TestCaseData(
                new ExecuteApiRequestParameters(),
                new DefaultJsonSerializer(),
                DataFormat.Json
            ),
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                },
                new DefaultJsonSerializer
                {
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                },
                DataFormat.Json
            ),
            new TestCaseData(
                new ExecuteApiRequestParameters
                {
                    Deserializer = new DefaultJsonSerializer(new []{new SingleOrListToSingleConverter<string>()}),
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                },
                new DefaultJsonSerializer(new []{new SingleOrListToSingleConverter<string>()})
                {
                    DateFormat = "yyyy-MM-ddTHH:mm:sszzz"
                },
                DataFormat.Json
            ),
        };

        public static IEnumerable<TestCaseData> SaveResponseInfoInApiContext_SetsInfoInApiContext = new[]
        {
            new TestCaseData(
                new ApiContext(),
                null,
                new ApiContext()
            ),
            new TestCaseData(
                new ApiContext
                {
                    ReceivedCorrelation = "30435ee1-c0ce-4664-85b9-cf5402f20e83"
                },
                null,
                new ApiContext
                {
                    ReceivedCorrelation = null
                }
            ),
            new TestCaseData(
                new ApiContext
                {
                    ReceivedCorrelation = "30435ee1-c0ce-4664-85b9-cf5402f20e83"
                },
                new RestResponseInformation(),
                new ApiContext
                {
                    ReceivedCorrelation = null
                }
            ),
            new TestCaseData(
                new ApiContext
                {
                    ReceivedCorrelation = "30435ee1-c0ce-4664-85b9-cf5402f20e83"
                },
                new RestResponseInformation
                {
                    ResponseHeaders = new Dictionary<string, object>()
                },
                new ApiContext
                {
                    ReceivedCorrelation = null
                }
            ),
            new TestCaseData(
                new ApiContext
                {
                    ReceivedCorrelation = "30435ee1-c0ce-4664-85b9-cf5402f20e83"
                },
                new RestResponseInformation
                {
                    ResponseHeaders = new Dictionary<string, object>
                    {
                        { "affiliate", "boxoffice" }
                    }
                },
                new ApiContext
                {
                    ReceivedCorrelation = null
                }
            ),
            new TestCaseData(
                new ApiContext
                {
                    ReceivedCorrelation = "30435ee1-c0ce-4664-85b9-cf5402f20e83"
                },
                new RestResponseInformation
                {
                    ResponseHeaders = new Dictionary<string, object>
                    {
                        { "X-Correlation-Id", "99999ee1-c0ce-4664-85b9-cf5402f20e83" }
                    }
                },
                new ApiContext
                {
                    ReceivedCorrelation = "99999ee1-c0ce-4664-85b9-cf5402f20e83"
                }
            ),
        };
    }
}
