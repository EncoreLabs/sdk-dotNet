namespace EncoreTickets.SDK.Api.Context
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
        Basic
    }
}
