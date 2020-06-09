using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Pricing.Models
{
    public class PriceRule
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public int Weight { get; set; }

        public int Active { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public IList<PriceQualifier> Qualifiers { get; set; }

        public IList<PriceModifier> Modifiers { get; set; }
    }
}
