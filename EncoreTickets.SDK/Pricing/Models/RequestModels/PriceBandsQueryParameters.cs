using System;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;

namespace EncoreTickets.SDK.Pricing.Models.RequestModels
{
    internal class PriceBandsQueryParameters
    {
        public string Date { get; }

        public string Time { get; }

        public PriceBandsQueryParameters(DateTime? performanceDateTime)
        {
            Date = performanceDateTime?.ToEncoreDate();
            Time = performanceDateTime?.TimeOfDay > TimeSpan.Zero ? performanceDateTime.Value.ToEncoreTime() : null;
        }
    }
}
