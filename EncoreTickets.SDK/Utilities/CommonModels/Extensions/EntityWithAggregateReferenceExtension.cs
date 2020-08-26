using System;
using System.Globalization;

namespace EncoreTickets.SDK.Utilities.CommonModels.Extensions
{
    /// <summary>
    /// Extension for the entity of <see cref="IEntityWithAggregateReference"/> type.
    /// </summary>
    public static class EntityWithAggregateReferenceExtension
    {
        /// <summary>
        /// Checks if the aggregate reference can be correctly parsed and contains some data.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>True if the aggregate reference is valid.</returns>
        public static bool IsDataAvailableFromAggregateReference(this IEntityWithAggregateReference entity)
            => JwtHelper.CheckIfJwtStringContainsData(entity.AggregateReference);

        /// <summary>
        /// Returns the venue Id from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Venue Id.</returns>
        public static string GetVenueId(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "vi");

        /// <summary>
        /// Returns the venue country from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Venue country.</returns>
        public static string GetVenueCountry(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "vc");

        /// <summary>
        /// Returns the product ID from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Product ID.</returns>
        public static string GetProductId(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "pi");

        /// <summary>
        /// Returns the item reference from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Item reference.</returns>
        public static string GetItemReference(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "ii");

        /// <summary>
        /// Returns the item block from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Item block.</returns>
        public static string GetItemBlock(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "ib");

        /// <summary>
        /// Returns the item row from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Item row.</returns>
        public static string GetItemRow(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "ir");

        /// <summary>
        /// Returns the item seat number from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Item seat number.</returns>
        public static string GetItemSeatNumber(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "isn");

        /// <summary>
        /// Returns the item seat location description from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Item seat location description.</returns>
        public static string GetItemSeatLocationDescription(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "isld");

        /// <summary>
        /// Returns the item performance ID from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Item performance ID.</returns>
        public static string GetItemPerformanceId(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "ipi");

        /// <summary>
        /// Returns the item date from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Item date.</returns>
        public static DateTime? GetItemDate(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<DateTime?>(entity, "id");

        /// <summary>
        /// Returns the external supplier ID from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>External supplier ID.</returns>
        public static string GetExternalSupplierId(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "esi");

        /// <summary>
        /// Returns the external row ID from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>External row ID.</returns>
        public static string GetExternalRowId(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "eri");

        /// <summary>
        /// Returns the external seat index from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>External seat index.</returns>
        public static string GetExternalSeatIndex(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "esei");

        /// <summary>
        /// Returns the external block ID from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>External block ID.</returns>
        public static string GetExternalBlockId(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "ebi");

        /// <summary>
        /// Returns the external performance index from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>External performance index.</returns>
        public static string GetExternalPerformanceIndex(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "epi");

        /// <summary>
        /// Returns the external discount code type from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>External discount code type.</returns>
        public static string GetExternalDiscountCodeType(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "edct");

        /// <summary>
        /// Returns the partner ID from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Partner ID.</returns>
        public static string GetPartnerId(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "pai");

        /// <summary>
        /// Returns the cost price value from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Cost price value.</returns>
        public static int? GetCostPriceValue(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<int?>(entity, "cpv");

        /// <summary>
        /// Returns the cost price currency from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Cost price currency.</returns>
        public static string GetCostPriceCurrency(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "cpc");

        /// <summary>
        /// Returns the office sale price value from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Office sale price value.</returns>
        public static int? GetOfficeSalePriceValue(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<int?>(entity, "ospv");

        /// <summary>
        /// Returns the office sale price currency from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Office sale price currency.</returns>
        public static string GetOfficeSalePriceCurrency(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "ospc");

        /// <summary>
        /// Returns the office face value from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Office face value.</returns>
        public static int? GetOfficeFaceValue(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<int?>(entity, "ofvv");

        /// <summary>
        /// Returns the office face value currency from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Office face value currency.</returns>
        public static string GetOfficeFaceValueCurrency(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "ofvc");

        /// <summary>
        /// Returns the shopper sale price value from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Shopper sale price value.</returns>
        public static int? GetShopperSalePriceValue(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<int?>(entity, "sspv");

        /// <summary>
        /// Returns the cost price currency from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Shopper sale price currency.</returns>
        public static string GetShopperSalePriceCurrency(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "sspc");

        /// <summary>
        /// Returns the shopper face value from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Shopper face value.</returns>
        public static int? GetShopperFaceValue(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<int?>(entity, "sfvv");

        /// <summary>
        /// Returns the shopper face value currency from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Shopper face value currency.</returns>
        public static string GetShopperFaceValueCurrency(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "sfvc");

        /// <summary>
        /// Returns the office to shopper sale price fx rate from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>FX rate of office to shopper sale price.</returns>
        public static int? GetOfficeToShopperSalePriceFXRate(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<int?>(entity, "otsspfr");

        /// <summary>
        /// Returns the supplier to office sale price fx rate from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>FX rate of supplier to office sale price.</returns>
        public static int? GetSupplierToOfficeSalePriceFXRate(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<int?>(entity, "stospfr");

        /// <summary>
        /// Returns the inside commission from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Inside commission.</returns>
        public static int? GetInsideCommission(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<int?>(entity, "ic");

        /// <summary>
        /// Returns the price matrix code from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Price matrix code.</returns>
        public static string GetPriceMatrixCode(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference(entity, "pmc");

        /// <summary>
        /// Returns the rate expiry date from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Rate expiry date.</returns>
        public static DateTime? GetRateExpiryDate(this IEntityWithAggregateReference entity)
        {
            var dateAsStr = GetPropertyFromAggregateReference(entity, "red");
            return DateTime.TryParseExact(dateAsStr, "yyyyMMdd", null, DateTimeStyles.None, out var result)
                ? (DateTime?)result
                : null;
        }

        /// <summary>
        /// Returns the price rate version from the returned aggregate reference.
        /// </summary>
        /// <param name="entity">Entity with an aggregate reference.</param>
        /// <returns>Price rate version.</returns>
        public static int? GetPriceRateVersion(this IEntityWithAggregateReference entity) =>
            GetPropertyFromAggregateReference<int?>(entity, "prv");

        private static string GetPropertyFromAggregateReference(IEntityWithAggregateReference entity, string propertyName)
        {
            return JwtHelper.GetDecodedPropertyFromJwtString(entity.AggregateReference, propertyName);
        }

        private static T GetPropertyFromAggregateReference<T>(IEntityWithAggregateReference entity, string propertyName)
        {
            return JwtHelper.GetDecodedPropertyFromJwtString<T>(entity.AggregateReference, propertyName);
        }
    }
}
