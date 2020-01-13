using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models
{
    public class Venue
    {
        public string CompositeId { get; set; }

        public string InternalId { get; set; }

        public string Title { get; set; }

        public string CardTitle { get; set; }

        public string Description { get; set; }

        public Address Address { get; set; }

        public SeatSettings SeatSettings { get; set; }

        public List<SeatLayout> SeatLayouts { get; set; }

        public List<VenueTerminal> VenueTerminals { get; set; }

        public List<Facility> Facilities { get; set; }

        public List<TransportAttribute> TransportAttributes { get; set; }

        public DateTime? ContentOverriddenAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool Published { get; set; }
    }
}