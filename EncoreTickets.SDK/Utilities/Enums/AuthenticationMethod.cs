namespace EncoreTickets.SDK.Utilities.Enums
{
    /// <summary>
    /// The type of authentication.
    /// </summary>
    public enum AuthenticationMethod
    {
        /// <summary>
        /// Authentication based on access tokens.
        /// </summary>
        JWT,

        /// <summary>
        /// Authentication based on username and password.
        /// </summary>
        Basic,

        /// <summary>
        /// Authentication based on API key.
        /// </summary>
        ApiKey
    }
}
