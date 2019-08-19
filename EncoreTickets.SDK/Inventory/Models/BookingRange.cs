using System;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class BookingRange : IObject
    {
        public DateTime? firstBookableDate { get; set; }

        public DateTime? lastBookableDate { get; set; }
    }
}