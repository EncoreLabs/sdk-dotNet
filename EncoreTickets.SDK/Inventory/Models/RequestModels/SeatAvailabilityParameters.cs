using System;
using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Inventory.Models.RequestModels
{
    /// <summary>
    /// The helper entity for collecting parameters for obtaining seat availability.
    /// </summary>
    public class SeatAvailabilityParameters
    {
        /// <summary>
        /// Gets or sets performance date and time: if nothing is sent, current time will be used.
        /// </summary>
        public DateTime? PerformanceTime { get; set; }

        /// <summary>
        /// Gets or sets seat grouping limit.
        /// </summary>
        public int GroupingLimit { get; set; }

        /// <summary>
        /// Gets or sets ASC or DESC.
        /// Default value : ASC.
        /// </summary>
        public Direction? Direction { get; set; }

        /// <summary>
        /// Gets or sets the field you'd like to sort by (limited to price).
        /// </summary>
        public string Sort { get; set; }
    }
}
