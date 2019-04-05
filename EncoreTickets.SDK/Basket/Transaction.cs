using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK.Basket
{
    public class Transaction
    {
        [SerializeAs(Attribute = true, Name = "reference")]
        public string Reference { get; set; }

        [SerializeAs(Name = "password")]
        public string Password { get; set; }
    }
}
