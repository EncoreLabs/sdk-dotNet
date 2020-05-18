using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Grouping : BaseGrouping
    {
        public string ItemReference { get; set; }

        public bool? IsAvailable { get; set; }

        public Attributes Attributes { get; set; }

        public Pricing Pricing { get; set; }

        public List<Seat> Seats { get; set; }

        public AggregateReference AggregateReferenceObject { get; set; }
    }
}