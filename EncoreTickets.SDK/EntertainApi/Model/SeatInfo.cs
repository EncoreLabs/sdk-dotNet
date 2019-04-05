using System;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class SeatInfo
    {
        public string showId { get; set; }
        public string venueId { get; set; }
        public DateTime? date { get; set; }
        public string eveMat { get; set; }
        public string noTickets { get; set; }
        public string blockId { get; set; }
        public string seatKey { get; set; }
        public string startFrom { get; set; }
        public string availabilityUrl { get; set; }

        public string Day
        {
            get { return date.HasValue ? date.Value.ToString("dd") : ""; }
        }

        public string MonthYear
        {
            get { return date.HasValue ? date.Value.ToString("yyyyZMM") : ""; }
        }
    }
}