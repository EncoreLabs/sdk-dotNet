using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class AvailabilityRange
    {
        public DateTime? FirstBookableDate { get; set; }

        public DateTime? LastBookableDate { get; set; }
    }
}