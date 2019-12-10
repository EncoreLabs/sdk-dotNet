using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Venue.Exceptions;
using EncoreTickets.SDK.Venue.Models;
using EncoreTickets.SDK.Venue.Models.RequestModels;
using EncoreTickets.SDK.Venue.Models.ResponseModels;

namespace EncoreTickets.SDK.Venue
{
    /// <inheritdoc cref="BaseApiWithAuthentication" />
    /// <inheritdoc cref="IVenueServiceApi" />
    /// <summary>
    /// The service to provide an interface for calling Venue API endpoints.
    /// </summary>
    public class VenueServiceApi : BaseApiWithAuthentication, IVenueServiceApi
    {
        private const string VenueHost = "venue-service.{0}tixuk.io/api/";

        /// <summary>
        /// Default constructor for the Venue service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="automaticAuthentication"></param>
        public VenueServiceApi(ApiContext context, bool automaticAuthentication = false) : base(context, VenueHost, automaticAuthentication)
        {
            context.AuthenticationMethod = AuthenticationMethod.JWT;
        }

        /// <inheritdoc/>
        public IList<Models.Venue> GetVenues()
        {
            var result = Executor.ExecuteApiWithWrappedResponse<List<Models.Venue>, VenuesResponse, VenuesResponseContent>(
                "v1/venues",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public Models.Venue GetVenueById(string id)
        {
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Venue>(
                $"v1/venues/{id}",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public Models.Venue UpdateVenueById(Models.Venue venue)
        {
            TriggerAutomaticAuthentication();
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Venue>(
                $"v1/admin/venues/{venue.InternalId}",
                RequestMethod.Post,
                venue);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public IList<SeatAttribute> GetSeatAttributes(Models.Venue venue)
        {
            return GetSeatAttributes(venue.InternalId);
        }

        /// <inheritdoc/>
        public IList<SeatAttribute> GetSeatAttributes(string venueId)
        {
            var result = Executor.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                $"v1/venues/{venueId}/seats/attributes/detailed",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public IList<StandardAttribute> GetStandardAttributes()
        {
            var result = Executor.ExecuteApiWithWrappedResponse<List<StandardAttribute>>(
                "v1/attributes/standard",
                RequestMethod.Get);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public StandardAttribute UpsertStandardAttributeByTitle(StandardAttribute attribute)
        {
            TriggerAutomaticAuthentication();
            var result = Executor.ExecuteApiWithWrappedResponse<StandardAttribute>(
                "v1/admin/attributes",
                RequestMethod.Patch,
                attribute);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public bool UpsertSeatAttributes(string venueId, IEnumerable<SeatAttribute> seatAttributes)
        {
            TriggerAutomaticAuthentication();
            const string successStatus = "Success";
            var body = new SeatAttributesRequest { Seats = seatAttributes };
            var result = Executor.ExecuteApiWithWrappedResponse<string>(
                $"v1/admin/venues/{venueId}/seats/attributes",
                RequestMethod.Patch,
                body);

            try
            {
                return result.DataOrException?.Contains(successStatus) ?? false;
            }
            catch (ApiException exception)
            {
                if (exception.ResponseCode == default) // hack, because RestSharp is currently returning “request was aborted” for a long response with a 403 status
                {
                    throw new AccessTokenExpiredException(exception);
                }

                throw;
            }
        }
    }
}