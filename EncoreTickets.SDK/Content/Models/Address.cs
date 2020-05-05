namespace EncoreTickets.SDK.Content.Models
{
    public class Address
    {
        public string FirstLine { get; set; }

        public string SecondLine { get; set; }

        public string ThirdLine { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public Location Region { get; set; }

        public Location Country { get; set; }
    }
}