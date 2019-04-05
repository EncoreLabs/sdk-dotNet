using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK.Content
{
    public class Product : IObject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string areaCode { get; set; }
        public ShowType showType { get; set; }
        public Venue venue { get; set; }

        public DateTime? firstPreviewDate { get; set; }
        public DateTime? openingDate { get; set; }
        public DateTime? boOpensDate { get; set; }
        public string runTime { get; set; }
        public int fitMaximum { get; set; }
        public Rating rating { get; set; }
        public string synopsis { get; set; }        
    }

    public class ShowType
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Rating
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class Region
    {
        public string name { get; set; }
        public string isoCode { get; set; }
    }

    public class Country
    {
        public string name { get; set; }
        public string isoCode { get; set; }
    }

    public class Address
    {
        public string firstLine { get; set; }
        public object secondLine { get; set; }
        public object thirdLine { get; set; }
        public string city { get; set; }
        public Region region { get; set; }
        public Country country { get; set; }
    }

    public class Venue
    {
        public string id { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
    }

}