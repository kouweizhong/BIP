using System;
using System.Web;
using System.Web.Caching;
using System.Diagnostics;

namespace Wmb.Web.Caching {
    /// <summary>
    /// The WebOutputCacheProvider class uses HttpContext.Current.Cache as the cache repository
    /// </summary>
    public class WebOutputCacheProvider : OutputCacheProvider {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebOutputCacheProvider"/> class.
        /// </summary>
        public WebOutputCacheProvider()
            : base() {
        }

        /// <summary>
        /// Inserts the specified entry into the output cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="utcExpiry">The UTC expiry.</param>
        /// <returns>Any existing object if it exists</returns>
        public override object Add(string key, object entry, DateTime utcExpiry) {
            Trace.TraceInformation("WebOutputCacheProvider: Adding item: '{0}'", key);
            return HttpContext.Current.Cache.Add(key, entry, null, utcExpiry, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        /// <summary>
        /// Returns a reference to the specified entry in the output cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A reference to the specified entry</returns>
        public override object Get(string key) {
            Trace.TraceInformation("WebOutputCacheProvider: Getting item: '{0}'", key);
            return HttpContext.Current.Cache.Get(key);
        }

        /// <summary>
        /// Removes the specified entry from the output cache.
        /// </summary>
        /// <param name="key">The key.</param>
        public override void Remove(string key) {
            Trace.TraceInformation("WebOutputCacheProvider: Removing item: '{0}'", key);
            HttpContext.Current.Cache.Remove(key);
        }

        /// <summary>
        /// Inserts the specified entry into the output cache, overwriting the entry if it is already cached.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="utcExpiry">The UTC expiry.</param>
        public override void Set(string key, object entry, DateTime utcExpiry) {
            Trace.TraceInformation("WebOutputCacheProvider: Setting item: '{0}'", key);
            HttpContext.Current.Cache.Insert(key, entry, null, utcExpiry, Cache.NoSlidingExpiration, null);
        }
    }
}
