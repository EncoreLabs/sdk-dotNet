using System.Collections.Generic;
using EncoreTickets.SDK.Api.Results.Response;

namespace EncoreTickets.SDK.Inventory.Models.ResponseModels
{
    /// <summary>
    /// The API response for product search response.
    /// </summary>
    /// <inheritdoc/>
    internal class ProductSearchResponse : ApiResponse<List<Product>>
    {
        /// <inheritdoc/>
        public override List<Product> Data => product;

        public List<Product> product { get; set; }
    }
}
