using System;

namespace EncoreTickets.SDK.Utilities.Cache
{
    public interface ICache
    {
        /// <summary>
        /// Gets an object from cache with the specified key. If the key is not found <paramref name="factory"/> is called and its result is cached and returned.
        /// </summary>
        /// <typeparam name="T">the type of object to get</typeparam>
        /// <param name="factory">The method to execute if a value is not found in the cache.</param>
        /// <param name="key">The key of the object.</param>
        /// <param name="lifeSpan">The life span of the added cached object.</param>
        /// <returns></returns>
        T AddOrGetExisting<T>(string key, Func<T> factory, TimeSpan? lifeSpan);

        /// <summary>
        /// Adds the specified key to the cache. Overwrites existing entry if it already exists.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="factory">The method to execute to create a cache entry.</param>
        /// <param name="lifeSpan">The life span of the entry.</param>
        void Set<T>(string key, Func<T> factory, TimeSpan? lifeSpan);

        /// <summary>
        /// Gets an object from cache with the specified key.
        /// </summary>
        /// <typeparam name="T">the type of object to get</typeparam>
        /// <param name="key">The key of the object.</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Removes the key from the cache.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        bool Remove(string key);

        /// <summary>
        /// Determines if an object exists in cache
        /// </summary>
        /// <param name="key">The key of the object to check exists.</param>
        /// <returns></returns>
        bool Contains(string key);
    }
}
