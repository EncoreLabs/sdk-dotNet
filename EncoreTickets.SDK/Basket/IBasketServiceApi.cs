using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Basket.Exceptions;
using EncoreTickets.SDK.Basket.Models;

namespace EncoreTickets.SDK.Basket
{
    /// <summary>
    /// The interface of a basket service.
    /// </summary>
    public interface IBasketServiceApi
    {
        /// <summary>
        /// Get details of a promotion by its ID. 
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns>Details of a promotion with the specified ID or an exception if not found.</returns>
        Promotion GetPromotionDetails(string promotionId);

        /// <summary>
        /// Get details of a basket by its reference. 
        /// </summary>
        /// <param name="basketReference">Basket ID</param>
        /// <returns>Details of a basket with the specified reference or an exception if not found.</returns>
        BasketDetails GetBasketDetails(string basketReference);

        /// <summary>
        /// Applies promotion to a basket when this is possible.
        /// </summary>
        /// <param name="basketId">Basket ID</param>
        /// <param name="coupon">Coupon name</param>
        /// <returns>Details of a basket with the specified ID.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        /// <exception cref="BasketNotFoundException">The API request failed if a requested basket was not found.</exception>
        /// <exception cref="BasketCannotBeModifiedException">The API request failed if an API request tried to modify a basket that was not available for change.</exception>
        /// <exception cref="InvalidPromoCodeException">The API request was successful, but the response context contains information about the invalid promo code.</exception>
        BasketDetails UpsertPromotion(string basketId, Coupon coupon);
    }
}