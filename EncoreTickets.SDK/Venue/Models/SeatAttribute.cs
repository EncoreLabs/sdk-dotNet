using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models
{
    public class SeatAttribute
    {
        public string SeatIdentifier { get; set; }

        public string StartDate { get; set;}
        
        public string EndDate { get; set; }
        
        public List<string> PerformanceTimes { get; set; }

        public List<Attribute> Attributes { get; set; }
    }
}
