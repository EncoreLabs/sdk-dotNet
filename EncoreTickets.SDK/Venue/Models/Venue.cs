using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Venue.Models
{
    public class Venue : IObject
    {
        public string compositeId { get; set; }

        public string internalId { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public Address address { get; set; }

        public SeatSettings seatSettings { get; set; }

        public List<object> seatLayouts { get; set; }

        public List<VenueTerminal> venueTerminals { get; set; }

        public List<Facility> facilities { get; set; }

        public List<object> transportAttributes { get; set; }

        public DateTime? contentOverriddenAt { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
}