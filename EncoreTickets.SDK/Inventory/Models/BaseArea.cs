using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class BaseArea
    {
        public int? AvailableCount { get; set; }

        public DateTime? Date { get; set; }

        public string Name { get; set; }

        public string Mode { get; set; }
    }
}