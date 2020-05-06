using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Authentication.JWTServices
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IAuthenticationService" />
    /// <summary>
    /// The base service for JWT authentication.
    /// </summary>
    public abstract class BaseJwtAuthenticationService : BaseApi, IAuthenticationService
    {
        protected readonly string Endpoint;

        /// <inheritdoc/>
        public override int? ApiVersion => null;

        protected BaseJwtAuthenticationService(ApiContext context, string host, string loginEndpoint)
            : base(context, host)
        {
            Endpoint = loginEndpoint;
        }

        /// <inheritdoc />
        public abstract ApiContext Authenticate();

        /// <inheritdoc />
        public bool IsThereAuthentication()
        {
            return !string.IsNullOrEmpty(Context?.AccessToken);
        }
    }
}