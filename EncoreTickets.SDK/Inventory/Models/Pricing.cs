using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Pricing
    {
        public string PriceReference { get; set; }

        public Price SalePrice { get; set; }

        public Price FaceValue { get; set; }

        public int? Percentage { get; set; }

        public bool? Offer { get; set; }

        public bool? NoBookingFee { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}