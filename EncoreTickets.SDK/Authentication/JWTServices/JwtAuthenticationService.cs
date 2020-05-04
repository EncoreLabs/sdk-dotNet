using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Authentication.Models;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Authentication.JWTServices
{
    /// <inheritdoc cref="BaseJwtAuthenticationService" />
    /// <summary>
    /// The service for JWT authentication based on credentials.
    /// </summary>
    public class JwtAuthenticationService : BaseJwtAuthenticationService
    {
        /// <summary>
        /// Initializes an instance for the JWT authentication service based on credentials.
        /// </summary>
        /// <param name="context">The API context.</param>
        /// <param name="host">The service host.</param>
        /// <param name="loginEndpoint">The endpoint for login method.</param>
        public JwtAuthenticationService(ApiContext context, string host, string loginEndpoint)
            : base(context, host, loginEndpoint)
        {
        }

        /// <inheritdoc />
        public override ApiContext Authenticate()
        {
            var accessToken = JwtLogin();
            Context.AccessToken = accessToken.Token;
            return Context;
        }

        private AccessToken JwtLogin()
        {
            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = Endpoint,
                Method = RequestMethod.Post,
                Body = new Credentials
                {
                    Username = Context.UserName ?? string.Empty,
                    Password = Context.Password ?? string.Empty
                }
            };
            var result = Executor.ExecuteApiWithNotWrappedResponse<AccessToken>(requestParameters);
            return result.DataOrException;
        }
    }
}