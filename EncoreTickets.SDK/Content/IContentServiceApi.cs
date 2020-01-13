using System.Collections.Generic;
using EncoreTickets.SDK.Content.Models;

namespace EncoreTickets.SDK.Content
{
    /// <summary>
    /// The interface of a content service
    /// </summary>
    public interface IContentServiceApi
    {
        /// <summary>
        /// Get locations
        /// </summary>
        /// <returns></returns>
        IList<Location> GetLocations();

        /// <summary>
        /// Get the available products
        /// </summary>
        /// <returns></returns>
        IList<Product> GetProducts();

        /// <summary>
        /// Get the product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProductById(string id);
    }
}