using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wmb.Web.Caching;


namespace Wmb.TestWeb {
    /// <summary>
    /// <para>The WebMetadataCacheBroker caches the metadata of the images only. The first request of a new client the images will be generated.
    /// All following requests can be served by sending a not-modified header only.</para>
    /// </summary>
    public class WebMetadataCacheBroker : WebImageCacheBroker {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebMetadataCacheBroker"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="utcExpiry">The UTC expiry.</param>
        public WebMetadataCacheBroker(string key, DateTime utcExpiry)
            : base(key, utcExpiry) {
        }

        /// <summary>
        /// Gets a value indicating whether this ImageCacheBroker caches image bytes.
        /// </summary>
        /// <value><c>true</c> if [cache image bytes]; otherwise, <c>false</c>.</value>
        protected override bool CacheImageBytes {
            get {
                return false;
            }
        }
    }
}
