using System;
using System.Collections.Generic;
using System.Text;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Content
{
    /// <summary>
    /// Location
    /// </summary>
    public class Location : IObject
    {
        public string name { get; set; }
        public string isoCode { get; set; }
        public List<Location> subLocations { get; set; }
    }
}
