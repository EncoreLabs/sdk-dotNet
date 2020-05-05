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
        /// Search for products by a keyword.
        /// GET /api/{VERSION}/search
        /// </summary>
        /// <param name="text">Search query</param>
        /// <returns>Products that contain a search query in their names</returns>
        IList<Product> SearchProducts(string text);

        /// <summary>
        /// Get the first and last bookable dates for a product.
        /// GET /api​/{VERSION}​/products​/{productId}​/availability-range
        /// </summary>
        /// <param name="productId">ID product</param>
        /// <returns>The first and last bookable dates for a product</returns>
        AvailabilityRange GetAvailabilityRange(int productId);

        /// <summary>
        /// Get the first and last bookable dates for a product.
        /// GET /api​/{VERSION}​/products​/{productId}​/availability-range
        /// </summary>
        /// <param name="productId">ID product, no longer than 50 chars might contain numbers, letters and dashes</param>
        /// <returns>The first and last bookable dates for a product</returns>
        AvailabilityRange GetAvailabilityRange(string productId);

        /// <summary>
        /// Get the performances for a given product.
        /// </summary>
        /// <param name="productId">ID product</param>
        /// <param name="quantity">quantity of seats needed</param>
        /// <param name="from">start date</param>
        /// <param name="to">to date</param>
        /// <returns>Array of performance availabilities</returns>
        IList<Performance> GetPerformances(int productId, int quantity, DateTime from, DateTime to);

        /// <summary>
        /// Get the performances for a given product.
        /// </summary>
        /// <param name="productId">ID product, no longer than 50 chars might contain numbers, letters and dashes</param>
        /// <param name="quantity">quantity of seats needed</param>
        /// <param name="fromDate">start date</param>
        /// <param name="toDate">to date</param>
        /// <returns>Array of performance availabilities</returns>
        IList<Performance> GetPerformances(string productId, int quantity, DateTime fromDate, DateTime toDate);
        
        /// <summary>
        /// Get the seats for a performance
        /// </summary>
        /// <param name="productId">ID product</param>
        /// <param name="quantity">quantity of seats needed</param>
        /// <param name="performance">performance time</param>
        /// <returns>Array of availability seat</returns>
        Availability GetAvailability(int productId, int quantity, DateTime? performance = null);

        /// <summary>
        /// Get the seats for a performance
        /// </summary>
        /// <param name="productId">ID product</param>
        /// <param name="quantity">quantity of seats needed</param>
        /// <param name="performance">performance time</param>
        /// <returns>Array of availability seat</returns>
        Availability GetAvailability(string productId, int quantity, DateTime? performance = null);

        /// <summary>
        /// Get the seats for a performance
        /// </summary>
        /// <param name="productId">ID product, no longer than 50 chars might contain numbers, letters and dashes</param>
        /// <param name="quantity">quantity of seats needed</param>
        /// <param name="date">performance date: if nothing is sent, current date will be used</param>
        /// <param name="time">performance time: if nothing is sent, current time will be used</param>
        /// <returns>Array of availability seat</returns>
        Availability GetAvailability(string productId, int quantity, DateTime? date, DateTime? time);
    }
}