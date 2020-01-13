using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models
{
    public class Seat
    {
        public string Area { get; set; }

        public string SeatIdentifier { get; set; }

        public List<Attribute> Attributes { get; set; }
    }
}