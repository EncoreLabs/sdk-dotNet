using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Constants;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Utilities.Serializers;
using EncoreTickets.SDK.Utilities.Serializers.Converters;
using EncoreTickets.SDK.Venue.Models;
using EncoreTickets.SDK.Venue.Models.RequestModels;
using Attribute = EncoreTickets.SDK.Venue.Models.Attribute;

namespace EncoreTickets.SDK.Venue
{
    /// <inheritdoc cref="BaseApiWithAuthentication" />
    /// <inheritdoc cref="IVenueServiceApi" />
    /// <summary>
    /// The service to provide an interface for calling Venue API endpoints.
    /// </summary>
    public class VenueServiceApi : BaseApiWithAuthentication, IVenueServiceApi
    {
        private const string VenueApiHost = "venue-service.{0}tixuk.io/api/";

        /// <inheritdoc/>
        public override int? ApiVersion => 2;

        /// <summary>
        /// Default constructor for the Venue service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="automaticAuthentication"></param>
        public VenueServiceApi(ApiContext context, bool automaticAuthentication = false)
            : base(context, VenueApiHost, automaticAuthentication)
        {
        }

        /// <inheritdoc/>
        public IList<Models.Venue> GetVenues()
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/venues",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResultsInResponse<List<Models.Venue>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public Models.Venue GetVenueById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("venue ID must be set");
            }

            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/venues/{id}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Venue>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public Models.Venue UpdateVenueById(Models.Venue venue)
        {
            if (string.IsNullOrEmpty(venue?.InternalId))
            {
                throw new ArgumentException("venue ID must be set");
            }

            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/admin/venues/{venue.InternalId}",
                Method = RequestMethod.Post,
                Body = venue
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Venue>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public IList<Attribute> GetStandardAttributes()
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/attributes/standard",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<Attribute>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public Attribute UpsertStandardAttributeByTitle(Attribute attribute)
        {
            if (string.IsNullOrEmpty(attribute?.Title))
            {
                throw new ArgumentException("attribute title must be set");
            }

            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/admin/attributes",
                Method = RequestMethod.Patch,
                Body = attribute
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Attribute>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public IList<SeatDetailed> GetSeatAttributes(Models.Venue venue)
        {
            var venueId = venue?.InternalId;
            return GetSeatAttributes(venueId);
        }

        /// <inheritdoc/>
        public IList<SeatDetailed> GetSeatAttributes(string venueId)
        {
            if (string.IsNullOrEmpty(venueId))
            {
                throw new ArgumentException("venue ID must be set");
            }

            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/venues/{venueId}/seats/attributes/detailed",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<SeatDetailed>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public bool UpsertSeatAttributes(string venueId, IEnumerable<SeatDetailed> seatAttributes)
        {
            if (string.IsNullOrEmpty(venueId))
            {
                throw new ArgumentException("venue ID must be set");
            }

            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/admin/venues/{venueId}/seats/attributes",
                Method = RequestMethod.Patch,
                Body = new SeatAttributesRequest
                {
                    Seats = seatAttributes ?? new List<SeatDetailed>()
                },
                DateFormat = "yyyy-MM-dd",
                Deserializer = new DefaultJsonSerializer(new[] {new SingleOrListToListConverter<string>()})
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<string>>(parameters);
            return result.DataOrException?.Any(x =>
                x.Equals(ActionResultStatuses.Success, StringComparison.InvariantCultureIgnoreCase)) ?? false;
        }
    }
}