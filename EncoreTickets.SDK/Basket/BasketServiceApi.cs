using System;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Basket.Exceptions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using EncoreTickets.SDK.EntertainApi;

namespace EncoreTickets.SDK.Basket
{
    /// <summary>
    /// The wrapper class for the Basket service API.
    /// </summary>
    public class BasketServiceApi : BaseEntertainApi
    {
        /// <summary>
        /// Default constructor for the Basket service.
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        public BasketServiceApi(ApiContext context) : base(context, "basket-service.{0}tixuk.io/api/")
        {
        }

        /// <summary>
        /// Get details of a promotion by its ID. 
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns>Details of a promotion with the specified ID or an exception if not found.</returns>
        public Promotion GetPromotionDetails(string promotionId)
        {
            var path = $"v1/promotions/{promotionId}";
            var result = Executor.ExecuteApiWithWrappedResponse<Promotion>(path, RequestMethod.Get);
            return result.DataOrException;
        }

        /// <summary>
        /// Get details of a basket by its reference. 
        /// </summary>
        /// <param name="basketReference">Basket ID</param>
        /// <returns>Details of a basket with the specified reference or an exception if not found.</returns>
        public BasketDetails GetBasketDetails(string basketReference)
        {
            var path = $"v1/baskets/{basketReference}";
            var result = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(path, RequestMethod.Get);
            return result.DataOrException;
        }

        /// <summary>
        /// Applies promotion to a basket when this is possible.
        /// </summary>
        /// <param name="basketId">Basket ID</param>
        /// <param name="coupon">Coupon name</param>
        /// <returns>Details of a basket with the specified ID.</returns>
        /// <exception cref="ApiException">The API request failed.</exception>
        /// <exception cref="ContextApiException">The API request was successful, but the response context contains information about the invalid promo code.</exception>
        public BasketDetails UpsertPromotion(string basketId, Coupon coupon)
        {
            var body = new ApplyPromotionRequest {coupon = coupon};
            var result = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(
                $"v1/baskets/{basketId}/applyPromotion",
                RequestMethod.Patch,
                body);

            try
            {
                return result.GetDataOrContextException("notValidPromotionCode");
            }
            catch (ContextApiException e)
            {
                throw new InvalidPromoCodeException(e, coupon);
            }
            catch (ApiException e)
            {
                switch (e.ResponseCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new BasketNotFoundException(e, basketId);
                    case HttpStatusCode.BadRequest:
                        throw new BasketCannotBeModifiedException(e, basketId);
                    default:
                        throw;
                }
            }
        }
    }
}
