using System;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Authentication.JWTServices;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Authentication
{
    /// <summary>
    /// The factory for authentication services.
    /// </summary>
    internal static class AuthenticationServiceFactory
    {
        /// <summary>
        /// Returns an authentication service based on selected authentication method from a context.
        /// </summary>
        /// <param name="context">The API context</param>
        /// <param name="host">The service host.</param>
        /// <param name="loginEndpoint">The endpoint of the login method for the service.</param>
        /// <returns>Authentication service.</returns>
        public static IAuthenticationService Create(ApiContext context, string host, string loginEndpoint)
        {
            switch (context?.AuthenticationMethod)
            {
                case AuthenticationMethod.JWT:
                    return new JwtAuthenticationService(context, host, loginEndpoint);
                case AuthenticationMethod.PredefinedJWT:
                    return new PredefinedJwtAuthenticationService(context, host, loginEndpoint);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
