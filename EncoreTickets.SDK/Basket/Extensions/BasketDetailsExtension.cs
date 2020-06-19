using System;
using System.Linq;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;

namespace EncoreTickets.SDK.Basket.Extensions
{
    public static class BasketDetailsExtension
    {
        /// <summary>
        /// Determines whether the basket is expired.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static bool IsExpired(this Models.Basket basket)
            => basket.ExpiredAt.UtcDateTime <= DateTime.UtcNow;

        /// <summary>
        /// Calculates the total number of items in all of the reservations in the basket.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static int ItemCount(this Models.Basket basket)
            => basket.Reservations?.Sum(r => r.Quantity) ?? 0;

        /// <summary>
        /// Determines whether the basket has any automatic promotions.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static bool HasAutomaticPromotion(this Models.Basket basket)
            => basket.Coupon == null && basket.HasPromotion();

        /// <summary>
        /// Determines whether the basket has any NON automatic promotions.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static bool HasNonAutomaticPromotion(this Models.Basket basket)
            => basket.Coupon != null && basket.HasPromotion();

        /// <summary>
        /// Determines whether the basket has any promotions.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static bool HasPromotion(this Models.Basket basket)
            => basket.AppliedPromotion != null;

        /// <summary>
        /// Gets total adjusted amount in office currency.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static Price GetBasketTotalInOfficeCurrency(this Models.Basket basket)
            => basket.GetTotalFromAllReservations(r => r.GetTotalAdjustedAmountInOfficeCurrency());

        /// <summary>
        /// Gets total adjusted amount in shopper currency.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static Price GetBasketTotalInShopperCurrency(this Models.Basket basket)
            => basket.GetTotalFromAllReservations(r => r.GetTotalAdjustedAmountInShopperCurrency());

        /// <summary>
        /// Gets total adjustment amount in office currency.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static Price GetTotalDiscountInOfficeCurrency(this Models.Basket basket)
            => basket.GetTotalFromAllReservations(r => r.GetTotalAdjustmentAmountInOfficeCurrency());

        /// <summary>
        /// Gets total adjustment amount in shopper currency.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static Price GetTotalDiscountInShopperCurrency(this Models.Basket basket)
            => basket.GetTotalFromAllReservations(r => r.GetTotalAdjustmentAmountInShopperCurrency());

        /// <summary>
        /// Gets total face value in office currency.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static Price GetTotalFaceValueInOfficeCurrency(this Models.Basket basket)
            => basket.GetTotalFromAllReservations(r => r.GetTotalFaceValueInOfficeCurrency());

        /// <summary>
        /// Gets total face value in shopper currency.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static Price GetTotalFaceValueInShopperCurrency(this Models.Basket basket)
            => basket.GetTotalFromAllReservations(r => r.GetTotalFaceValueInShopperCurrency());

        /// <summary>
        /// Gets total sale price without any adjustments in office currency.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static Price GetTotalSalePriceInOfficeCurrency(this Models.Basket basket)
            => basket.GetTotalFromAllReservations(r => r.GetTotalSalePriceInOfficeCurrency());

        /// <summary>
        /// Gets total sale price without any adjustments in office currency.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static Price GetTotalSalePriceInShopperCurrency(this Models.Basket basket)
            => basket.GetTotalFromAllReservations(r => r.GetTotalSalePriceInShopperCurrency());

        /// <summary>
        /// Gets basket total without delivery.
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public static Price GetBasketTotalWithoutDelivery(this Models.Basket basket)
        {
            var total = basket.GetBasketTotalInOfficeCurrency();
            return total != null && basket.Delivery?.Charge != null
                ? total.Subtract(basket.Delivery.Charge)
                : total;
        }

        private static Price GetTotalFromAllReservations(this Models.Basket basket, Func<Reservation, Price> priceFunc)
             => basket.Reservations?.Count > 0
                 ? basket.Reservations.Select(priceFunc).Aggregate((x, y) => x.Add(y))
                 : null;
    }
}
