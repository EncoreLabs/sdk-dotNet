using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Utilities.RestClientWrapper;

namespace EncoreTickets.SDK.Api.Utilities.RestClientBuilder
{
    /// <summary>
    /// The interface for creating entities for the rest client wrapper of API services.
    /// </summary>
    public interface IApiRestClientBuilder
    {
        /// <summary>
        /// Creates <see cref="RestClientWrapper"></see> for requests to API./>.
        /// </summary>
        /// <param name="context">API context.</param>
        /// <returns>Initialized client wrapper.</returns>
        RestClientWrapper CreateClientWrapper(ApiContext context);

        /// <summary>
        /// Creates <see cref="RestClientParameters"></see> for requests to API./>.
        /// </summary>
        /// <param name="context">API context.</param>
        /// <param name="baseUrl">Site URL.</param>
        /// <param name="requestParameters">Parameters to create a request.</param>
        /// <returns>Initialized client wrapper parameters.</returns>
        RestClientParameters CreateClientWrapperParameters(
            ApiContext context,
            string baseUrl,
            ExecuteApiRequestParameters requestParameters);

        /// <summary>
        /// Saves response information in the context.
        /// </summary>
        /// <param name="responseInformation">Response information.</param>
        /// <param name="context">Api context.</param>
        void SaveResponseInfoInApiContext(RestResponseInformation responseInformation, ApiContext context);
    }
}