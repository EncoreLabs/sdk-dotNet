using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;

namespace EncoreTickets.SDK.Inventory.Models.RequestModels
{
    /// <summary>
    /// The helper entity for collecting query parameters for obtaining aggregated seat availability.
    /// </summary>
    internal class AggregateSeatAvailabilityQueryParameters
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
        /// Gets or sets quantity of seats needed.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets direction.
        /// </summary>
        public string Direction { get; }

        public AggregateSeatAvailabilityQueryParameters(AggregateSeatAvailabilityParameters parameters)
        {
            Date = parameters.PerformanceTime.ToEncoreDate();
            Time = parameters.PerformanceTime.ToEncoreTime();
            Quantity = parameters.Quantity;
            var allPossibleDirections = EnumExtension.GetEnumValues<Direction>();
            Direction = parameters.Direction.HasValue && allPossibleDirections.Contains(parameters.Direction.Value)
                ? parameters.Direction.ToString().ToLower()
                : null;
        }
    }
}