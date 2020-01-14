using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models.RequestModels
{
    internal class SeatAttributesRequest
    {
        public IEnumerable<SeatDetailed> Seats { get; set; }
    }
}
