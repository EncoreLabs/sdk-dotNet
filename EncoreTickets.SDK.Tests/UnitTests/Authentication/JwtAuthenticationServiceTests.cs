using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Authentication.JWTServices;
using EncoreTickets.SDK.Authentication.Models;
using EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers;
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

        public JwtAuthenticationServiceTests() : base(new ApiContext(Environments.Sandbox),
            "some-service.{0}tixuk.io/api/", "login")
        {
        }

        [SetUp]
        public void CreateMockers()
        {
            mockers = new MockersForApiServiceWithAuthentication();
        }

        [TestCaseSource(typeof(JwtAuthenticationServiceTestsSource), nameof(JwtAuthenticationServiceTestsSource.Authenticate_CallsApiWithRightParameters))]
        public void Authenticate_CallsApiWithRightParameters(ApiContext context, string expectedRequestBody)
        {
            Context = context;
            mockers.SetupAnyExecution<AccessToken>();

            try
            {
                var actual = Authenticate();
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<AccessToken>(BaseUrl, Endpoint, Method.POST, bodyInJson: expectedRequestBody);
        }

        [TestCaseSource(typeof(JwtAuthenticationServiceTestsSource), nameof(JwtAuthenticationServiceTestsSource.Authenticate_IfApiResponseSuccessful_ReturnsInitializedContext))]
        public void Authenticate_IfApiResponseSuccessful_ReturnsInitializedContext(string expectedToken)
        {
            var responseContent = $"{{\"token\":\"{expectedToken}\"}}";
            mockers.SetupSuccessfulExecution<AccessToken>(responseContent);

            var actual = Authenticate();

            Assert.AreEqual(expectedToken, actual.AccessToken);
        }

        [TestCaseSource(typeof(JwtAuthenticationServiceTestsSource), nameof(JwtAuthenticationServiceTestsSource.Authenticate_IfApiResponseFailed_ThrowsApiException))]
        public void Authenticate_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<AccessToken>(responseContent, code);
            
            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = Authenticate();
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
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
        public static IEnumerable<TestCaseData> Authenticate_CallsApiWithRightParameters = new[]
        {
            new TestCaseData(
                new ApiContext(It.IsAny<Environments>(), "admin", "valid_password"),
                "{\"username\":\"admin\",\"password\":\"valid_password\"}"
            ),
            new TestCaseData(
                new ApiContext(It.IsAny<Environments>(), "admin", "invalid_password"),
                "{\"username\":\"admin\",\"password\":\"invalid_password\"}"
            ),
        };

        public static IEnumerable<TestCaseData> Authenticate_IfApiResponseSuccessful_ReturnsInitializedContext = new[]
        {
            new TestCaseData(
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1Ni"
            ),
        };

        public static IEnumerable<TestCaseData> Authenticate_IfApiResponseFailed_ThrowsApiException = new[]
        {
            new TestCaseData(
                "{\"request\":{\"body\":\"{\\\"username\\\":\\\"admin\\\",\\\"password\\\":\\\"invalid_password\\\"}\",\"query\":{},\"urlParams\":{}},\"response\":\"\",\"context\":{\"errors\":[{\"message\":\"Bad credentials, please verify that your username/password are correctly set\"}]}}",
                HttpStatusCode.Unauthorized,
                "Bad credentials, please verify that your username/password are correctly set"
            ),
        };

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
