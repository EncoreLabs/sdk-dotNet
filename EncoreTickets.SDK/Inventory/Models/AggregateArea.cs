using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class AggregateArea : BaseArea
    {
        public List<AggregateGrouping> Groupings { get; set; }
    }
}