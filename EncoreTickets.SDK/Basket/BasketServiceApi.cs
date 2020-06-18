using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Basket.Exceptions;
using EncoreTickets.SDK.Basket.Extensions;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Basket.Models.RequestModels;
using EncoreTickets.SDK.Utilities;
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
        /// Initialises a new instance of the <see cref="BasketServiceApi"/> class.
        /// </summary>
        /// <param name="context">The API context for requests.</param>
        public BasketServiceApi(ApiContext context)
            : base(context, BasketApiHost)
        {
        }

        /// <inheritdoc />
        public Models.Basket GetBasketDetails(string basketReference)
        {
            ThrowArgumentExceptionIfBasketReferenceNotSet(basketReference);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketReference}",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public IList<Delivery> GetBasketDeliveryOptions(string basketReference)
        {
            ThrowArgumentExceptionIfBasketReferenceNotSet(basketReference);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketReference}/deliveryOptions",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResultsInResponse<List<Delivery>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Models.Basket UpsertBasket(Models.Basket source, bool? hasFlexiTickets = null)
        {
            ThrowArgumentExceptionIfBasketDetailsNotSet(source);
            source.Reservations = source.Reservations?.Where(x => !x.IsFlexi() && x.Items != null).ToList();
            var request = source.Map<Models.Basket, UpsertBasketParameters>();
            if (hasFlexiTickets.HasValue)
            {
                request.HasFlexiTickets = hasFlexiTickets.Value;
            }

            return UpsertBasket(request);
        }

        /// <inheritdoc />
        public Models.Basket UpsertBasket(UpsertBasketParameters basketParameters)
        {
            ThrowArgumentExceptionIfBasketDetailsNotSet(basketParameters);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets",
                Method = RequestMethod.Patch,
                Body = basketParameters,
            };
            var response = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return response.DataOrException;
        }

        /// <inheritdoc />
        public Models.Basket UpsertPromotion(string basketReference, string couponName)
        {
            var coupon = new Coupon { Code = couponName };
            return UpsertPromotion(basketReference, coupon);
        }

        /// <inheritdoc />
        public Models.Basket UpsertPromotion(string basketReference, Coupon coupon)
        {
            ThrowArgumentExceptionIfBasketReferenceNotSet(basketReference);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketReference}/applyPromotion",
                Method = RequestMethod.Patch,
                Body = new ApplyPromotionRequest { Coupon = coupon },
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return GetUpsertPromotionResult(result, coupon, basketReference);
        }

        /// <inheritdoc />
        public Models.Basket ClearBasket(string basketReference)
        {
            ThrowArgumentExceptionIfBasketReferenceNotSet(basketReference);
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketReference}/clear",
                Method = RequestMethod.Patch,
            };
            var response = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return response.DataOrException;
        }

        /// <inheritdoc />
        public Models.Basket RemoveReservation(string basketReference, int reservationId)
        {
            return RemoveReservation(basketReference, reservationId.ToString());
        }

        /// <inheritdoc />
        public Models.Basket RemoveReservation(string basketReference, string reservationId)
        {
            ValidationHelper.ThrowArgumentExceptionIfNotSet(
                ("basket ID", basketReference),
                ("reservation ID", reservationId));
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/baskets/{basketReference}/reservations/{reservationId}",
                Method = RequestMethod.Delete,
            };
            var response = Executor.ExecuteApiWithWrappedResponse<Models.Basket>(parameters);
            return response.DataOrException;
        }

        /// <inheritdoc />
        public IList<Promotion> GetPromotions(PageRequest pageParameters = null)
        {
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/promotions",
                Method = RequestMethod.Get,
                Query = pageParameters,
            };
            var result = Executor.ExecuteApiWithWrappedResultsInResponse<List<Promotion>>(parameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public Promotion GetPromotionDetails(string promotionId)
        {
            ValidationHelper.ThrowArgumentExceptionIfNotSet(("promotion ID", promotionId));
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/promotions/{promotionId}",
                Method = RequestMethod.Get,
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Promotion>(parameters);
            return result.DataOrException;
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

        private void ThrowArgumentExceptionIfBasketReferenceNotSet(string basketReference)
        {
            ValidationHelper.ThrowArgumentExceptionIfNotSet(("basket ID", basketReference));
        }

        private void ThrowArgumentExceptionIfBasketDetailsNotSet(object basketDetails)
        {
            ValidationHelper.ThrowArgumentExceptionIfNotSet(("details for basket", basketDetails));
        }
    }
}
