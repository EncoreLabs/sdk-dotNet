using System.Net;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Basket.Exceptions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Basket
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="IBasketServiceApi" />
    /// <summary>
    /// The wrapper class for the Basket service API.
    /// </summary>
    public class BasketServiceApi : BaseApi, IBasketServiceApi
    {
        private const string BasketApiHost = "basket-service.{0}tixuk.io/api/";

        /// <summary>
        /// Default constructor for the Basket service.
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        public BasketServiceApi(ApiContext context) : base(context, BasketApiHost)
        {
        }

        /// <inheritdoc />
        public Promotion GetPromotionDetails(string promotionId)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/promotions/{promotionId}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Promotion>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public BasketDetails GetBasketDetails(string basketReference)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/baskets/{basketReference}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public BasketDetails UpsertPromotion(string basketId, Coupon coupon)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/baskets/{basketId}/applyPromotion",
                Method = RequestMethod.Patch,
                Body = new ApplyPromotionRequest {coupon = coupon}
            };
            var result = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(parameters);
            return GetUpsertPromotionResult(result, coupon, basketId);
        }

        /// <inheritdoc />
        public BasketDetails UpsertBasket(UpsertBasketRequest request)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = "v1/baskets",
                Method = RequestMethod.Patch,
                Body = request
            };
            var response = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(parameters);
            return response.DataOrException;
        }

        /// <inheritdoc />
        public BasketDetails RemoveReservation(string basketId, int reservationId)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/baskets/{basketId}/reservations/{reservationId}",
                Method = RequestMethod.Delete
            };
            var response = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(parameters);
            return response.DataOrException;
        }

        public BasketDetails ClearBasket(string basketId)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v1/baskets/{basketId}/clear",
                Method = RequestMethod.Patch
            };
            var response = Executor.ExecuteApiWithWrappedResponse<BasketDetails>(parameters);
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
