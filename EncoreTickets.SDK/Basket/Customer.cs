using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK.Basket
{
    public class Customer
    {
        [SerializeAs(Name = "title")]
        public string title { get; set; }

        [SerializeAs(Name = "firstName")]
        public string firstName { get; set; }

        [SerializeAs(Name = "lastName")]
        public string lastName { get; set; }

        [SerializeAs(Name = "email")]
        public string email { get; set; }

        [SerializeAs(Name = "address")]
        public Address address { get; set; }

        [SerializeAs(Name = "phone")]
        public string phone { get; set; }

        public Customer()
        {
            address = new Address();            
        }
    }
}
