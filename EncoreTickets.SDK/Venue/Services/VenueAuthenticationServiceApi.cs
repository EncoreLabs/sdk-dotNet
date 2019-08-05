using RestSharp;

namespace EncoreTickets.SDK.Venue
{
    /// <inheritdoc />
    /// <summary>
    /// The authentication service for a venue service.
    /// </summary>
    public class VenueAuthenticationServiceApi : BaseCapabilityServiceApi
    {
        /// <summary>
        /// Initializes an instance for the venue authentication service.
        /// </summary>
        /// <param name="context">The API context.</param>
        public VenueAuthenticationServiceApi(ApiContext context) 
            : base(context, "venue-service.{0}tixuk.io/api/") { }

        /// <summary>
        /// Get an API context with data set for an authenticated user.
        /// </summary>
        /// <returns>The API context</returns>
        public ApiContext Authenticate()
        {
            var credentials = new CredentialsRequest{username = context.userName, password = context.password};
            var result = ExecuteApi<CredentialsResponse>("login", Method.POST, false, credentials);
            context.accessToken = result.Data?.token;
            return context;
        }

        /// <summary>
        /// Verifies that the used context has been authenticated.
        /// </summary>
        /// <returns><c>true</c> If the context has been authenticated before ; otherwise, <c>false</c>.</returns>
        public bool IsThereAuthentication()
        {
            return !string.IsNullOrEmpty(context.accessToken);
        }
    }
}
