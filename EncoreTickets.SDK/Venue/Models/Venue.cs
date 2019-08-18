using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Venue
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

    public class Address
    {
        public string firstLine { get; set; }
        public string secondLine { get; set; }
        public object thirdLine { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }
        public Region region { get; set; }
        public Country country { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class Region
    {
        public string name { get; set; }
        public string isoCode { get; set; }
    }

    public class Country
    {
        public string name { get; set; }
        public string isoCode { get; set; }
    }

    public class SeatSettings
    {
        public bool seatsSupplied { get; set; }
        public SeatSelectionMode seatSelectionMode { get; set; }
        public AllocationType allocationType { get; set; }
    }

    public class SeatSelectionMode
    {
        public string name { get; set; }
    }

    public class AllocationType
    {
        public string name { get; set; }
    }

    public class VenueTerminal
    {
        public string directions { get; set; }
        public string journeyTime { get; set; }
        public Terminal terminal { get; set; }
    }

    public class Terminal
    {
        public string name { get; set; }
        public List<Route> routes { get; set; }
    }

    public class Route
    {
        public string description { get; set; }
        public TransportMode transportMode { get; set; }
    }

    public class TransportMode
    {
        public string name { get; set; }
    }

    public class Facility
    {
        public string description { get; set; }
    }
}