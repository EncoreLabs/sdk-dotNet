using System.Collections.Generic;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Authentication.JWTServices;
using EncoreTickets.SDK.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Authentication
{
    internal class PredefinedJwtAuthenticationServiceTests
    {
        [TestCaseSource(typeof(JwtWithApiKeyAuthenticationServiceTestsSource), nameof(JwtWithApiKeyAuthenticationServiceTestsSource.Authenticate_DoesNotChangeContext))]
        public void Authenticate_DoesNotChangeContext(ApiContext context)
        {
            var sourceContext = TestHelper.CopyObject(context);
            var service = new PredefinedJwtAuthenticationService(context, It.IsAny<string>(), It.IsAny<string>());

            var updatedContext = service.Authenticate();

            Assert.AreEqual(context, updatedContext);
            AssertExtension.AreObjectsValuesEqual(sourceContext, updatedContext);
        }

        [TestCaseSource(typeof(JwtWithApiKeyAuthenticationServiceTestsSource), nameof(JwtWithApiKeyAuthenticationServiceTestsSource.IsThereAuthentication_ReturnsCorrectly))]
        public void IsThereAuthentication_ReturnsCorrectly(ApiContext context, bool expectedResult)
        {
            var service = new PredefinedJwtAuthenticationService(context, It.IsAny<string>(), It.IsAny<string>());

            var actual = service.IsThereAuthentication();

            Assert.AreEqual(expectedResult, actual);
        }
    }

    internal static class JwtWithApiKeyAuthenticationServiceTestsSource
    {
        public static IEnumerable<TestCaseData> Authenticate_DoesNotChangeContext = new[]
        {
            new TestCaseData(
                new ApiContext(Environments.Production, "token")
            ),
            new TestCaseData(
                new ApiContext(Environments.QA, "admin", "valid_password")
            ),
            new TestCaseData(
                new ApiContext(Environments.Staging, "admin", "valid_password")
            ),
        };

        public static IEnumerable<TestCaseData> IsThereAuthentication_ReturnsCorrectly = new[]
        {
            new TestCaseData(
                null,
                false
            ),
            new TestCaseData(
                new ApiContext
                {
                    AccessToken = null
                },
                false
            ),
            new TestCaseData(
                new ApiContext
                {
                    AccessToken = ""
                },
                false
            ),
            new TestCaseData(
                new ApiContext
                {
                    AccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpYXQiO"
                },
                true
            ),
        };
    }
}