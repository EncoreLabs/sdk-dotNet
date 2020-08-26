using System;
using EncoreTickets.SDK.Api.Models;
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
        /// <inheritdoc />
        public virtual IAuthenticationService AuthenticationService => GetAuthenticationService(Context);

        /// <summary>
        /// Gets or sets a value indicating whether gets the flag enabled automatic authentication.
        /// </summary>
        protected bool AutomaticAuthentication { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseApiWithAuthentication"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="host">The host.</param>
        /// <param name="automaticAuthentication">Optional: the flag enables automatic authentication.</param>
        protected BaseApiWithAuthentication(ApiContext context, string host, bool automaticAuthentication = false)
            : base(context, host)
        {
            AutomaticAuthentication = automaticAuthentication;
        }

        /// <inheritdoc />
        public IAuthenticationService GetAuthenticationService(ApiContext context)
        {
            const string standardLoginEndpoint = "login";
            try
            {
                return AuthenticationServiceFactory.Create(context, Host, standardLoginEndpoint);
            }
            catch (NotImplementedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Triggers authentication if automatic authentication is enabled and an authentication service exists.
        /// </summary>
        protected virtual void TriggerAutomaticAuthentication()
        {
            if (AutomaticAuthentication)
            {
                AuthenticationService?.Authenticate();
            }
        }
    }
}