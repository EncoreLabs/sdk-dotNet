using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class BookingRange
    {
        public DateTime? firstBookableDate { get; set; }

        public DateTime? lastBookableDate { get; set; }
    }
}