using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Utilities.Common.Serializers;
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
        private const string VenueApiHost = "venue-service.{0}tixuk.io/api/";

        /// <summary>
        /// Default constructor for the Venue service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="automaticAuthentication"></param>
        public VenueServiceApi(ApiContext context, bool automaticAuthentication = false)
            : base(context, VenueApiHost, automaticAuthentication)
        {
            context.AuthenticationMethod = AuthenticationMethod.JWT;
        }

        /// <inheritdoc/>
        public IList<Models.Venue> GetVenues()
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = "v1/venues",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<Models.Venue>, VenuesResponse, VenuesResponseContent>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public Models.Venue GetVenueById(string id)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/venues/{id}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Venue>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public Models.Venue UpdateVenueById(Models.Venue venue)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/admin/venues/{venue.InternalId}",
                Method = RequestMethod.Post,
                Body = venue
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Venue>(parameters);
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
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/venues/{venueId}/seats/attributes/detailed",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public IList<StandardAttribute> GetStandardAttributes()
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = "v1/attributes/standard",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<StandardAttribute>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public StandardAttribute UpsertStandardAttributeByTitle(StandardAttribute attribute)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = "v1/admin/attributes",
                Method = RequestMethod.Patch,
                Body = attribute
            };
            var result = Executor.ExecuteApiWithWrappedResponse<StandardAttribute>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc/>
        public bool UpsertSeatAttributes(string venueId, IEnumerable<SeatAttribute> seatAttributes)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/admin/venues/{venueId}/seats/attributes",
                Method = RequestMethod.Patch,
                Body = new SeatAttributesRequest {Seats = seatAttributes},
                Deserializer = new SingleOrListJsonSerializer<string>()
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<string>>(parameters);
            return GetUpsertSeatAttributesResult(result);
        }

        private bool GetUpsertSeatAttributesResult(ApiResult<List<string>> apiResult)
        {
            const string successStatus = "Success";
            try
            {
                return apiResult.DataOrException?.Contains(successStatus) ?? false;
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