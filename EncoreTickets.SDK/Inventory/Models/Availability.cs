﻿using System.Collections.Generic;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Availability
    {
        public int? AvailableCount { get; set; }

        public List<Area> Areas { get; set; }

        public bool IsAvailable { get; set; }
    }
}
