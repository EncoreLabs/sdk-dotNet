using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Api.Models
{
    /// <summary>
    /// The context for requests to API.
    /// </summary>
    public class ApiContext
    {
        /// <summary>
        /// Gets the Nova user name of the user that your API calls will run as.
        /// </summary>
        public string UserName { get; protected set; }

        /// <summary>
        /// Gets the user's password.
        /// </summary>
        public string Password { get; protected set; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        public Environments Environment { get; protected set; }

        /// <summary>
        /// Gets or sets the type of authentication that should be used to login.
        /// </summary>
        public AuthenticationMethod AuthenticationMethod { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

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
        public ApiContext(Environments environment, string token) : this(environment)
        {
            AccessToken = token;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        public ApiContext() : this(Environments.Production)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        public ApiContext(Environments env)
        {
            AuthenticationMethod = AuthenticationMethod.JWT;
            Environment = env;
        }
    }
}
