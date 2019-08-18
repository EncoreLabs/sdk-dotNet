using System;
using System.Runtime.Serialization;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Inventory
{
    [DataContract]
    public class Product : IIdentifiableObject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public Venue venue { get; set; }
        public string onSale { get; set; }
        public DateTime bookingStarts { get; set; }
        public DateTime bookingEnds { get; set; }

        /*
         * id: 1587,
name: "Wicked",
type: "show",
venue: {
id: 138
},
onSale: "yes",
bookingStarts: "2018-08-07T00:00:00+0000",
bookingEnds: "2019-05-25T00:00:00+0000"
*/
    }

    /// <summary>
    /// Represents a venue object
    /// </summary>
    public class Venue : IIdentifiableObject
    {
        public string id { get; set; }
    }
}