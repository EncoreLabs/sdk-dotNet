using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public Venue Venue { get; set; }

        public string OnSale { get; set; }

        public DateTime BookingStarts { get; set; }

        public DateTime BookingEnds { get; set; }
    }
}