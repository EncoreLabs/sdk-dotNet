using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK.Basket
{
    public class Venue
    {
        [SerializeAs(Attribute = true, Name = "id")]
        public string id { get; set; }
    }
}
