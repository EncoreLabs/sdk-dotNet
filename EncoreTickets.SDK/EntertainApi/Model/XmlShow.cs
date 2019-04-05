using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public enum XmlCategories
    {
        Musicals,
        Plays
    }

    public class XmlShow //: IXmlError
    {
        public int MerchantShowId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AgeRestriction { get; set; }
        public string ImportantInfo { get; set; }
        public DateTime? OnSaleDate { get; set; }
        public DateTime? BookingFrom { get; set; }
        public DateTime? BookingUntil { get; set; }
        public string Runtime { get; set; }
        public string Matinee { get; set; }
        public string Evenings { get; set; }
        public string LocationId { get; set; }
        public XmlCategories Category { get; set; }
        public int MerchantVenueId { get; set; }
        public List<XmlImage> XmlImages { get; set; }
        public List<XmlVideo> XmlVideos { get; set; }
        public List<XmlError> XmlErrors { get; set; }

        public XmlShow()
        {
            XmlImages = new List<XmlImage>();
            XmlVideos = new List<XmlVideo>();
            XmlErrors = new List<XmlError>();
        }
    }
}