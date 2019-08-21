using System;

namespace EncoreTickets.SDK.Api.Context
{
    /// <summary>
    /// The context for requests to API.
    /// </summary>
    public class ApiContext
    {
        /// <summary>
        /// The event that occurs when an error occurred.
        /// </summary>
        public static event EventHandler<ApiErrorEventArgs> ApiError;

        /// <summary>
        /// Gets the Nova user name of the user that your API calls will run as.
        /// </summary>
        public string UserName { get; protected set; }

        /// <summary>
        /// Gets the user's password.
        /// </summary>
        public string Password { get; protected set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the type of authentication that should be used to login.
        /// </summary>
        public AuthenticationMethod AuthenticationMethod { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the timeout milliseconds.
        /// </summary>
        public int TimeoutMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the affiliate.
        /// </summary>
        public string Affiliate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        public ApiContext(Environments environment, string userName, string password) : this(environment)
        {
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        public ApiContext() : this(Environments.Production)
        { 
            TimeoutMilliseconds = 120000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        public ApiContext(Environments env)
        {
            AuthenticationMethod = AuthenticationMethod.JWT;
            Environment = (env == Environments.Production) ? "" : "dev";
        }

        /// <summary>
        /// Called when an error occurred.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ApiErrorEventArgs"/> instance containing the event data.</param>
        /// <returns>if handled</returns>
        public static bool OnErrorOccurred(object sender, ApiErrorEventArgs e)
        {
            if (ApiError == null)
            {
                return false;
            }

            ApiError(sender, e);
            return true;
        }
    }
}
