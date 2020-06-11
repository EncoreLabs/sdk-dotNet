using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Basket.Models
{
    public class ReservationItem : IEntityWithAggregateReference
    {
        /// <summary>
        /// Gets or sets the aggregate reference.
        /// </summary>
        public string AggregateReference { get; set; }

        /// <summary>
        /// Gets or sets the area ID.
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// Gets or sets the area name.
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// Gets or sets the row of the reservation.
        /// </summary>
        public string Row { get; set; }

        /// <summary>
        /// Gets or sets the row of the reservation.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the location description.
        /// </summary>
        public string LocationDescription { get; set; }
    }
}
