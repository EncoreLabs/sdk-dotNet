using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Utilities.RestClientWrapper
{
    /// <summary>
    /// Credentials for <see cref="RestClientWrapper"/>.
    /// </summary>
    public class RestClientCredentials
    {
        /// <summary>
        /// Gets or sets an authentication method for the credentials.
        /// </summary>
        public AuthenticationMethod AuthenticationMethod { get; set; }

        /// <summary>
        /// Gets or sets username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets access token.
        /// </summary>
        public string AccessToken { get; set; }
    }
}