namespace EncoreTickets.SDK.Utilities.Enums
{
    /// <summary>
    /// The type of authentication.
    /// </summary>
    public enum AuthenticationMethod
    {
        /// <summary>
        /// Authentication based on access token obtained based on credentials.
        /// </summary>
        JWT,

        /// <summary>
        /// Authentication based on predefined access token.
        /// </summary>
        PredefinedJWT,

        /// <summary>
        /// Authentication based on username and password.
        /// </summary>
        Basic,
    }
}
