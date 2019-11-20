using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Authentication.Models;

namespace EncoreTickets.SDK.Authentication
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IAuthenticationService" />
    /// <summary>
    /// The service for JWT authentication.
    /// </summary>
    public class JwtAuthenticationService : BaseApi, IAuthenticationService
    {
        private readonly string endpoint;

        /// <summary>
        /// Initializes an instance for the JWT authentication service.
        /// </summary>
        /// <param name="context">The API context.</param>
        /// <param name="host">The service host.</param>
        /// <param name="loginEndpoint">The endpoint for login method.</param>
        public JwtAuthenticationService(ApiContext context, string host, string loginEndpoint)
            : base(context, host)
        {
            endpoint = loginEndpoint;
        }

        /// <inheritdoc />
        public ApiContext Authenticate()
        {
            var accessToken = JwtLogin();
            Context.AccessToken = accessToken.token;
            return Context;
        }

        /// <inheritdoc />
        public bool IsThereAuthentication()
        {
            return !string.IsNullOrEmpty(Context?.AccessToken);
        }

        private AccessToken JwtLogin()
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
            return result.DataOrException;
        }
    }
}