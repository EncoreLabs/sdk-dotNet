using System;

namespace EncoreTickets.SDK.Api.Context
{
    public class ApiContext
    {
        /// <summary>
        /// Occurs when an error occurred.
        /// </summary>
        public static event EventHandler<ApiErrorEventArgs> ApiError;

        /// <summary>
        /// Gets or sets the Nova user name of the user that your API calls will run as.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; protected set; }

        /// <summary>
        /// Gets or sets the user's Password.
        /// </summary>
        /// <value>The Password.</value>
        public string Password { get; protected set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The type of authentication that should be used to login.
        /// </summary>
        public AuthenticationMethod AuthenticationMethod { get; set; }

        /// <summary>
        /// The environment
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the timeout milliseconds.
        /// </summary>
        /// <value>The timeout milliseconds.</value>
        public int TimeoutMilliseconds { get; set; }

        /// <summary>
        /// Use broadway 
        /// </summary>
        public bool UseBroadway { get; set; }

        /// <summary>
        /// The affiliate
        /// </summary>
        public string Affiliate { get; set; }

        /// <summary>
        /// Initializes static members of the <see cref="ApiContext"/> class.
        /// </summary>
        public ApiContext(Environments env, string userName, string password) : this(env)
        {
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiContext() : this(Environments.Production)
        { 
            TimeoutMilliseconds = 120000;
        }

        /// <summary>
        /// Initializes static members of the <see cref="ApiContext"/> class.
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
