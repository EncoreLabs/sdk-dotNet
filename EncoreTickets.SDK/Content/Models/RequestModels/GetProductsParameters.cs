using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Content.Models.RequestModels
{
    public class GetProductsParameters : PageRequest
    {
        /// <summary>
        /// Gets or sets sort by.
        /// Default value : id
        /// </summary>
        public string Sort { get; set; }
    }
}
