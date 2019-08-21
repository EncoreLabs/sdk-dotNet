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
                    {"x-SDK", "EncoreTickets.SDK.NET 1.0.0"},
                },
                new Dictionary<string, string> { }
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
                    {"x-SDK", "EncoreTickets.SDK.NET 1.0.0"},
                    {"affiliateId", "test"},
                },
                new Dictionary<string, string> { }
            },
            new object[]
            {
                new ApiContext{UseBroadway = true},
                RequestMethod.Get,
                new Dictionary<string, string>
                {
                    {"x-SDK", "EncoreTickets.SDK.NET 1.0.0"},
                    {"x-apply-price-engine", "true"},
                    {"x-market", "broadway"},
                },
                new Dictionary<string, string>
                {
                    {"countryCode", null }
                }
            },
            new object[]
            {
                new ApiContext{UseBroadway = true},
                RequestMethod.Post,
                new Dictionary<string, string>
                {
                    {"x-SDK", "EncoreTickets.SDK.NET 1.0.0"},
                    {"x-apply-price-engine", "true"},
                    {"x-market", "broadway"},
                },
                new Dictionary<string, string> {}
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
            Dictionary<string, string> expectedHeaders, Dictionary<string, string> expectedQueryParams)
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
            };
            var result = ApiClientWrapperBuilder.CreateClientWrapperParameters(context, baseUrl, endpoint, method, body);
            AssertExtension.SimplePropertyValuesAreEquals(expectedParameters, result);
            foreach (var expected in expectedHeaders)
            {
                Assert.Contains(expected, result.RequestHeaders);
            }
            foreach (var expected in expectedQueryParams)
            {
                Assert.Contains(expected.Key, result.RequestQueryParameters.Keys);
            }
        }
    }
}
