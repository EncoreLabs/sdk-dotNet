using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK.Basket
{
    public class Performance
    {
        [SerializeAs(Attribute = true, Name = "type")]
        public string type { get; set; }

        public Performance()
        {
            type = "A";
        }
    }
}
