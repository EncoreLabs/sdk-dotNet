using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Availability
    {
        public int? availableCount { get; set; }

        public List<Area> areas { get; set; }

        public bool isAvailable { get; set; }
    }
}
