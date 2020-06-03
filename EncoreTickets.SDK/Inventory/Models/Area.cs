using System.Collections.Generic;
using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Area : BaseArea, IEntityWithAggregateReference
    {
        public string AggregateReference { get; set; }

        public string ItemReference { get; set; }

        public bool? IsAvailable { get; set; }

        public List<Grouping> Groupings { get; set; }

        public AggregateReference AggregateReferenceObject { get; set; }
    }
}