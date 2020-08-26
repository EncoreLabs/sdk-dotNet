using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Pricing.Models
{
    public class PriceQualifier
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public IList<PriceProperty> Properties { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public DateTimeOffset PublishedAt { get; set; }
    }
}
