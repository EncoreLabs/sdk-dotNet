using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Inventory.Models;

namespace EncoreTickets.SDK.Inventory
{
    /// <summary>
    /// The interface of an inventory service
    /// </summary>
    public interface IInventoryServiceApi
    {
        /// <summary>
        /// Search for a product
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IList<Product> Search(string text);

        /// <summary>
        /// Get the performances for a given product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IList<Performance> GetPerformances(int productId, int quantity, DateTime from, DateTime to);

        /// <summary>
        /// Get the performances for a given product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IList<Performance> GetPerformances(string productId, int quantity, DateTime from, DateTime to);

        /// <summary>
        /// Get the seats for a performance
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="performance"></param>
        /// <returns></returns>
        Availability GetAvailability(string productId, int quantity, DateTime performance);

        /// <summary>
        /// Get the first and last bookable dates for a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        BookingRange GetBookingRange(string productId);
    }
}