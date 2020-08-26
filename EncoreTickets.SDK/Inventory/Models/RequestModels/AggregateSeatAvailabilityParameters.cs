using System;
using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Inventory.Models.RequestModels
{
    public class AggregateSeatAvailabilityParameters
    {
        /// <summary>
        /// Gets or sets performance date and time.
        /// </summary>
        public DateTime PerformanceTime { get; set; }

        /// <summary>
        /// Gets or sets quantity of seats needed.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets ASC or DESC.
        /// Default value : ASC.
        /// </summary>
        public Direction? Direction { get; set; }
    }
}