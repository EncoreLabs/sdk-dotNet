using System;
using System.Runtime.Serialization;

namespace EncoreTickets.SDK.Inventory
{
    [DataContract]
    public class Performance : IObject
    {
        public DateTime datetime { get; set; }
        public int? largestLumpOfTickets { get; set; }

        /*
{
datetime: "2019-03-21T19:30:00+0000",
largestLumpOfTickets: 10
},
*/
    }
}