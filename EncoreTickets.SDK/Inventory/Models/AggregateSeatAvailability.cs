using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class AggregateSeatAvailability
    {
        public string DisplayCurrency { get; set; }

        public List<AggregateArea> Areas { get; set; }

        public int? AvailableCount { get; set; }
    }
}