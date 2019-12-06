using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Authentication;

namespace EncoreTickets.SDK.Api
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IServiceApiWithAuthentication" />
    /// <summary>
    /// The base API class for a service which allows an authentication.
    /// </summary>
    public abstract class BaseApiWithAuthentication : BaseApi, IServiceApiWithAuthentication
    {
        private IAuthenticationService authenticationService;

        /// <inheritdoc />
        public virtual IAuthenticationService AuthenticationService => authenticationService ?? (authenticationService = GetAuthenticationService(Context));

        private bool AutomaticAuthentication { get; }

        protected BaseApiWithAuthentication(ApiContext context, string host, bool automaticAuthentication = false) : base(context, host)
        {
            AutomaticAuthentication = automaticAuthentication;
        }

        /// <inheritdoc />
        public virtual IAuthenticationService GetAuthenticationService(ApiContext context)
        {
            const string standardLoginEndpoint = "login";
            return AuthenticationServiceFactory.Create(context, Host, standardLoginEndpoint);
        }

        protected void TriggerAutomaticAuthentication()
        {
            if (AutomaticAuthentication)
            {
                AuthenticationService.Authenticate();
            }
        }
    }
}