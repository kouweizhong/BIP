using System;

namespace Wmb.Web.Caching {
    /// <summary>
    /// Base class for output caching. Designed conform the new ASP.Net 4.0 System.Web.Caching.OutputCacheProvider. Can be swapped in a later version.
    /// </summary>
    public abstract class OutputCacheProvider {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputCacheProvider"/> class.
        /// </summary>
        protected OutputCacheProvider() {
        }

        /// <summary>
        /// Inserts the specified entry into the output cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="utcExpiry">The UTC expiry.</param>
        /// <returns>Any existing object if it exists</returns>
        public abstract object Add(string key, object entry, DateTime utcExpiry);

        /// <summary>
        /// Returns a reference to the specified entry in the output cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A reference to the specified entry</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
        public abstract object Get(string key);

        /// <summary>
        /// Removes the specified entry from the output cache.
        /// </summary>
        /// <param name="key">The key.</param>
        public abstract void Remove(string key);

        /// <summary>
        /// Inserts the specified entry into the output cache, overwriting the entry if it is already cached.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="utcExpiry">The UTC expiry.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Set")]
        public abstract void Set(string key, object entry, DateTime utcExpiry);
    }
}
