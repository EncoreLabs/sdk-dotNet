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
        public VenueServiceApi(ApiContext context, string baseUrl) : base(context, baseUrl) { }

        /// <summary>
        /// Get the available venues
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IList<Venue> GetVenues()
        {
            ApiResultList<GetVenuesResponse> result = ExecuteApiList<GetVenuesResponse>("v1/venues", HttpMethod.Get, false, null);

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
    }
}