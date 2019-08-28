using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Helpers.RestClientWrapper;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiClientWrapperBuilderTests
    {
        private const string SdkVersion = "1.0.1";

        private static object[] sourceForCreateClientWrapperTests =
        {
            new ApiContext(),
            null,
        };

        private static object[] sourceForCreateClientWrapperParametersTests =
        {
            new object[]
            {
                new ApiContext(),
                RequestMethod.Get,
                new Dictionary<string, string>
                {
                    {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"},
                },
            },
            new object[]
            {
                new ApiContext
                {
                    Affiliate = "test"
                },
                RequestMethod.Get,
                new Dictionary<string, string>
                {
                    {"x-SDK", $"EncoreTickets.SDK.NET {SdkVersion}"},
                    {"affiliateId", "test"},
                },
            },
        };

        [TestCaseSource(nameof(sourceForCreateClientWrapperTests))]
        public void Api_ApiClientWrapperBuilder_CreateClientWrapper_ReturnsClientWrapper(ApiContext context)
        {
            var wrapper = ApiClientWrapperBuilder.CreateClientWrapper(context);
            Assert.IsTrue(wrapper != null);
        }

        [TestCaseSource(nameof(sourceForCreateClientWrapperParametersTests))]
        public void Api_ApiClientWrapperBuilder_CreateClientWrapperParameters_ReturnsCorrectedParameters(ApiContext context, RequestMethod method,
            Dictionary<string, string> expectedHeaders)
        {
            var baseUrl = It.IsAny<string>();
            var endpoint = It.IsAny<string>();
            var body = It.IsAny<object>();
            var expectedParameters = new RestClientParameters
            {
                BaseUrl = baseUrl,
                RequestUrl = endpoint,
                RequestBody = body,
                RequestFormat = RequestFormat.Json,
                RequestMethod = method,
                RequestUrlSegments = null,
                RequestQueryParameters = null,
            };
            var result = ApiClientWrapperBuilder.CreateClientWrapperParameters(context, baseUrl, endpoint, method, body);
            AssertExtension.SimplePropertyValuesAreEquals(expectedParameters, result);
            AssertExtension.EnumerableAreEquals(expectedParameters.RequestUrlSegments, result.RequestUrlSegments);
            AssertExtension.EnumerableAreEquals(expectedParameters.RequestQueryParameters, result.RequestQueryParameters);
            AssertExtension.EnumerableAreEquals(expectedParameters.RequestHeaders, expectedHeaders);
        }
    }
}
