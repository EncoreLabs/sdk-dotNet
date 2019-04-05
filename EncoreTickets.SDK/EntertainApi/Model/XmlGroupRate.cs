using System;
using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class XmlGroupRate //: IXmlError
    {
        public int MerchantGroupRateId { get; set; }
        public int MerchantShowId { get; set; }
        public string Name { get; set; }
        public decimal? FaceValue { get; set; }
        public decimal? Price { get; set; }
        public string MinimumTicketQuantity { get; set; }
        public DateTime? BookingFrom { get; set; }
        public DateTime? BookingTo { get; set; }
        public DateTime? BookingBy { get; set; }
        public string ExclusionsValidity { get; set; }
        public List<XmlGroupRateDay> XmlGroupRateDays { get; set; }
        public List<XmlError> XmlErrors { get; set; }

        public XmlGroupRate()
        {
            XmlGroupRateDays = new List<XmlGroupRateDay>();
            XmlErrors = new List<XmlError>();
        }
    }
}