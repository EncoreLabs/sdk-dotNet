using System;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class XmlSpecialOffer
    {
        public int MerchantShowId { get; set; }
        public string Title { get; set; }
        public Nullable<decimal> NormalPrice { get; set; }
        public Nullable<decimal> OfferPrice { get; set; }
        public string Conditions { get; set; }
        public string Exclusions { get; set; }
    }
}