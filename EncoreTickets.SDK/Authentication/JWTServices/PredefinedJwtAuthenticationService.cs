using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Authentication.JWTServices
{
    /// <inheritdoc cref="BaseJwtAuthenticationService" />
    /// <summary>
    /// The service for JWT authentication with existing access token.
    /// </summary>
    public class PredefinedJwtAuthenticationService : BaseJwtAuthenticationService
    {
        /// <summary>
        /// Initializes an instance for the JWT authentication service with existing access token.
        /// </summary>
        /// <param name="context">The API context.</param>
        /// <param name="host">The service host.</param>
        /// <param name="loginEndpoint">The endpoint for login method.</param>
        public PredefinedJwtAuthenticationService(ApiContext context, string host, string loginEndpoint)
            : base(context, host, loginEndpoint)
        {
        }

        /// <inheritdoc />
        public override ApiContext Authenticate()
        {
            return Context;
        }
    }
}