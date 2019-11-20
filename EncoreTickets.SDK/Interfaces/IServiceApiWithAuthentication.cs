using EncoreTickets.SDK.Authentication;

namespace EncoreTickets.SDK.Interfaces
{
    /// <summary>
    /// The interface of a service which allows an authentication.
    /// </summary>
    public interface IServiceApiWithAuthentication
    {
        /// <summary>
        /// The authentication service.
        /// </summary>
        IAuthenticationService AuthenticationService { get; }
    }
}
