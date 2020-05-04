using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models
{
    public class SeatLayout
    {
        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public List<PerformanceTimeItem> PerformanceTimes { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Seat> Seats { get; set; }
    }
}