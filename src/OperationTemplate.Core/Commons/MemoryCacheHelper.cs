using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoneCo.Buy4.OperationTemplate.Core.Commons
{
    /// <summary>
    /// Memory cache using objects of type TValue with key of type TKey.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MemoryCacheHelper<TKey, TValue> : IMemoryCacheHelper<TKey, TValue> where TValue : class
    {
        private readonly MemoryCacheOptions _options;
        private readonly DateTimeOffset _cacheTimeExpiration = DateTimeOffset.MaxValue;
        private readonly object _cacheLock = new object();
        private readonly Func<Dictionary<TKey, TValue>> _loadCacheData;
        private readonly Func<TKey, TValue> _getCacheData;
        private readonly Func<TKey, Task<TValue>> _getCacheDataAsync;
        private MemoryCache _cache;

        /// <summary>
        /// MemoryCacheHelper constructor.
        /// </summary>
        /// <param name="loadCacheData">Load all data function</param>
        /// <param name="getCacheData">Get one item data function</param>
        /// <param name="getCacheDataAsync">Get one item data function asynchronously</param>
        /// <param name="timeoutSeconds">cache timeout</param>
        /// <param name="options">Set default options</param>
        public MemoryCacheHelper(Func<Dictionary<TKey, TValue>> loadCacheData = null, Func<TKey, TValue> getCacheData = null, Func<TKey, Task<TValue>> getCacheDataAsync = null, int timeoutSeconds = -1, MemoryCacheOptions options = null)
        {
            this._options = new MemoryCacheOptions();

            if (options != null)
            {
                this._options = options;
            }

            this._cache = new MemoryCache(this._options);

            if (timeoutSeconds != -1)
            {
                this._cacheTimeExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(timeoutSeconds));
            }

            this._loadCacheData = loadCacheData;

            this._getCacheData = getCacheData;

            this._getCacheDataAsync = getCacheDataAsync;

            if (this._loadCacheData != null)
            {
                Load(this._loadCacheData);
            }
        }

        #region Main methods.

        /// <summary>
        /// Get specific item.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="getCacheData"></param>
        /// <returns></returns>
        public TValue GetItem(TKey cacheKey, Func<TKey, TValue> getCacheData = null)
        {
            return this._cache.GetOrCreate(cacheKey, (cacheEntry) => this.Factory(cacheEntry, getCacheData));
        }

        /// <summary>
        /// Get specific item asynchronously.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="getCacheDataAsync"></param>
        /// <returns></returns>
        public Task<TValue> GetItemAsync(TKey cacheKey, Func<TKey, Task<TValue>> getCacheDataAsync = null)
        {
            return this._cache.GetOrCreateAsync(cacheKey, (cacheEntry) => this.FactoryAsync(cacheEntry, getCacheDataAsync));
        }

        /// <summary>
        /// Store specific item in cache.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cachedData"></param>
        public void SetItem(TKey cacheKey, TValue cachedData)
        {
            this._cache.Set(cacheKey, cachedData, this._cacheTimeExpiration);
        }

        /// <summary>
        /// Remove item from cache.
        /// </summary>
        /// <param name="cacheKey"></param>
        public void RemoveItem(TKey cacheKey)
        {
            this._cache.Remove(cacheKey);
        }

        /// <summary>
        /// Load all items in cache using LoadData function.
        /// </summary>
        /// <param name="loadCacheData"></param>
        public void Load(Func<Dictionary<TKey, TValue>> loadCacheData)
        {
            Dictionary<TKey, TValue> cacheData = loadCacheData();
            lock (this._cacheLock)
            {
                this._cache.Dispose();
                this._cache = new MemoryCache(this._options);

                foreach (KeyValuePair<TKey, TValue> cacheitem in cacheData)
                {
                    this._cache.Set(cacheitem.Key, cacheitem.Value, this._cacheTimeExpiration);
                }
            }
        }

        /// <summary>
        /// Clear cache.
        /// </summary>
        public void ClearCache()
        {
            lock (this._cacheLock)
            {
                this._cache.Dispose();
                this._cache = new MemoryCache(this._options);
            }
        }

        /// <summary>
        /// Get cache total items.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this._cache.Count;
        }

        #endregion

        #region Private Methods

        private TValue Factory(ICacheEntry entry, Func<TKey, TValue> getCacheData = null)
        {
            return getCacheData != null ? getCacheData((TKey)entry.Key) : this._getCacheData?.Invoke((TKey)entry.Key);
        }

        /// <summary>
        /// Get data asynchronous factory.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="getCacheDataAsync"></param>
        /// <returns></returns>
        private Task<TValue> FactoryAsync(ICacheEntry entry, Func<TKey, Task<TValue>> getCacheDataAsync)
        {
            if (getCacheDataAsync != null)
            {
                return getCacheDataAsync((TKey)entry.Key);
            }

            return this._getCacheDataAsync?.Invoke((TKey)entry.Key);
        }

        #endregion
    }
}
