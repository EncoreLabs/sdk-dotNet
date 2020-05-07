using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Api
{
    /// <summary>
    /// The base interface of any API service.
    /// </summary>
    public interface IServiceApi
    {
        /// <summary>
        /// Gets the current version of API used by service.
        /// </summary>
        int? ApiVersion { get; }

        /// <summary>
        /// Gets or sets API context.
        /// </summary>
        ApiContext Context { get; set; }
    }
}