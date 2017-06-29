using System;

namespace Wmb.Web.Caching {
    /// <summary>
    /// The WebImageCacheBroker uses the WebOutputCacheProvider for caching image data. 
    /// </summary>
    public class WebImageCacheBroker : ImageCacheBroker {
        private static readonly OutputCacheProvider outputCacheProvider = new WebOutputCacheProvider();
        /// <summary>
        /// Initializes a new instance of the <see cref="WebImageCacheBroker"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="utcExpiry">The UTC expiry.</param>
        public WebImageCacheBroker(string key, DateTime utcExpiry)
            : base(key, utcExpiry) {
        }

        /// <summary>
        /// Gets the output cache provider.
        /// </summary>
        /// <value>The output cache provider.</value>
        protected override OutputCacheProvider OutputCacheProvider {
            get {
                return outputCacheProvider;
            }
        }
    }
}
