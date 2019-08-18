using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Authentication;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Authentication
{
    internal class AuthenticationServiceTests
    {
        private static object[] sourceForIsThereAuthenticationTests =
        {
            new object[]
            {
                new ApiContext
                {
                    AuthenticationMethod = AuthenticationMethod.JWT,
                    AccessToken = null
                },
                false,
            },
            new object[]
            {
                new ApiContext
                {
                    AuthenticationMethod = AuthenticationMethod.JWT,
                    AccessToken = ""
                },
                false,
            },
            new object[]
            {
                new ApiContext
                {
                    AuthenticationMethod = AuthenticationMethod.JWT,
                    AccessToken = "test"
                },
                true,
            },
            new object[]
            {
                new ApiContext
                {
                    AuthenticationMethod = AuthenticationMethod.Basic,
                },
                false,
            },
            new object[]
            {
                null,
                false,
            },
        };

        [TestCaseSource(nameof(sourceForIsThereAuthenticationTests))]
        public void Authentication_AuthenticationService_IsThereAuthentication(ApiContext context, bool expectedResult)
        {
            var service = new AuthenticationService(context, It.IsAny<string>(), It.IsAny<string>());
            Assert.AreEqual(expectedResult, service.IsThereAuthentication());
        }
    }
}
