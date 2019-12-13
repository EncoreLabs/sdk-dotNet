using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Grouping
    {
        public string AggregateReference { get; set; }

        public string ItemReference { get; set; }

        public string Row { get; set; }

        public int? SeatNumberStart { get; set; }

        public int? SeatNumberEnd { get; set; }

        public int? AvailableCount { get; set; }

        public bool? IsAvailable { get; set; }

        public Attributes Attributes { get; set; }

        public Pricing Pricing { get; set; }

        public List<Seat> Seats { get; set; }
    }
}