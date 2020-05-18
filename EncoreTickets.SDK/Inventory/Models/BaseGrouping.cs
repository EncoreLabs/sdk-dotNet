using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class BaseGrouping
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