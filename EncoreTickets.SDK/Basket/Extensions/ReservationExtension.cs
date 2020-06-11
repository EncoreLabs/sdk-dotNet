using System;
using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Utilities.CommonModels.Constants;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;

namespace EncoreTickets.SDK.Basket.Extensions
{
    public static class ReservationExtension
    {
        /// <summary>
        /// Indicates whether the reservation is used for flexi tickets.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static bool IsFlexi(this Reservation reservation)
        {
            return (reservation?.ProductType?.Equals(ProductConstants.FlexiProductType,
                       StringComparison.InvariantCultureIgnoreCase) ?? false) &&
                   (reservation.ProductId?.Equals(ProductConstants.FlexiProductId,
                       StringComparison.InvariantCultureIgnoreCase) ?? false);
        }

        /// <summary>
        /// Gets total adjusted amount in office currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalAdjustedAmountInOfficeCurrency(this Reservation reservation) 
            => reservation.AdjustedSalePriceInOfficeCurrency.MultiplyByNumber(reservation.Quantity);

        /// <summary>
        /// Gets total adjusted amount in shopper currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalAdjustedAmountInShopperCurrency(this Reservation reservation) 
            => reservation.AdjustedSalePriceInShopperCurrency.MultiplyByNumber(reservation.Quantity);

        /// <summary>
        /// Gets total adjustment amount in office currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalAdjustmentAmountInOfficeCurrency(this Reservation reservation) 
            => reservation.AdjustmentAmountInOfficeCurrency.MultiplyByNumber(reservation.Quantity);

        /// <summary>
        /// Gets total adjustment amount in shopper currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalAdjustmentAmountInShopperCurrency(this Reservation reservation)
            => reservation.AdjustmentAmountInShopperCurrency.MultiplyByNumber(reservation.Quantity);

        /// <summary>
        /// Gets total sale price in office currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalSalePriceInOfficeCurrency(this Reservation reservation) 
            => reservation.SalePriceInOfficeCurrency.MultiplyByNumber(reservation.Quantity);

        /// <summary>
        /// Gets total sale price in shopper currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalSalePriceInShopperCurrency(this Reservation reservation) 
            => reservation.SalePriceInShopperCurrency.MultiplyByNumber(reservation.Quantity);

        /// <summary>
        /// Gets total face value in office currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalFaceValueInOfficeCurrency(this Reservation reservation) 
            => reservation.FaceValueInOfficeCurrency.MultiplyByNumber(reservation.Quantity);

        /// <summary>
        /// Gets total face value in shopper currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalFaceValueInShopperCurrency(this Reservation reservation) 
            => reservation.FaceValueInShopperCurrency.MultiplyByNumber(reservation.Quantity);
    }
}
