using System.Collections.Generic;
using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class BaseGrouping : IEntityWithAggregateReference
    {
        public string GroupIdentifier { get; set; }

        public string AggregateReference { get; set; }

        public string Row { get; set; }

        public int? SeatNumberStart { get; set; }

        public int? SeatNumberEnd { get; set; }

        public int? AvailableCount { get; set; }

        public List<SeatLump> SeatLumps { get; set; }
    }
}