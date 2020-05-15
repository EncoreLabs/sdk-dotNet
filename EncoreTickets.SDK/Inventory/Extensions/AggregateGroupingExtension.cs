using System;
using System.Globalization;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Utilities.Serializers;

namespace EncoreTickets.SDK.Inventory.Extensions
{
    /// <summary>
    /// Extension for the <see cref="AggregateGrouping"/>
    /// </summary>
    public static class AggregateGroupingExtension
    {
        /// <summary>
        /// Returns the venue Id to which the grouping belongs from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Venue Id</returns>
        public static string GetVenueId(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "vi");

        /// <summary>
        /// Returns the venue country to which the grouping belongs from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Venue country</returns>
        public static string GetVenueCountry(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "vc");

        /// <summary>
        /// Returns the product ID to which the grouping belongs from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Product ID</returns>
        public static string GetProductId(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "pi");

        /// <summary>
        /// Returns the item reference of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Item reference</returns>
        public static string GetItemReference(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "ii");

        /// <summary>
        /// Returns the item block of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Item block</returns>
        public static string GetItemBlock(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "ib");

        /// <summary>
        /// Returns the item row of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Item row</returns>
        public static string GetItemRow(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "ir");

        /// <summary>
        /// Returns the item seat number of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Item seat number</returns>
        public static string GetItemSeatNumber(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "isn");

        /// <summary>
        /// Returns the item seat location description of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Item seat location description</returns>
        public static string GetItemSeatLocationDescription(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "isld");

        /// <summary>
        /// Returns the item performance ID of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Item performance ID</returns>
        public static string GetItemPerformanceId(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "ipi");

        /// <summary>
        /// Returns the item date of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Item date</returns>
        public static DateTime? GetItemDate(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<DateTime?>(grouping, "id");

        /// <summary>
        /// Returns the external supplier ID of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>External supplier ID</returns>
        public static string GetExternalSupplierId(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "esi");

        /// <summary>
        /// Returns the external row ID of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>External row ID</returns>
        public static string GetExternalRowId(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "eri");

        /// <summary>
        /// Returns the external seat index of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>External seat index</returns>
        public static string GetExternalSeatIndex(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "esei");

        /// <summary>
        /// Returns the external block ID of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>External block ID</returns>
        public static string GetExternalBlockId(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "ebi");

        /// <summary>
        /// Returns the external performance index of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>External performance index</returns>
        public static string GetExternalPerformanceIndex(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "epi");

        /// <summary>
        /// Returns the external discount code type of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>External discount code type</returns>
        public static string GetExternalDiscountCodeType(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "edct");

        /// <summary>
        /// Returns the partner ID of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Partner ID</returns>
        public static string GetPartnerId(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "pai");

        /// <summary>
        /// Returns the cost price value of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Cost price value</returns>
        public static int? GetCostPriceValue(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<int?>(grouping, "cpv");

        /// <summary>
        /// Returns the cost price currency of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Cost price currency</returns>
        public static string GetCostPriceCurrency(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "cpc");

        /// <summary>
        /// Returns the office sale price value of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Office sale price value</returns>
        public static int? GetOfficeSalePriceValue(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<int?>(grouping, "ospv");

        /// <summary>
        /// Returns the office sale price currency of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Office sale price currency</returns>
        public static string GetOfficeSalePriceCurrency(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "ospc");

        /// <summary>
        /// Returns the office face value of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Office face value</returns>
        public static int? GetOfficeFaceValue(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<int?>(grouping, "ofvv");

        /// <summary>
        /// Returns the office face value currency of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Office face value currency</returns>
        public static string GetOfficeFaceValueCurrency(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "ofvc");

        /// <summary>
        /// Returns the shopper sale price value of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Shopper sale price value</returns>
        public static int? GetShopperSalePriceValue(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<int?>(grouping, "sspv");

        /// <summary>
        /// Returns the cost price currency of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Shopper sale price currency</returns>
        public static string GetShopperSalePriceCurrency(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "sspc");

        /// <summary>
        /// Returns the shopper face value of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Shopper face value</returns>
        public static int? GetShopperFaceValue(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<int?>(grouping, "sfvv");

        /// <summary>
        /// Returns the shopper face value currency of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Shopper face value currency</returns>
        public static string GetShopperFaceValueCurrency(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "sfvc");

        /// <summary>
        /// Returns the office to shopper sale price fx rate of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>FX rate of office to shopper sale price</returns>
        public static int? GetOfficeToShopperSalePriceFXRate(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<int?>(grouping, "otsspfr");

        /// <summary>
        /// Returns the supplier to office sale price fx rate of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>FX rate of supplier to office sale price</returns>
        public static int? GetSupplierToOfficeSalePriceFXRate(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<int?>(grouping, "stospfr");

        /// <summary>
        /// Returns the inside commission of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Inside commission</returns>
        public static int? GetInsideCommission(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<int?>(grouping, "ic");

        /// <summary>
        /// Returns the price matrix code of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Price matrix code</returns>
        public static string GetPriceMatrixCode(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "pmc");

        /// <summary>
        /// Returns the rate expiry date of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Rate expiry date</returns>
        public static DateTime? GetRateExpiryDate(this AggregateGrouping grouping)
        {
            var dateAsStr = BaseGroupingExtensionHelper.GetPropertyFromAggregateReference(grouping, "red");
            return DateTime.TryParseExact(dateAsStr, "yyyyMMdd", null, DateTimeStyles.None, out var result)
                ? (DateTime?) result
                : null;
        }

        /// <summary>
        /// Returns the price rate version of the grouping from the returned aggregate reference.
        /// </summary>
        /// <param name="grouping">Aggregate grouping</param>
        /// <returns>Price rate version</returns>
        public static int? GetPriceRateVersion(this AggregateGrouping grouping) =>
            BaseGroupingExtensionHelper.GetPropertyFromAggregateReference<int?>(grouping, "prv");
    }
}
