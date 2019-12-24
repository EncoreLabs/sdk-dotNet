using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Authentication.Models;
using EncoreTickets.SDK.Tests.Helpers;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Authentication
{
    internal class JwtAuthenticationServiceTests : JwtAuthenticationService
    {
        private MockersForApiService mockers;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public JwtAuthenticationServiceTests() : base(new ApiContext(), "some-service.{0}tixuk.io/api/", "login")
        {
        }

        [Test]
        public void Authenticate_IfApiResponseSuccessful_ReturnsInitializedContext()
        {
            mockers = new MockersForApiService();
            Context = new ApiContext(It.IsAny<Environments>(), "admin", "valid_password");
            var expectedToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1Ni";
            var responseContent = $"{{\"token\":\"{expectedToken}\"}}";
            mockers.SetupSuccessfulExecution<AccessToken>(responseContent);

            var actual = Authenticate();

            Assert.AreEqual(expectedToken, actual.AccessToken);
            mockers.VerifyExecution<AccessToken>(BaseUrl, endpoint, Method.POST, true);
        }

        [Test]
        public void Authenticate_IfApiResponseFailedWithBadCredentialsError_ThrowsApiExceptionWith401Code()
        {
            mockers = new MockersForApiService();
            Context = new ApiContext(It.IsAny<Environments>(), "admin", "invalid_password");
            var code = HttpStatusCode.Unauthorized;
            var responseContent = "{\r\n    \"request\": {\r\n        \"body\": \"{\\n\\t\\\"username\\\": \\\"admin\\\",\\n\\t\\\"password\\\": \\\"invalid_password\\\"\\n}\",\r\n        \"query\": {},\r\n        \"urlParams\": {}\r\n    },\r\n    \"response\": \"\",\r\n    \"context\": {\r\n        \"errors\": [\r\n            {\r\n                \"message\": \"Bad credentials, please verify that your username/password are correctly set\"\r\n            }\r\n        ]\r\n    }\r\n}";
            mockers.SetupFailedExecution<AccessToken>(responseContent, code);
            
            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = Authenticate();
            });

            Assert.AreEqual(code, exception.ResponseCode);
            mockers.VerifyExecution<AccessToken>(BaseUrl, endpoint, Method.POST, true);
        }

        [TestCaseSource(typeof(JwtAuthenticationServiceTestsSource), nameof(JwtAuthenticationServiceTestsSource.IsThereAuthentication_ReturnsCorrectly))]
        public void IsThereAuthentication_ReturnsCorrectly(ApiContext context, bool expectedResult)
        {
            var service = new JwtAuthenticationService(context, It.IsAny<string>(), It.IsAny<string>());

            var actual = service.IsThereAuthentication();

            Assert.AreEqual(expectedResult, actual);
        }
    }

    public static class JwtAuthenticationServiceTestsSource
    {
        public static IEnumerable<TestCaseData> IsThereAuthentication_ReturnsCorrectly = new[]
        {
            new TestCaseData(
                null,
                false
            ) {TestName = $"{nameof(IsThereAuthentication_ReturnsCorrectly)}: Api context is null"},
            new TestCaseData(
                new ApiContext
                {
                    AccessToken = null
                },
                false
            ) {TestName = $"{nameof(IsThereAuthentication_ReturnsCorrectly)}: Token is null"},
            new TestCaseData(
                new ApiContext
                {
                    AccessToken = ""
                },
                false
            ) {TestName = $"{nameof(IsThereAuthentication_ReturnsCorrectly)}: Token is empty"},
            new TestCaseData(
                new ApiContext
                {
                    AccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpYXQiO"
                },
                true
            ) {TestName = $"{nameof(IsThereAuthentication_ReturnsCorrectly)}: Token is filled"},
        };
    }
}
