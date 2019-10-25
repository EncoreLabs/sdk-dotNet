using System;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Promotion : IIdentifiableObject
    {
        public string id { get; internal set; }

        public string name { get; internal set; }

        public string displayText { get; internal set; }

        public string description { get; internal set; }

        public string reference { get; internal set; }

        public string reportingCode { get; internal set; }

        public DateTimeOffset validFrom { get; internal set; }

        public DateTimeOffset validTo { get; internal set; }
    }
}
