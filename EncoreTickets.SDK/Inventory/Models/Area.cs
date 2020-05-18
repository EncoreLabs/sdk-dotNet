using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Area : BaseArea
    {
        public string AggregateReference { get; set; }

        public string ItemReference { get; set; }

        public bool? IsAvailable { get; set; }

        public List<Grouping> Groupings { get; set; }

        public AggregateReference AggregateReferenceObject { get; set; }
    }
}