using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models
{
    public class SeatAttribute
    {
        public string seatIdentifier { get; set; }

        public string startDate { get; set;}
        
        public string endDate { get; set; }
        
        public List<string> performanceTimes { get; set; }

        public List<Attribute> attributes { get; set; }
    }
}
