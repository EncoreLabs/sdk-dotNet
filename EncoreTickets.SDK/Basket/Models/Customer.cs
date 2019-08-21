using RestSharp.Serializers;

namespace EncoreTickets.SDK.Basket.Models
{
    /// <summary>
    /// The customer model of the Basket service.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        public Customer()
        {
            address = new Address();
        }
    }
}
