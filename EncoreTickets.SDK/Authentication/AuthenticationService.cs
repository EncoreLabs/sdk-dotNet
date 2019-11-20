using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Authentication.Models;

namespace EncoreTickets.SDK.Authentication
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IAuthenticationService" />
    /// <summary>
    /// The authentication service.
    /// </summary>
    public class AuthenticationService : BaseApi, IAuthenticationService
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

        /// <inheritdoc />
        public ApiContext Authenticate()
        {
            switch (Context?.AuthenticationMethod)
            {
                case AuthenticationMethod.JWT:
                    JwtLogin();
                    break;
            }

            return Context;
        }

        /// <inheritdoc />
        public bool IsThereAuthentication()
        {
            switch (Context?.AuthenticationMethod)
            {
                case AuthenticationMethod.JWT:
                    return !string.IsNullOrEmpty(Context.AccessToken);
                default:
                    return false;
            }
        }

        private void JwtLogin()
        {
            var credentials = new Credentials
            {
                username = Context.UserName ?? string.Empty,
                password = Context.Password ?? string.Empty
            };
            var result = Executor.ExecuteApiWithNotWrappedResponse<AccessToken>(
                endpoint,
                RequestMethod.Post,
                credentials,
                wrappedError: true);
            Context.AccessToken = result.DataOrException?.token;
        }
    }
}
