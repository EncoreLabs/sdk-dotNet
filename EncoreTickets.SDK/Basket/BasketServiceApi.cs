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
    /// <inheritdoc cref="BaseEntertainApi" />
    /// <inheritdoc cref="IBasketServiceApi" />
    /// <summary>
    /// The wrapper class for the Basket service API.
    /// </summary>
    public class BasketServiceApi : BaseEntertainApi, IBasketServiceApi
    {
        /// <summary>
        /// Default constructor for the Basket service.
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        public BasketServiceApi(ApiContext context) : base(context, "basket-service.{0}tixuk.io/api/")
        {
        }

        /// <inheritdoc />
        public Promotion GetPromotionDetails(string promotionId)
        {
            var path = $"v1/promotions/{promotionId}";
            var result = Executor.ExecuteApiWithWrappedResponse<Promotion>(path, RequestMethod.Get);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public BasketDetails GetBasketDetails(string basketReference)
        {
            var path = $"v1/baskets/{basketReference}";
            var result = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(path, RequestMethod.Get);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public BasketDetails UpsertPromotion(string basketId, Coupon coupon)
        {
            var body = new ApplyPromotionRequest {coupon = coupon};
            var result = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(
                $"v1/baskets/{basketId}/applyPromotion",
                RequestMethod.Patch,
                body);
            return GetUpsertPromotionResult(result, coupon, basketId);
        }

        /// <inheritdoc />
        public BasketDetails UpsertBasket(UpsertBasketRequest request)
        {
            var response = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(
                "v1/baskets",
                RequestMethod.Patch,
                request);
            return response.DataOrException;
        }

        /// <inheritdoc />
        public BasketDetails RemoveReservation(string basketId, int reservationId)
        {
            var response = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(
                $"v1/baskets/{basketId}/reservations/{reservationId}",
                RequestMethod.Delete);
            return response.DataOrException;
        }

        public BasketDetails ClearBasket(string basketId)
        {
            var response = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(
                $"v1/baskets/{basketId}/clear",
                RequestMethod.Patch);
            return response.DataOrException;
        }

        private BasketDetails GetUpsertPromotionResult(ApiResult<BasketDetails> apiResult, Coupon coupon, string basketId)
        {
            try
            {
                return apiResult.GetDataOrContextException("notValidPromotionCode");
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
