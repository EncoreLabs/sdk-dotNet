using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Authentication
{
    /// <summary>
    /// The interface of an authentication service.
    /// </summary>
    public interface IAuthenticationService : IServiceApi
    {
        /// <summary>
        /// Get an API context with data set for an authenticated user.
        /// </summary>
        /// <returns>The API context</returns>
        ApiContext Authenticate();

        /// <summary>
        /// Verifies that the used context has been authenticated.
        /// </summary>
        /// <returns><c>true</c> If the context has been authenticated before ; otherwise, <c>false</c>.</returns>
        bool IsThereAuthentication();
    }
}