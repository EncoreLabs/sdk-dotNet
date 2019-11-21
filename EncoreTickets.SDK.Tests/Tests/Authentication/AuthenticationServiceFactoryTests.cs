using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Authentication;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Authentication
{
    internal class AuthenticationServiceFactoryTests
    {
        private static readonly object[] SourceForCreate_IfServiceForAuthMethodExists =
        {
            new object[]
            {
                typeof(JwtAuthenticationService),
                AuthenticationMethod.JWT,
            },
        };

        private static readonly object[] SourceForCreate_IfServiceForAuthMethodDoesNotExist =
        {
            new object[]
            {
                AuthenticationMethod.Basic,
            },
            new object[]
            {
                (AuthenticationMethod) 1090,
            },
        };

        [TestCaseSource(nameof(SourceForCreate_IfServiceForAuthMethodExists))]
        public void Authentication_AuthenticationServiceFactory_Create_IfServiceForAuthMethodExists_ReturnsService(
            Type expectedType, AuthenticationMethod authMethod)
        {
            // Arrange
            var context = new ApiContext(It.IsAny<Environments>()) {AuthenticationMethod = authMethod};

            // Act
            var service = AuthenticationServiceFactory.Create(context, It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.IsInstanceOf(expectedType, service);
        }

        [TestCaseSource(nameof(SourceForCreate_IfServiceForAuthMethodDoesNotExist))]
        public void
            Authentication_AuthenticationServiceFactory_Create_IfServiceForAuthMethodDoesNotExist_ThrowsNotImplementedException(
                AuthenticationMethod authMethod)
        {
            // Arrange
            var context = new ApiContext(It.IsAny<Environments>()) {AuthenticationMethod = authMethod};

            // Act + Assert
            Assert.Throws<NotImplementedException>(() =>
            {
                AuthenticationServiceFactory.Create(context, It.IsAny<string>(), It.IsAny<string>());
            });
        }
    }
}
