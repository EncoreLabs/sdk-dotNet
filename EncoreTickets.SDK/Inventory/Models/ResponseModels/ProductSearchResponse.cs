using System.Collections.Generic;
using EncoreTickets.SDK.Api.Results.Response;

namespace EncoreTickets.SDK.Inventory.Models.ResponseModels
{
    /// <summary>
    /// The API response for product search response.
    /// </summary>
    /// <inheritdoc/>
    internal class ProductSearchResponse : BaseWrappedApiResponse<ProductSearchResponseContent, List<Product>>
    {
        /// <inheritdoc/>
        public override List<Product> Data => Response.Product;
    }
}
