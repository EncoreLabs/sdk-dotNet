using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Availability
    {
        public DateTime DateTime { get; set; }

        public int LargestLumpOfTickets { get; set; }
    }
}