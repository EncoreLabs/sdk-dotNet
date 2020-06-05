using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Basket.Exceptions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using EncoreTickets.SDK.Basket.Models.ResponseModels;
using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Utilities.Mapping;

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

        /// <inheritdoc/>
        public override int? ApiVersion => 1;

        /// <summary>
        /// Default constructor for the Basket service.
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        public BasketServiceApi(ApiContext context) : base(context, BasketApiHost)
        {
        }

        /// <inheritdoc />
        public Models.Basket GetBasketDetails(string basketReference)
        {
            ThrowArgumentExceptionIfBasketReferenceIsIncorrect(basketReference);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketReference}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<Delivery> GetBasketDeliveryOptions(string basketReference)
        {
            ThrowArgumentExceptionIfBasketReferenceIsIncorrect(basketReference);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketReference}/deliveryOptions",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<List<Delivery>, GettingDeliveryOptionsResponse,
                GettingDeliveryOptionsResponseContent>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Models.Basket UpsertBasket(Models.Basket source)
        {
            ThrowArgumentExceptionIfBasketDetailsAreIncorrect(source);
            var request = source.Map<Models.Basket, UpsertBasketParameters>();
            return UpsertBasket(request);
        }

        /// <inheritdoc />
        public Models.Basket UpsertBasket(UpsertBasketParameters basketParameters)
        {
            ThrowArgumentExceptionIfBasketDetailsAreIncorrect(basketParameters);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets",
                Method = RequestMethod.Patch,
                Body = basketParameters
            };
            var response = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return response.DataOrException;
        }

        /// <inheritdoc />
        public Models.Basket ClearBasket(string basketId)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketId}/clear",
                Method = RequestMethod.Patch
            };
            var response = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return response.DataOrException;
        }

        /// <inheritdoc />
        public Models.Basket RemoveReservation(string basketId, int reservationId)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketId}/reservations/{reservationId}",
                Method = RequestMethod.Delete
            };
            var response = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return response.DataOrException;
        }

        /// <inheritdoc />
        public Promotion GetPromotionDetails(string promotionId)
        {
            if (string.IsNullOrEmpty(promotionId))
            {
                throw new ArgumentException("promotion ID must be set");
            }

            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/promotions/{promotionId}",
                Method = RequestMethod.Get
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Promotion>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Models.Basket UpsertPromotion(string basketId, Coupon coupon)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketId}/applyPromotion",
                Method = RequestMethod.Patch,
                Body = new ApplyPromotionRequest {Coupon = coupon}
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return GetUpsertPromotionResult(result, coupon, basketId);
        }

        private void ThrowArgumentExceptionIfBasketReferenceIsIncorrect(string basketReference)
        {
            if (string.IsNullOrWhiteSpace(basketReference))
            {
                throw new ArgumentException("basket ID must be set");
            }
        }

        private void ThrowArgumentExceptionIfBasketDetailsAreIncorrect(object basketDetails)
        {
            if (basketDetails == null)
            {
                throw new ArgumentException("details for basket must be set");
            }
        }

        private Models.Basket GetUpsertPromotionResult(ApiResult<Models.Basket> apiResult, Coupon coupon, string basketId)
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
