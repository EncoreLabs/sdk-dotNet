using System;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Authentication.Models;

namespace EncoreTickets.SDK.Authentication
{
    /// <inheritdoc />
    /// <summary>
    /// The authentication service.
    /// </summary>
    public class AuthenticationService : BaseApi
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
            switch (Context?.AuthenticationMethod)
            {
                case AuthenticationMethod.JWT:
                    JwtLogin();
                    break;
            }

            return Context;
        }

        /// <summary>
        /// Verifies that the used context has been authenticated.
        /// </summary>
        /// <returns><c>true</c> If the context has been authenticated before ; otherwise, <c>false</c>.</returns>
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
