using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Api
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IServiceApiWithAuthentication" />
    /// <summary>
    /// The base API class for a service which allows an authentication.
    /// </summary>
    public abstract class BaseApiWithAuthentication : BaseApi, IServiceApiWithAuthentication
    {
        /// <inheritdoc />
        public virtual IAuthenticationService AuthenticationService => GetAuthenticationService(Context);

        protected BaseApiWithAuthentication(ApiContext context, string host) : base(context, host)
        {
        }

        /// <inheritdoc />
        public virtual IAuthenticationService GetAuthenticationService(ApiContext context)
        {
            return AuthenticationServiceFactory.Create(context, Host, "login");
        }
    }
}