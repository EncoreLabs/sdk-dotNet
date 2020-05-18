using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class AggregateGrouping : BaseGrouping
    {
        public AggregatePricing Pricing { get; set; }

        public List<AggregateSeat> Seats { get; set; }
    }
}