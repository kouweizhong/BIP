using System;
using System.Configuration;

namespace Wmb.Web {
    /// <summary>
    /// Represents a collection of image cache brokers in the configuration.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface"),
    ConfigurationCollection(typeof(ImageCacheBrokerElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public sealed class ImageCacheBrokerCollection : ConfigurationElementCollection {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCacheBrokerCollection"/> class.
        /// </summary>
        public ImageCacheBrokerCollection()
            : base() {
            ImageCacheBrokerElement defaultElement = (ImageCacheBrokerElement)this.CreateNewElement();
            defaultElement.Name = "WebImageCacheBroker";
            defaultElement.Class = "Wmb.Web.Caching.WebImageCacheBroker";

            BaseAdd(defaultElement);
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement() {
            return new ImageCacheBrokerElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element) {
            if (element == null) {
                throw new ArgumentNullException("element");
            }

            return ((ImageCacheBrokerElement)element).Name;
        }
    }
}
