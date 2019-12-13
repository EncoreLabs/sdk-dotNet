using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models
{
    public class Terminal
    {
        public string Name { get; set; }

        public List<Route> Routes { get; set; }
    }
}