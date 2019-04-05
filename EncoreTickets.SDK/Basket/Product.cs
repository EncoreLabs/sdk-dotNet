using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK.Basket
{
    public class Product
    {
        [SerializeAs(Attribute = true, Name = "id")]
        public string id { get; set; }

        [SerializeAs(Name = "type")]
        public string type { get; set; }

        [SerializeAs(Name = "venue")]
        public Venue venue { get; set; }

        [SerializeAs(Name = "performance")]
        public Performance performance { get; set; }

        [SerializeAs(Name = "date")]
        public string date { get; set; }

        [SerializeAs(Name = "quantity")]
        public string quantity { get; set; }

        [SerializeAs(Name = "seat")]
        public Seat seat { get; set; }

        [SerializeAs(Name = "startFrom")]
        public string startFrom { get; set; }

        public Product()
        {
            venue = new Venue();
            performance = new Performance();
            seat = new Seat();
        }
    }
}
