using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Area
    {
        public string AggregateReference { get; set; }

        public string ItemReference { get; set; }

        public bool? IsAvailable { get; set; }

        public int? AvailableCount { get; set; }

        public DateTime? Date { get; set; }

        public string Name { get; set; }

        public string Mode { get; set; }

        public List<Grouping> Groupings { get; set; }
    }
}