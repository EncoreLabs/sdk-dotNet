using System;

namespace EncoreTickets.SDK.Pricing.Models
{
    public class DailyPriceRange : PriceRange
    {
        public DateTimeOffset? Date { get; set; }
    }
}
