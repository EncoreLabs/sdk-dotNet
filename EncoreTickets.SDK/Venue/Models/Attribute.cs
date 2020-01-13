using System.Collections.Generic;

namespace EncoreTickets.SDK.Venue.Models
{
    public class Attribute
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Intention { get; set; }

        public List<string> Mapping { get; set; }
    }
}
