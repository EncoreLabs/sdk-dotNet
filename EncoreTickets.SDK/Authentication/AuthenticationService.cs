using EncoreTickets.SDK.v1;
using RestSharp;

namespace EncoreTickets.SDK.Authentication
{
    /// <inheritdoc />
    /// <summary>
    /// The authentication service.
    /// </summary>
    public class AuthenticationService : BaseCapabilityServiceApi
    {
        private readonly string endpoint;

        /// <summary>
        /// Initializes an instance for the authentication service.
        /// </summary>
        /// <param name="context">The API context.</param>
        /// <param name="host">The service host.</param>
        /// <param name="loginEndpoint">The endpoint for login method.</param>
        public AuthenticationService(ApiContext context, string host, string loginEndpoint)
            : base(context, host)
        {
            endpoint = loginEndpoint;
        }

        /// <summary>
        /// Get an API context with data set for an authenticated user.
        /// </summary>
        /// <returns>The API context</returns>
        public ApiContext Authenticate()
        {
            switch (context.AuthenticationMethod)
            {
                case AuthenticationMethod.JWT:
                    JwtLogin();
                    break;
            }

            return context;
        }

        /// <summary>
        /// Verifies that the used context has been authenticated.
        /// </summary>
        /// <returns><c>true</c> If the context has been authenticated before ; otherwise, <c>false</c>.</returns>
        public bool IsThereAuthentication()
        {
            switch (context.AuthenticationMethod)
            {
                case AuthenticationMethod.JWT:
                    return !string.IsNullOrEmpty(context.accessToken);
                default:
                    return false;
            }
        }

        private void JwtLogin()
        {
            var credentials = new CredentialsRequest { username = context.userName, password = context.password };
            var result = ExecuteApi<CredentialsResponse>(endpoint, Method.POST, false, credentials);
            context.accessToken = result.Data?.token;
        }
    }
}
