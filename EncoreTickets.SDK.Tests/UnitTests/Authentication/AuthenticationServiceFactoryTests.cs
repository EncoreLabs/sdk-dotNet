using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Authentication.JWTServices;
using EncoreTickets.SDK.Utilities.Enums;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Authentication
{
    internal class AuthenticationServiceFactoryTests
    {
        [TestCaseSource(typeof(AuthenticationServiceFactoryTestsSource), nameof(AuthenticationServiceFactoryTestsSource.Create_IfServiceForAuthMethodExists_ReturnsService))]
        public void Create_IfServiceForAuthMethodExists_ReturnsService(AuthenticationMethod authMethod, Type expectedType)
        {
            var context = new ApiContext(It.IsAny<Environments>()) { AuthenticationMethod = authMethod };

            var service = AuthenticationServiceFactory.Create(context, It.IsAny<string>(), It.IsAny<string>());

            Assert.IsInstanceOf(expectedType, service);
        }

        [TestCase(AuthenticationMethod.Basic)]
        [TestCase((AuthenticationMethod)1090)]
        public void
            Create_IfServiceForAuthMethodDoesNotExist_ThrowsNotImplementedException(AuthenticationMethod authMethod)
        {
            var context = new ApiContext(It.IsAny<Environments>()) { AuthenticationMethod = authMethod };

            Assert.Throws<NotImplementedException>(() =>
            {
                AuthenticationServiceFactory.Create(context, It.IsAny<string>(), It.IsAny<string>());
            });
        }
    }

    internal static class AuthenticationServiceFactoryTestsSource
    {
        public static IEnumerable<TestCaseData> Create_IfServiceForAuthMethodExists_ReturnsService { get; } = new[]
        {
            new TestCaseData(
                AuthenticationMethod.JWT,
                typeof(JwtAuthenticationService)),
            new TestCaseData(
                AuthenticationMethod.PredefinedJWT,
                typeof(PredefinedJwtAuthenticationService)),
        };
    }
}
