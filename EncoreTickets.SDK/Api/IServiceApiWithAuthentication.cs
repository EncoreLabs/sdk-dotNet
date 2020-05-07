using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Authentication;

namespace EncoreTickets.SDK.Api
{
    /// <summary>
    /// The interface of a service which allows an authentication.
    /// </summary>
    public interface IServiceApiWithAuthentication : IServiceApi
    {
        /// <summary>
        /// Gets the authentication service.
        /// </summary>
        IAuthenticationService AuthenticationService { get; }

        /// <summary>
        /// Returns an authentication service for some context.
        /// </summary>
        /// <param name="context">The API context.</param>
        /// <returns>The authentication service.</returns>
        IAuthenticationService GetAuthenticationService(ApiContext context);
    }
}
