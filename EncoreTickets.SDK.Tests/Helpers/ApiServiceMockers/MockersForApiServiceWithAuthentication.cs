using EncoreTickets.SDK.Authentication;
using Moq;

namespace EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers
{
    internal class MockersForApiServiceWithAuthentication : MockersForApiService
    {
        public Mock<IAuthenticationService> AuthenticationServiceMock;

        public MockersForApiServiceWithAuthentication()
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