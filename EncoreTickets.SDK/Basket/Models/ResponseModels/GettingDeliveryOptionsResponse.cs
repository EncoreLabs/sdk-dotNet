using System.Collections.Generic;
using EncoreTickets.SDK.Api.Results.Response;

namespace EncoreTickets.SDK.Basket.Models.ResponseModels
{
    /// <summary>
    /// The API response for getting delivery options response.
    /// </summary>
    /// <inheritdoc/>
    internal class GettingDeliveryOptionsResponse : BaseWrappedApiResponse<GettingDeliveryOptionsResponseContent, List<Delivery>>
    {
        /// <inheritdoc/>
        public override List<Delivery> Data => Response.Results;
    }

    internal class GettingDeliveryOptionsResponseContent
    {
        public List<Delivery> Results { get; set; }
    }
}
