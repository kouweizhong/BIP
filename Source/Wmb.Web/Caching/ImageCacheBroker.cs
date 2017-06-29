using System;

namespace Wmb.Web.Caching {
    /// <summary>
    /// The ImageCacheBroker class handles the caching and retrieval of images and their metadata from and to the given OutputCacheProvider
    /// </summary>
    public abstract class ImageCacheBroker {
        private string metaDataKey;
        private string imageBytesKey;
        DateTime utcExpiry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCacheBroker"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="utcExpiry">The UTC expiry.</param>
        protected ImageCacheBroker(string key, DateTime utcExpiry) {
            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException("key");
            }

            this.metaDataKey =   key;
            this.imageBytesKey = key + "_imagebytes";

            this.utcExpiry = utcExpiry;
        }


        /// <summary>
        /// Gets the output cache provider.
        /// </summary>
        /// <value>The output cache provider.</value>
        protected abstract OutputCacheProvider OutputCacheProvider { get; }

        /// <summary>
        /// Gets a value indicating whether this ImageCacheBroker caches image metadata.
        /// </summary>
        /// <value><c>true</c> if [cache image metadata]; otherwise, <c>false</c>.</value>
        protected virtual bool CacheImageMetadata {
            get {
                return true;
            }
        }

        /// <summary>
        /// Gets the image metadata.
        /// </summary>
        /// <returns>
        /// The image metadata that's currently in cache.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public virtual ImageMetadata GetImageMetadata() {
            if (CacheImageMetadata) {
                return this.OutputCacheProvider.Get(this.metaDataKey) as ImageMetadata;
            }

            return null;
        }

        /// <summary>
        /// Adds the image metadata to the cache.
        /// </summary>
        /// <param name="imageMetadata">The image metadata that is to be added to the cache.</param>
        public virtual void AddImageMetadata(ImageMetadata imageMetadata) {
            if (CacheImageMetadata) {
                this.OutputCacheProvider.Add(this.metaDataKey, imageMetadata, utcExpiry);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this ImageCacheBroker caches image bytes.
        /// </summary>
        /// <value><c>true</c> if [cache image bytes]; otherwise, <c>false</c>.</value>
        protected virtual bool CacheImageBytes {
            get {
                return true;
            }
        }

        /// <summary>
        /// Gets the image bytes.
        /// </summary>
        /// <returns>
        /// The image bytes that are currently in cache.
        /// </returns>
        public virtual byte[] GetImageBytes() {
            if (CacheImageBytes) {
                return this.OutputCacheProvider.Get(this.imageBytesKey) as byte[];
            }

            return null;
        }

        /// <summary>
        /// Adds the image bytes to the cache.
        /// </summary>
        /// <param name="value">The image bytes that are to be added to the cache.</param>
        public virtual void AddImageBytes(byte[] value) {
            if (CacheImageBytes) {
                this.OutputCacheProvider.Add(this.imageBytesKey, value, utcExpiry);
            }
        }
    }
}
