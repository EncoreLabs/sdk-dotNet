using System;
using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.Utilities.Common
{
    /// <summary>
    /// Cache Handler class
    /// </summary>
    /// // TODO: APS-1935 - complete this class!
    public class CacheHandler
    {
        /// <summary>
        /// Default cache time
        /// </summary>
        private static readonly TimeSpan DefaultCacheTime = TimeSpan.FromHours(4);

        // todo replace
        private static readonly Dictionary<string, object> Cache = new Dictionary<string, object>();

        /// <summary>
        /// Initializes static members of the <see cref="CacheHandler"/> class.
        /// </summary>
        static CacheHandler()
        {
        }

        /// <summary>
        /// A method that should be executed if no value is found in the cache.
        /// </summary>
        /// <typeparam name="T">The type of object the cache method returns.</typeparam>
        /// <returns>The object to cache.</returns>
        public delegate T CacheMethod<out T>();

        /// <summary>
        /// Get an object from cache with the specified key
        /// </summary>
        /// <typeparam name="T">the type of object to get</typeparam>
        /// <param name="key">The key of the object.</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (!Cache.TryGetValue(key, out var cachedObject))
            {
                // fire the object not found in cache handler, which can actually "re-load" an object into the cache
                /*
                ObjectNotFoundInCacheEventArgs args = null;
                if (OnObjectNotFoundInCache(key, out args))
                {
                    cachedObject = AddInternal(key, args.Data, null, CacheItemPriority.Default);
                }
                */
            }

            return cachedObject != null ? (T) cachedObject : default;
        }

        /// <summary>
        /// Get an object from cache with the specified key. If the key is not found <paramref name="cacheMethod"/> is called and its value cached and returned.
        /// </summary>
        /// <typeparam name="T">the type of object to get</typeparam>
        /// <param name="cacheMethod">The method to execute if a value is not found in the cache.</param>
        /// <param name="key">The key of the object.</param>
        /// <returns></returns>
        public static T Get<T>(CacheMethod<T> cacheMethod, string key)
        {
            var cachedObject = Get<T>(key);
            if (cachedObject != null)
            {
                return cachedObject;
            }

            lock (string.Intern(key))
            {
                cachedObject = Get<T>(key);
                if (cachedObject != null)
                {
                    return cachedObject;
                }

                cachedObject = cacheMethod();
                if (cachedObject != null)
                {
                    Add(cachedObject, key, null);
                }
            }

            return cachedObject;
        }

        /// <summary>
        /// Determines if an object exists in cache
        /// </summary>
        /// <param name="key">The key of the object to check exists.</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return Cache.Keys.Contains(key);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="data">The data to add.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="timeOut">The time out.</param>
        /// <returns></returns>
        public static bool Add(object data, string cacheKey, TimeSpan? timeOut)
        {
            Cache.Add(cacheKey, data);
            return true;
        }

        /// <summary>
        /// Removes the keys from the cache.
        /// </summary>
        /// <param name="synchronize">If the cache removal should be synchronized across web servers.</param>
        /// <param name="keys">The keys.</param>
        public static void Delete(bool synchronize, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (key != null)
                {
                    Cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// Called when an object is removed from cache
        /// </summary>
        /// <param name="key">The key of the object to remove.</param>
        /// <param name="data">The data to remove.</param>
        private static void OnObjectRemovedFromCache(string key, object data)
        {
            /*// raise the event, if necessary
            if (ObjectRemovedFromCache != null)
            {
                ObjectRemovedFromCache(null, new CacheEventArgs(key, data));
            }*/
        }

        /// <summary>
        /// Called when an object is added to the cache
        /// </summary>
        /// <param name="key">The key of the object.</param>
        /// <param name="data">The data added.</param>
        private static void OnObjectAddedToCache(string key, object data)
        {
            /*
            if (ObjectAddedToCache != null)
            {
                ObjectAddedToCache(null, new CacheEventArgs(key, data));
            }*/
        }

    }
}
