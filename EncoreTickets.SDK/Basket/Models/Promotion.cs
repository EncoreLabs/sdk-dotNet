using System;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Promotion : IIdentifiableObject
    {
        public string id { get; set; }

        public string name { get; set; }

        public string displayText { get; set; }

        public string description { get; set; }

        public string reference { get; set; }

        public string reportingCode { get; set; }

        public DateTimeOffset validFrom { get; set; }

        public DateTimeOffset validTo { get; set; }
    }
}
