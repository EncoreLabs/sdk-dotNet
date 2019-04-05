using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK.Basket
{
    public class Mail
    {
        [SerializeAs(Attribute = true, Name = "type")]
        public string Type { get; set; }

        public Mail()
        {
            Type = "C";
        }
    }
}
