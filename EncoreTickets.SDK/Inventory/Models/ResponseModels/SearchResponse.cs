using System.Collections.Generic;
using EncoreTickets.SDK.Api.Results.Response;

namespace EncoreTickets.SDK.Inventory.Models.ResponseModels
{
    internal class ProductSearchResponse : ApiResponse<List<Product>>
    {
        /// <summary>
        /// Returns the data.
        /// </summary>
        public override List<Product> Data => product;

        public List<Product> product { get; set; }
    }
}
