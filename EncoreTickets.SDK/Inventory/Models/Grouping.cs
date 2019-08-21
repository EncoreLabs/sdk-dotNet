using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Grouping
    {
        public string aggregateReference { get; set; }

        public string itemReference { get; set; }

        public string row { get; set; }

        public int? seatNumberStart { get; set; }

        public int? seatNumberEnd { get; set; }

        public int? availableCount { get; set; }

        public bool? isAvailable { get; set; }

        public Attributes attributes { get; set; }

        public Pricing pricing { get; set; }

        public List<Seat> seats { get; set; }
    }
}