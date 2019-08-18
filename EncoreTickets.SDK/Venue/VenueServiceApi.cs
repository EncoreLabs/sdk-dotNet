﻿using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Venue.Models.RequestModels;

namespace EncoreTickets.SDK.Venue
{
    public class VenueServiceApi : BaseCapabilityServiceApi
    {
        private const string VenueHost = "venue-service.{0}tixuk.io/api/";

        public AuthenticationService AuthenticationService { get; }

        /// <summary>
        /// Default constructor for the Venue service
        /// </summary>
        /// <param name="context"></param>
        public VenueServiceApi(ApiContext context) : base(context, VenueHost)
        {
            context.AuthenticationMethod = AuthenticationMethod.JWT;
            AuthenticationService = new AuthenticationService(context, VenueHost, "login");
        }

        /// <summary>
        /// Get the available venues
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IList<Venue> GetVenues()
        {
            var result = ExecuteApiList<VenuesResponse>("v1/venues", RequestMethod.Get, false, null);
            return result.GetList<Venue>();
        }

        /// <summary>
        /// Get the venue by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Venue GetVenueById(string id)
        {
            var result = ExecuteApi<Venue>($"v1/venues/{id}", RequestMethod.Get, true, null);
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
            var result = ExecuteApiList<SeatAttributeResponse>($"v1/venues/{venueId}/seats/attributes/detailed", RequestMethod.Get, false, null);
            return result.GetList<SeatAttribute>();
        }

        /// <summary>
        /// Get the standard attributes
        /// </summary>
        /// <returns></returns>
        public IList<StandardAttribute> GetStandardAttributes()
        {
            var result = ExecuteApiList<StandardAttributeResponse>("v1/attributes/standard", RequestMethod.Get, false, null);
            return result.GetList<StandardAttribute>();
        }

        /// <summary>
        /// Upsert a standard attribute by its title.
        /// </summary>
        /// <returns>The updated standard attribute.</returns>
        public StandardAttribute UpsertStandardAttributeByTitle(StandardAttribute attribute)
        {
            var result = ExecuteApi<StandardAttribute>("v1/admin/attributes", RequestMethod.Patch, true, attribute);
            return result.Data;
        }

        /// <summary>
        /// Upsert venue's seat attributes.
        /// </summary>
        /// <returns><c>true</c> If the seat attributes were updated ; otherwise, <c>false</c>.</returns>
        public bool UpsertSeatAttributes(string venueId, IEnumerable<SeatAttribute> seatAttributes)
        {
            const string successStatus = "Success";
            var body = new SeatAttributesRequest {seats = seatAttributes};
            var result = ExecuteApi<IEnumerable<string>>($"v1/admin/venues/{venueId}/seats/attributes", RequestMethod.Patch, true, body);
            return result.Data?.Contains(successStatus) ?? false;
        }
    }
}