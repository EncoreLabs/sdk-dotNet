using System;
using EncoreTickets.SDK.Api.Context;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiContextTests
    {
        private static readonly object[] SourceForConstructorTest =
        {
            new object[]
            {
                new ApiContext(),
                Environments.Production
            },
            new object[]
            {
                new ApiContext(Environments.Sandbox),
                Environments.Sandbox
            },
            new object[]
            {
                new ApiContext(Environments.Production),
                Environments.Production
            },
            new object[]
            {
                new ApiContext(Environments.QA, "token"),
                Environments.QA
            },
            new object[]
            {
                new ApiContext(Environments.Production, "token"),
                Environments.Production
            },
            new object[]
            {
                new ApiContext(Environments.Sandbox, "username", "password"),
                Environments.Sandbox
            },
            new object[]
            {
                new ApiContext(Environments.Production, "username", "password"),
                Environments.Production
            },
            new object[]
            {
                new ApiContext(Environments.QA, "username", "password"),
                Environments.QA
            },
            new object[]
            {
                new ApiContext(Environments.Staging, "username", "password"),
                Environments.Staging
            },
        };

        private static readonly object[] SourceForOnErrorOccurredTest =
        {
            new object[]
            {
                new EventHandler<ApiErrorEventArgs>((sender, args) => { }),
                true
            },
            new object[]
            {
                null,
                false
            },
        };

        [TestCaseSource(nameof(SourceForConstructorTest))]
        public void Api_ApiContext_Constructor_InitializesEnvironment(ApiContext context, Environments expected)
        {
            Assert.AreEqual(expected, context.Environment);
        }

        [Test]
        public void Api_ApiContext_Constructor_InitializesCredentials()
        {
            const string username = "username";
            const string password = "password";
            var context  = new ApiContext(Environments.Sandbox, username, password);
            Assert.AreEqual(username, context.UserName);
            Assert.AreEqual(password, context.Password);
        }

        [Test]
        public void Api_ApiContext_Constructor_InitializesToken()
        {
            const string token = "acess_token";
            var context = new ApiContext(Environments.Sandbox, token);
            Assert.AreEqual(token, context.AccessToken);
        }

        [TestCaseSource(nameof(SourceForOnErrorOccurredTest))]
        public void Api_ApiContext_OnErrorOccurred_ReturnsCorrectly(EventHandler<ApiErrorEventArgs> apiError, bool expected)
        {
            ApiContext.ApiError += apiError;

            var result = ApiContext.OnErrorOccurred(It.IsAny<object>(), It.IsAny<ApiErrorEventArgs>());
            Assert.AreEqual(expected, result);

            ApiContext.ApiError -= apiError;
        }
    }
}
