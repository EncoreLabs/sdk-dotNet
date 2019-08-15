using System;
using System.Collections.Generic;


namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class Ticket
    {
        public string Block { get; set; }

        public string BlockDescription { get; set; }

        public string BlockId { get; set; }

        public DateTime Date { get; set; }

        public bool Discounted { get; set; }

        public decimal FaceValue { get; set; }

        public string First { get; set; }

        public string Last { get; set; }

        public string Performance { get; set; }

        public decimal Price { get; set; }

        public bool IsRestrictedView { get; set; }

        public string Row { get; set; }

        public string SeatKey { get; set; }

        public List<string> SeatLumps { get; set; }

        public string Seats { get; set; }

        public int ShowId { get; set; }

        public decimal Total { get; set; }

        #region Helper Properties & Methods

        public string DateFormatted => Date.ToString("ddd, ") + Date.Date.Day + Date.ToString(" MMM yyyy");

        public bool Enta => !string.IsNullOrEmpty(SeatKey) && SeatKey.ToLower().StartsWith("enta");

        public string FaceValueFormatted => $"{FaceValue:C}";

        public int FirstAsInt => int.TryParse(First, out var firstAsInt) ? firstAsInt : 0;

        public int LastAsInt => int.TryParse(Last, out var lastAsInt) ? lastAsInt : 0;

        public string PriceFormatted => $"{Price:C}";

        public string SavingAsPercentageFormatted => "5%";

        public string Tag => BlockId + ":" + Block.Replace(" ", "");

        public string TimeFormatted => Date.ToString("h:mmtt").ToLower();

        #endregion
    }
}