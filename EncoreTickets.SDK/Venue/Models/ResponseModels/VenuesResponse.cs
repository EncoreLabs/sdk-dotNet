using System.Collections.Generic;
using EncoreTickets.SDK.Api.Results.Response;

namespace EncoreTickets.SDK.Venue.Models.ResponseModels
{
    internal class VenuesResponse : BaseWrappedApiResponse<VenuesResponseContent, List<Venue>>
    {
        public override List<Venue> Data => (response as VenuesResponseContent).results;
    }

    internal class VenuesResponseContent
    {
        public List<Venue> results { get; set; }
    }
}