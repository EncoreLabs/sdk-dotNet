using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models
{
    public class SeatDetailed
    {
        public string SeatIdentifier { get; set; }

        public DateTime? StartDate { get; set;}
        
        public DateTime? EndDate { get; set; }
        
        public List<string> PerformanceTimes { get; set; }

        public List<Attribute> Attributes { get; set; }
    }
}
