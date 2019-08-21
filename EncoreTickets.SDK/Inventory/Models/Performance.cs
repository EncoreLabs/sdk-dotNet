using System;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Performance : IObject
    {
        public DateTime datetime { get; set; }

        public int? largestLumpOfTickets { get; set; }
    }
}