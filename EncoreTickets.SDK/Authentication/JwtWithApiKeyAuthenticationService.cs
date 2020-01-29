using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Authentication
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IAuthenticationService" />
    /// <summary>
    /// The service for JWT authentication with existing API key.
    /// </summary>
    public class JwtWithApiKeyAuthenticationService : BaseApi, IAuthenticationService
    {
        protected readonly string Endpoint;

        /// <summary>
        /// Initializes an instance for the JWT authentication service with existing API key.
        /// </summary>
        /// <param name="context">The API context.</param>
        /// <param name="host">The service host.</param>
        /// <param name="loginEndpoint">The endpoint for login method.</param>
        public JwtWithApiKeyAuthenticationService(ApiContext context, string host, string loginEndpoint)
            : base(context, host)
        {
            Endpoint = loginEndpoint;
        }

        /// <inheritdoc />
        public virtual ApiContext Authenticate()
        {
            return Context;
        }

        /// <inheritdoc />
        public bool IsThereAuthentication()
        {
            return !string.IsNullOrEmpty(Context?.AccessToken);
        }
    }
}