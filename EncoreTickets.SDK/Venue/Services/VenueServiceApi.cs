using System.Collections.Generic;
using RestSharp;

namespace EncoreTickets.SDK.Venue
{
    public class VenueServiceApi : BaseCapabilityServiceApi
    {
        /// <summary>
        /// Default constructor for the Venue service
        /// </summary>
        /// <param name="context"></param>
        public VenueServiceApi(ApiContext context) : base(context, "venue-service.{0}tixuk.io/api/") { }

        /// <summary>
        /// Get the available venues
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IList<Venue> GetVenues()
        {
            var result = ExecuteApiList<VenuesResponse>("v1/venues", Method.GET, false, null);
            return result.GetList<Venue>();
        }

        /// <summary>
        /// Get the venue by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Venue GetVenueById(string id)
        {
            var result = ExecuteApi<Venue>($"v1/venues/{id}", Method.GET, true, null);
            return result.Data;
        }

        /// <summary>
        /// Get the seat attributes for a venue
        /// </summary>
        /// <param name="venue"></param>
        /// <returns></returns>
        public IList<SeatAttribute> GetSeatAttributes(Venue venue)
        {
            return GetSeatAttributes(venue.internalId);
        }

        /// <summary>
        /// Get detailed seat attributes
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        public IList<SeatAttribute> GetSeatAttributes(string venueId)
        {
            var result = ExecuteApiList<SeatAttributeResponse>($"v1/venues/{venueId}/seats/attributes/detailed", Method.GET, false, null);
            return result.GetList<SeatAttribute>();
        }

        /// <summary>
        /// Get the standard attributes
        /// </summary>
        /// <returns></returns>
        public IList<StandardAttribute> GetStandardAttributes()
        {
            var result = ExecuteApiList<StandardAttributeResponse>("v1/attributes/standard", Method.GET, false, null);
            return result.GetList<StandardAttribute>();
        }
    }
}