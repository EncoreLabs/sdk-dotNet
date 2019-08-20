namespace EncoreTickets.SDK.Content.Models
{
    public class Address
    {
        public string firstLine { get; set; }

        public object secondLine { get; set; }

        public object thirdLine { get; set; }

        public string city { get; set; }

        public object postCode { get; set; }

        public Region region { get; set; }

        public Country country { get; set; }
    }
}