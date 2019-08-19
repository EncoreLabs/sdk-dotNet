using System;
using System.Runtime.Serialization;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Inventory.Models
{
    [DataContract]
    public class Performance : IObject
    {
        public DateTime datetime { get; set; }

        public int? largestLumpOfTickets { get; set; }
    }
}