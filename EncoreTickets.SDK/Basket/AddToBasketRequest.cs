using System;
using RestSharp.Serializers;

namespace EncoreTickets.SDK.Basket
{
    [SerializeAs(Name = "basket")]
    public class AddToBasketRequest
    {
        [SerializeAs(Name = "transaction")]
        public Transaction transaction { get; set; }

        [SerializeAs(Name = "product")]
        public Product product { get; set; }
    }
}
