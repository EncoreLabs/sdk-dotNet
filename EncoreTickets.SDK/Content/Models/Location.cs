using System.Collections.Generic;

namespace EncoreTickets.SDK.Content.Models
{
    /// <summary>
    /// Location
    /// </summary>
    public class Location
    {
        public string name { get; set; }

        public string isoCode { get; set; }

        public List<Location> subLocations { get; set; }
    }
}
