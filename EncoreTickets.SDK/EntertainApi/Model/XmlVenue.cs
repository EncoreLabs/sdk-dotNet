using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class XmlVenue //: IXmlError
    {
        public int MerchantVenueId { get; set; }
        public string LocationId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Directions { get; set; }
        public string NearestTube { get; set; }
        public string TubeDirection { get; set; }
        public string Underground { get; set; }
        public string RailwayStation { get; set; }
        public string BusRoutes { get; set; }
        public string NightBusRoutes { get; set; }
        public string CarPark { get; set; }
        public string InCongestionZone { get; set; }
        public List<string> XmlFacilities { get; set; }
        public List<XmlImage> XmlImages { get; private set; }
        public List<XmlError> XmlErrors { get; set; }

        public XmlVenue()
        {
            XmlFacilities = new List<string>();
            XmlImages = new List<XmlImage>();
            XmlErrors = new List<XmlError>();
        }
    }
}