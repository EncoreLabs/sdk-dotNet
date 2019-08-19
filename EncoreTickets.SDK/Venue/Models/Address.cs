namespace EncoreTickets.SDK.Venue.Models
{
    public class Address
    {
        public string firstLine { get; set; }

        public string secondLine { get; set; }

        public object thirdLine { get; set; }

        public string city { get; set; }

        public string postcode { get; set; }

        public Region region { get; set; }

        public Country country { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }
    }
}