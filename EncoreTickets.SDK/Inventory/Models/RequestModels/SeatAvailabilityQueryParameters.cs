using System;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;

namespace EncoreTickets.SDK.Inventory.Models.RequestModels
{
    /// <summary>
    /// The helper entity for collecting query parameters for obtaining seat availability.
    /// </summary>
    internal class SeatAvailabilityQueryParameters
    {
        /// <summary>
        /// Gets performance date.
        /// </summary>
        public string Date { get; }

        /// <summary>
        /// Gets performance time.
        /// </summary>
        public string Time { get; }

        /// <summary>
        /// Gets seat grouping limit.
        /// </summary>
        public int? GroupingLimit { get; }

        /// <summary>
        /// Gets ASC or DESC.
        /// Default value : ASC
        /// </summary>
        public Direction? Direction { get; }

        /// <summary>
        /// Gets the field you'd like to sort by (limited to price)
        /// </summary>
        public string Sort { get; }

        public SeatAvailabilityQueryParameters(SeatAvailabilityParameters parameters)
        {
            Date = parameters.PerformanceTime?.ToEncoreDate();
            Time = parameters.PerformanceTime?.ToEncoreTime();
            Sort = parameters.Sort;
            Direction = parameters.Direction;
            GroupingLimit = parameters.GroupingLimit > 0 ? parameters.GroupingLimit : (int?)null;
        }
    }
}
