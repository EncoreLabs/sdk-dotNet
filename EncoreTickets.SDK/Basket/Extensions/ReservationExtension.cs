using EncoreTickets.SDK.Basket.Models;
using EncoreTickets.SDK.Inventory.Models;

namespace EncoreTickets.SDK.Basket.Extensions
{
    public static class ReservationExtension
    {
        /// <summary>
        /// Gets total adjusted amount in office currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalAdjustedAmountInOfficeCurrency(this Reservation reservation) 
            => reservation.adjustedSalePriceInOfficeCurrency * reservation.quantity;

        /// <summary>
        /// Gets total adjusted amount in shopper currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalAdjustedAmountInShopperCurrency(this Reservation reservation) 
            => reservation.adjustedSalePriceInShopperCurrency * reservation.quantity;

        /// <summary>
        /// Gets total adjustment amount in office currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalAdjustmentAmountInOfficeCurrency(this Reservation reservation) 
            => reservation.adjustmentAmountInOfficeCurrency * reservation.quantity;

        /// <summary>
        /// Gets total adjustment amount in shopper currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalAdjustmentAmountInShopperCurrency(this Reservation reservation)
            => reservation.adjustmentAmountInShopperCurrency * reservation.quantity;

        /// <summary>
        /// Gets total sale price in office currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalSalePriceInOfficeCurrency(this Reservation reservation) 
            => reservation.salePriceInOfficeCurrency * reservation.quantity;

        /// <summary>
        /// Gets total sale price in shopper currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalSalePriceInShopperCurrency(this Reservation reservation) 
            => reservation.salePriceInShopperCurrency * reservation.quantity;

        /// <summary>
        /// Gets total face value in office currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalFaceValueInOfficeCurrency(this Reservation reservation) 
            => reservation.faceValueInOfficeCurrency * reservation.quantity;

        /// <summary>
        /// Gets total face value in shopper currency for the reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public static Price GetTotalFaceValueInShopperCurrency(this Reservation reservation) 
            => reservation.faceValueInShopperCurrency * reservation.quantity;
    }
}
