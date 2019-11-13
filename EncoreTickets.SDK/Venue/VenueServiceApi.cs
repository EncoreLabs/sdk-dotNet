using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Venue.Models;
using EncoreTickets.SDK.Venue.Models.RequestModels;
using EncoreTickets.SDK.Venue.Models.ResponseModels;

namespace EncoreTickets.SDK.Venue
{
    /// <inheritdoc/>
    /// <summary>
    /// The service to provide an interface for calling Venue API endpoints.
    /// </summary>
    public class VenueServiceApi : BaseApi
    {
        private const string VenueHost = "venue-service.{0}tixuk.io/api/";

        /// <summary>
        /// Gets the authentication service for the current Venue service./>
        /// </summary>
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
        /// <returns></returns>
        public IList<Models.Venue> GetVenues()
        {
            var result = Executor.ExecuteApiWithWrappedResponse<List<Models.Venue>, VenuesResponse, VenuesResponseContent>(
                "v1/venues",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <summary>
        /// Get the venue by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Venue GetVenueById(string id)
        {
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Venue>(
                $"v1/venues/{id}",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <summary>
        /// Update the venue by its id.
        /// </summary>
        /// <param name="venue">Venue for update</param>
        /// <returns>Updated venue</returns>
        public Models.Venue UpdateVenueById(Models.Venue venue)
        {
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Venue>(
                $"v1/admin/venues/{venue.internalId}",
                RequestMethod.Post,
                venue);
            return result.DataOrException;
        }

        /// <summary>
        /// Get the seat attributes for a venue
        /// </summary>
        /// <param name="venue"></param>
        /// <returns></returns>
        public IList<SeatAttribute> GetSeatAttributes(Models.Venue venue)
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
            var result = Executor.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                $"v1/venues/{venueId}/seats/attributes/detailed",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <summary>
        /// Get the standard attributes
        /// </summary>
        /// <returns></returns>
        public IList<StandardAttribute> GetStandardAttributes()
        {
            var result = Executor.ExecuteApiWithWrappedResponse<List<StandardAttribute>>(
                "v1/attributes/standard",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <summary>
        /// Upsert a standard attribute by its title.
        /// </summary>
        /// <returns>The updated standard attribute.</returns>
        public StandardAttribute UpsertStandardAttributeByTitle(StandardAttribute attribute)
        {
            var result = Executor.ExecuteApiWithWrappedResponse<StandardAttribute>(
                "v1/admin/attributes",
                RequestMethod.Patch,
                attribute);
            return result.DataOrException;
        }

        /// <summary>
        /// Upsert venue's seat attributes.
        /// </summary>
        /// <returns><c>true</c> If the seat attributes were updated ; otherwise, <c>false</c>.</returns>
        public bool UpsertSeatAttributes(string venueId, IEnumerable<SeatAttribute> seatAttributes)
        {
            const string successStatus = "Success";
            var body = new SeatAttributesRequest { seats = seatAttributes };
            var result = Executor.ExecuteApiWithWrappedResponse<IEnumerable<string>>(
                $"v1/admin/venues/{venueId}/seats/attributes",
                RequestMethod.Patch,
                body);
            return result.DataOrException?.Contains(successStatus) ?? false;
        }
    }
}