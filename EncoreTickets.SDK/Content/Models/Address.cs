namespace EncoreTickets.SDK.Content.Models
{
    public class Address
    {
        public string FirstLine { get; set; }

        public object SecondLine { get; set; }

        public object ThirdLine { get; set; }

        public string City { get; set; }

        public object PostCode { get; set; }

        public Region Region { get; set; }

        public Country Country { get; set; }
    }
}