namespace EncoreTickets.SDK.Venue.Models
{
    public class Address
    {
        public string FirstLine { get; set; }

        public string SecondLine { get; set; }

        public string ThirdLine { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public IsoLocation Region { get; set; }

        public IsoLocation Country { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}