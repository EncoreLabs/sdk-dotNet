using System;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class Reservation
    {
        public string reservationId { get; set; }
        public string showId { get; set; }
        public string showName { get; set; }
        public string showImageUrl { get; set; }
        public string showReviewUrl { get; set; }
        public string venueId { get; set; }
        public string venueName { get; set; }
        public string venueAddress { get; set; }
        public string venueMap { get; set; }
        public DateTime performance { get; set; }
        public string seats { get; set; }
        public string blockId { get; set; }
        public string block { get; set; }
        public string noOfTickets { get; set; }
        public decimal facevalue { get; set; }
        public decimal price { get; set; }
        public decimal total { get; set; }
        public decimal? commission { get; set; }
        public bool hideFaceValue { get; set; }
        public string rebookingUrl { get; set; }
        public BasketItemHistory basketItemHistory { get; set; }

        #region Helper Properties & Methods

        public bool Enta
        {
            get { return basketItemHistory != null && basketItemHistory.enta; }
        }

        public bool ExpiredBasketItemHistory
        {
            get
            {
                return basketItemHistory != null && basketItemHistory.enta && basketItemHistory.basketExpiry <= DateTime.Now;
            }
        }

        public string SavingAsPercentageFormatted
        {
            get
            {
                return facevalue > 0 && price > 0 && facevalue > price
                    ? ((facevalue - price) / facevalue).ToString()
                    : string.Empty;
            }
        }

        public string Tag
        {
            get { return blockId + ":" + block.Replace(" ", ""); }
        }

        #endregion
    }
}