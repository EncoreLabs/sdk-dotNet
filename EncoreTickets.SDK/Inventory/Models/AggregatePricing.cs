using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class AggregatePricing
    {
        public List<Price> SalePrice { get; set; }

        public List<Price> FaceValue { get; set; }

        public int? PercentageDiscount { get; set; }

        public bool? IncludesBookingFee { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}