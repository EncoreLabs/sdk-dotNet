using EncoreTickets.SDK.Authentication;
using Moq;

namespace EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers
{
    internal class ApiServiceMockerWithAuthentication : ApiServiceMocker
    {
        public Mock<IAuthenticationService> AuthenticationServiceMock { get; set; }

        public ApiServiceMockerWithAuthentication()
        {
            AuthenticationServiceMock = GetAuthenticationServiceMock();
        }

        public void VerifyAuthenticateExecution(Times times)
        {
            AuthenticationServiceMock.Verify(x => x.Authenticate(), times);
        }

        private Mock<IAuthenticationService> GetAuthenticationServiceMock()
        {
            var mock = new Mock<IAuthenticationService>();
            mock.Setup(x => x.Authenticate());
            return mock;
        }
    }
}