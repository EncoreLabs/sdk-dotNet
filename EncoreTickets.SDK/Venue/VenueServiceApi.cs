using System.Collections.Generic;
using System.Net.Http;

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
            ApiResultList<VenuesResponse> result = ExecuteApiList<VenuesResponse>("v1/venues", HttpMethod.Get, false, null);

            return result.GetList<Venue>();
        }

        /// <summary>
        /// Get the venue by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Venue GetVenueById(string id)
        {
            ApiResult<Venue> result = this.ExecuteApi<Venue>(string.Format("v1/venues/{0}", id), HttpMethod.Get, true, null);

            return result.Data;
        }

        /// <summary>
        /// Get the seat attributes for a venue
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IList<SeatAttribute> GetSeatAttributes(Venue v)
        {
            return this.GetSeatAttributes(v.internalId);
        }

        /// <summary>
        /// Get detailed seat attributes
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        public IList<SeatAttribute> GetSeatAttributes(string venueId)
        {
            ApiResultList<SeatAttributeResponse> result = ExecuteApiList<SeatAttributeResponse>(string.Format("v1/venues/{0}/seats/attributes/detailed", venueId), HttpMethod.Get, false, null);

            return result.GetList<SeatAttribute>();
        }

        /// <summary>
        /// Get the standard attributes
        /// </summary>
        /// <returns></returns>
        public IList<StandardAttribute> GetStandardAttributes()
        {
            ApiResultList<StandardAttributeResponse> result = ExecuteApiList<StandardAttributeResponse>("v1/attributes/standard", HttpMethod.Get, false, null);

            return result.GetList<StandardAttribute>();
        }
    }
}