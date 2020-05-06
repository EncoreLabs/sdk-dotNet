using System;

namespace EncoreTickets.SDK.Content.Models
{
    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AreaCode { get; set; }

        public ShowType ShowType { get; set; }

        public DateTime? FirstPreviewDate { get; set; }

        public DateTime? OpeningDate { get; set; }

        public DateTime? BoOpensDate { get; set; }

        public DateTime? BoClosesDate { get; set; }

        public string RunTime { get; set; }

        public int FitMaximum { get; set; }

        public string RatingCode { get; set; }

        public Rating Rating { get; set; }

        public string Synopsis { get; set; }

        public Venue Venue { get; set; }

        public string OnSale { get; set; }
        
        public string ImageUrl { get; set; }

        public bool ShowFaceValue { get; set; }
    }
}