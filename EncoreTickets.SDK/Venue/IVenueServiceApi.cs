using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Venue.Models;

namespace EncoreTickets.SDK.Venue
{
    public interface IVenueServiceApi : IServiceApiWithAuthentication
    {
        /// <summary>
        /// Get the available venues.
        /// </summary>
        /// <returns></returns>
        IList<Models.Venue> GetVenues();

        /// <summary>
        /// Get the venue by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.Venue GetVenueById(string id);

        /// <summary>
        /// Update the venue by its id.
        /// </summary>
        /// <param name="venue">Venue for update.</param>
        /// <returns>Updated venue.</returns>
        Models.Venue UpdateVenueById(Models.Venue venue);

        /// <summary>
        /// Get the seat attributes for a venue.
        /// </summary>
        /// <param name="venue"></param>
        /// <returns></returns>
        IList<SeatDetailed> GetSeatAttributes(Models.Venue venue);

        /// <summary>
        /// Get detailed seat attributes.
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        IList<SeatDetailed> GetSeatAttributes(string venueId);

        /// <summary>
        /// Get the standard attributes.
        /// </summary>
        /// <returns></returns>
        IList<Attribute> GetStandardAttributes();

        /// <summary>
        /// Upsert a standard attribute by its title.
        /// </summary>
        /// <returns>The updated standard attribute.</returns>
        Attribute UpsertStandardAttributeByTitle(Attribute attribute);

        /// <summary>
        /// Upsert venue's seat attributes.
        /// </summary>
        /// <returns><c>true</c> If the seat attributes were updated ; otherwise, <c>false</c>.</returns>
        bool UpsertSeatAttributes(string venueId, IEnumerable<SeatDetailed> seatsWithAttributes);
    }
}
