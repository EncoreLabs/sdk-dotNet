using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Area
    {
        public int? availableCount { get; set; }

        public DateTime? date { get; set; }

        public string name { get; set; }

        public string mode { get; set; }

        public bool? isAvailable { get; set; }

        public List<Grouping> groupings { get; set; }

        public string aggregateReference { get; set; }

        public string itemReference { get; set; }
    }
}