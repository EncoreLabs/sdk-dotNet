using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Venue
{
    public class SeatAttribute : IObject
    {
        public string seatIdentifier { get; set; }

        public string startDate { get; set;}
        
        public string endDate { get; set; }
        
        public string[] performanceTimes { get; set; }

        public List<Attribute> attributes { get; set; }
    }
}
