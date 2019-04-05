using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK.Basket
{
    public class Seat
    {
        [SerializeAs(Attribute = true, Name = "key")]
        public string key { get; set; }
    }
}
