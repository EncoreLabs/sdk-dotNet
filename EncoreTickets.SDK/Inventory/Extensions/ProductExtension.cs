using System;
using EncoreTickets.SDK.Inventory.Models;
using EncoreTickets.SDK.Utilities.Mapping;

namespace EncoreTickets.SDK.Inventory.Extensions
{
    /// <summary>
    /// Extension for product model
    /// </summary>
    public static class ProductExtension
    {
        /// <summary>
        /// Returns the OnSale property as bool
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>On sale</returns>
        public static bool GetOnSale(this Product product) =>
            product.OnSale?.Trim().Equals("yes", StringComparison.InvariantCultureIgnoreCase) ?? false;

        /// <summary>
        /// Returns the Type property as the ProductType enum
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Product type</returns>
        public static ProductType GetProductType(this Product product) => product.Type.Map<string, ProductType>();
    }
}