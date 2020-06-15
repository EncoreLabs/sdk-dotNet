using System;

namespace EncoreTickets.SDK.Pricing.Models
{
    public class PriceModifier
    {
        public int Id { get; set; }

        public int Mode { get; set; }

        public decimal AdjustmentValue { get; set; }

        public string AdjustmentType { get; set; }

        public int RoundingPrecision { get; set; }

        public string RoundingType { get; set; }

        public int Weight { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
