using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Authentication.Models;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Authentication
{
    internal class AuthenticationServiceTests : AuthenticationService
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

        private static Mock<ApiRequestExecutor> executorMock;

        protected override ApiRequestExecutor Executor => executorMock.Object;

        [SetUp]
        public static void SetUp()
        {
            executorMock = new Mock<ApiRequestExecutor>(It.IsAny<ApiContext>(), It.IsAny<string>());
        }

        [TestCaseSource(nameof(sourceForIsThereAuthenticationTests))]
        public void Authentication_AuthenticationService_IsThereAuthentication_ReturnsCorrectly(ApiContext context, bool expectedResult)
        {
            var service = new AuthenticationService(context, It.IsAny<string>(), It.IsAny<string>());
            Assert.AreEqual(expectedResult, service.IsThereAuthentication());
        }

        [Test]
        public void Authentication_AuthenticationService_AuthenticateJwt_IfSuccess_ReturnsCorrectContext()
        {
            Context = new ApiContext(It.IsAny<Environments>(), "username", "pass")
            {
                AuthenticationMethod = AuthenticationMethod.JWT
            };
            var token = new AccessToken { token = "test" };
            executorMock
                .Setup(x => x.ExecuteApiWithNotWrappedResponse<AccessToken>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    It.IsAny<Credentials>(),
                    null,
                    null,
                    true))
                .Returns(() =>
                    new ApiResult<AccessToken>(token, TestHelper.GetSuccessResponse(), It.IsAny<ApiContext>()));

            var resultContext = Authenticate();
            executorMock.Verify(mock => mock.ExecuteApiWithNotWrappedResponse<AccessToken>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    It.Is<object>(cred =>
                        ((Credentials)cred).password == Context.Password &&
                        ((Credentials)cred).username == Context.UserName),
                    null,
                    null,
                    true),
                Times.Once);
            Assert.AreEqual(token.token, resultContext.AccessToken);
        }

        [Test]
        public void Authentication_AuthenticationService_AuthenticateJwt_IfNotSuccess_ThrowsException()
        {
            Context = new ApiContext(It.IsAny<Environments>(), "username", "pass")
            {
                AuthenticationMethod = AuthenticationMethod.JWT
            };
            executorMock
                .Setup(x => x.ExecuteApiWithNotWrappedResponse<AccessToken>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    It.IsAny<Credentials>(),
                    null,
                    null,
                    true))
                .Returns(() =>
                    new ApiResult<AccessToken>(null, TestHelper.GetFailedResponse(), It.IsAny<ApiContext>(),
                        It.IsAny<Context>(), It.IsAny<Request>()));

            Assert.Throws<ApiException>(() => Authenticate());
            executorMock.Verify(mock => mock.ExecuteApiWithNotWrappedResponse<AccessToken>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    It.Is<object>(cred =>
                        ((Credentials)cred).password == Context.Password &&
                        ((Credentials)cred).username == Context.UserName),
                    null,
                    null,
                    true),
                Times.Once);
        }

        public AuthenticationServiceTests() : base(null, "", "")
        {
        }
    }
}
