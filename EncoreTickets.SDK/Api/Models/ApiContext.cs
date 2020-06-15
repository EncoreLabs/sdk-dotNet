using EncoreTickets.SDK.Authentication.Models;
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
        public string UserName { get; set; }

        /// <summary>
        /// Gets the user's password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        public Environments Environment { get; set; }

        /// <summary>
        /// Gets or sets the type of authentication that should be used to login.
        /// </summary>
        public AuthenticationMethod AuthenticationMethod { get; set; }

        /// <summary>
        /// Gets or sets the affiliate.
        /// Used as a header in requests.
        /// </summary>
        public string Affiliate { get; set; }

        /// <summary>
        /// Gets or sets the correlation ID.
        /// Used as a header in requests.
        /// </summary>
        public string Correlation { get; set; }

        /// <summary>
        /// Gets or sets the received correlation ID.
        /// Received as a header in responses.
        /// </summary>
        public string ReceivedCorrelation { get; internal set; }

        /// <summary>
        /// Gets or sets the market.
        /// Used as a header in requests.
        /// </summary>
        public Market? Market { get; set; }

        /// <summary>
        /// Gets or sets the display currency.
        /// Used as a header in requests.
        /// </summary>
        public string DisplayCurrency { get; set; }

        internal Credentials AgentCredentials { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        public ApiContext(
            Environments environment,
            string userName,
            string password,
            AuthenticationMethod authMethod = AuthenticationMethod.JWT) : this(environment, authMethod)
        {
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        public ApiContext(
            Environments environment,
            string token,
            AuthenticationMethod authMethod = AuthenticationMethod.PredefinedJWT) : this(environment, authMethod)
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
        public ApiContext(
            Environments env,
            AuthenticationMethod authMethod = AuthenticationMethod.PredefinedJWT)
        {
            Environment = env;
            AuthenticationMethod = authMethod;
        }
    }
}
