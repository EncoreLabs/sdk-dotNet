using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Authentication.Models;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Authentication
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IAuthenticationService" />
    /// <summary>
    /// The service for JWT authentication.
    /// </summary>
    public class JwtAuthenticationService : BaseApi, IAuthenticationService
    {
        protected readonly string endpoint;

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
            Context.AccessToken = accessToken.Token;
            return Context;
        }

        /// <inheritdoc />
        public bool IsThereAuthentication()
        {
            return !string.IsNullOrEmpty(Context?.AccessToken);
        }

        private AccessToken JwtLogin()
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = endpoint,
                Method = RequestMethod.Post,
                Body = new Credentials
                {
                    Username = Context.UserName ?? string.Empty,
                    Password = Context.Password ?? string.Empty
                }
            };
            var result = Executor.ExecuteApiWithNotWrappedResponse<AccessToken>(parameters, true);
            return result.DataOrException;
        }
    }
}