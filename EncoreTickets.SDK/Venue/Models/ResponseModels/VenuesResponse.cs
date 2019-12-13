using System.Collections.Generic;
using EncoreTickets.SDK.Api.Results.Response;

namespace EncoreTickets.SDK.Venue.Models.ResponseModels
{
    /// <summary>
    /// The API response for venue list response.
    /// </summary>
    /// <inheritdoc/>
    internal class VenuesResponse : BaseWrappedApiResponse<VenuesResponseContent, List<Venue>>
    {
        /// <inheritdoc/>
        public override List<Venue> Data => Response.Results;
    }

    internal class VenuesResponseContent
    {
        public List<Venue> Results { get; set; }
    }
}