using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Commons
{
    /// <summary>
    /// Memory cache interface using objects of type TValue with key of type TKey.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IMemoryCacheHelper<TKey, TValue> where TValue : class
    {
        /// <summary>
        /// Clear cache.
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Get cache total items.
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Get specific item.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="getCacheData"></param>
        /// <returns></returns>
        TValue GetItem(TKey cacheKey, Func<TKey, TValue> getCacheData = null);

        /// <summary>
        /// Get specific item asynchronously.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="getCacheDataAsync"></param>
        /// <returns></returns>
        Task<TValue> GetItemAsync(TKey cacheKey, Func<TKey, Task<TValue>> getCacheDataAsync = null);

        /// <summary>
        /// Load all items in cache using LoadData function.
        /// </summary>
        /// <param name="loadCacheData"></param>
        void Load(Func<Dictionary<TKey, TValue>> loadCacheData);

        /// <summary>
        /// Remove item from cache.
        /// </summary>
        /// <param name="cacheKey"></param>
        void RemoveItem(TKey cacheKey);

        /// <summary>
        /// Store specific item in cache.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cachedData"></param>
        void SetItem(TKey cacheKey, TValue cachedData);
    }
}
