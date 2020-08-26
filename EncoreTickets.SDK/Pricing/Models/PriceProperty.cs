using System;

namespace EncoreTickets.SDK.Pricing.Models
{
    public class PriceProperty
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
