using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Basket.Exceptions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;

namespace EncoreTickets.SDK.Basket
{
    /// <summary>
    /// The interface of a basket service.
    /// </summary>
    public interface IBasketServiceApi : IServiceApi
    {
        /// <summary>
        /// Get details of a basket by its reference.
        /// GET /api/v{ApiVersion}/baskets/{reference}
        /// </summary>
        /// <param name="basketReference">Basket ID</param>
        /// <returns>Details of a basket with the specified reference or an exception if not found.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        Models.Basket GetBasketDetails(string basketReference);

        /// <summary>
        /// Get basket delivery options by reference.
        /// GET /api/v1/baskets/{reference}/deliveryOptions
        /// </summary>
        /// <param name="basketReference">Basket ID</param>
        /// <returns>If the reference is correct, the method returns the basket delivery options; otherwise, an exception.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        IList<Delivery> GetBasketDeliveryOptions(string basketReference);

        /// <summary>
        /// Creates or updates a basket when it is possible.
        /// PATCH /api/v{ApiVersion}/baskets
        /// </summary>
        /// <param name="source">Object containing the details of the upserted basket</param>
        /// <param name="hasFlexiTickets">Flag indicating whether the basket needs Flexi tickets.
        /// If the value is null, it will be added based on the permission of Flexi tickets from the basket.</param>
        /// <returns>Details of the upserted basket.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        Models.Basket UpsertBasket(Models.Basket source, bool? hasFlexiTickets = null);

        /// <summary>
        /// Creates or updates a basket when it is possible.
        /// PATCH /api/v{ApiVersion}/baskets
        /// </summary>
        /// <param name="parameters">Object containing the details of the upserted basket</param>
        /// <returns>Details of the upserted basket.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        Models.Basket UpsertBasket(UpsertBasketParameters parameters);

        /// <summary>
        /// Applies promotion to a basket when this is possible.
        /// PATCH /api/v{ApiVersion}/baskets/{reference}/applyPromotion
        /// </summary>
        /// <param name="basketReference">Basket ID</param>
        /// <param name="couponName">Coupon name</param>
        /// <returns>Details of a basket with the specified ID.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        /// <exception cref="BasketNotFoundException">The API request failed if a requested basket was not found.</exception>
        /// <exception cref="BasketCannotBeModifiedException">The API request failed if an API request tried to modify a basket that was not available for change.</exception>
        /// <exception cref="InvalidPromoCodeException">The API request was successful, but the response context contains information about the invalid promo code.</exception>
        Models.Basket UpsertPromotion(string basketReference, string couponName);

        /// <summary>
        /// Applies promotion to a basket when this is possible.
        /// PATCH /api/v{ApiVersion}/baskets/{reference}/applyPromotion
        /// </summary>
        /// <param name="basketReference">Basket ID</param>
        /// <param name="coupon">Coupon</param>
        /// <returns>Details of a basket with the specified ID.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        /// <exception cref="BasketNotFoundException">The API request failed if a requested basket was not found.</exception>
        /// <exception cref="BasketCannotBeModifiedException">The API request failed if an API request tried to modify a basket that was not available for change.</exception>
        /// <exception cref="InvalidPromoCodeException">The API request was successful, but the response context contains information about the invalid promo code.</exception>
        Models.Basket UpsertPromotion(string basketReference, Coupon coupon);

        /// <summary>
        /// Removes all reservations from the basket.
        /// PATCH /api/v{ApiVersion}/baskets/{reference}/clear
        /// </summary>
        /// <param name="basketReference">Basket ID</param>
        /// <returns>Details of the updated basket.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        Models.Basket ClearBasket(string basketReference);

        /// <summary>
        /// Removes a reservation with the specified ID from the basket.
        /// DELETE /api/v{ApiVersion}/baskets/{reference}/reservations/{reservationId}
        /// </summary>
        /// <param name="basketReference">Basket ID</param>
        /// <param name="reservationId">Reservation ID</param>
        /// <returns>Details of the updated basket.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        Models.Basket RemoveReservation(string basketReference, int reservationId);

        /// <summary>
        /// Removes a reservation with the specified ID from the basket.
        /// DELETE /api/v{ApiVersion}/baskets/{reference}/reservations/{reservationId}
        /// </summary>
        /// <param name="basketReference">Basket ID</param>
        /// <param name="reservationId">Reservation ID</param>
        /// <returns>Details of the updated basket.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        Models.Basket RemoveReservation(string basketReference, string reservationId);

        /// <summary>
        /// Gets a promotion list.
        /// GET /api/v{ApiVersion}/promotions
        /// </summary>
        /// <returns>Promotions list.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        IList<Promotion> GetPromotions(PageRequest pageParameters = null);

        /// <summary>
        /// Gets details of a promotion by its ID.
        /// GET /api/v{ApiVersion}/promotions/{promotionId}
        /// </summary>
        /// <param name="promotionId">Promotion ID</param>
        /// <returns>Details of a promotion with the specified ID or an exception if not found.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        Promotion GetPromotionDetails(string promotionId);
    }
}