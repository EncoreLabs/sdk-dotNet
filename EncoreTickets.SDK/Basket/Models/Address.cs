using RestSharp.Serializers;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Address
    {
        [SerializeAs(Attribute = true, Name = "type")]
        public string Type { get; set; }

        [SerializeAs(Name = "line1")]
        public string Line1 { get; set; }

        [SerializeAs(Name = "line2")]
        public string Line2 { get; set; }

        [SerializeAs(Name = "city")]
        public string City { get; set; }

        [SerializeAs(Name = "county")]
        public string County { get; set; }

        [SerializeAs(Name = "postcode")]
        public string Postcode { get; set; }

        [SerializeAs(Name = "country")]
        public string Country { get; set; }

        public Address()
        {
            Type = "C";
        }
    }
}
