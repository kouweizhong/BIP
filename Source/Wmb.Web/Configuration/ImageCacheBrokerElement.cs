using System.Configuration;

namespace Wmb.Web {
    /// <summary>
    /// The image cache broker element represents a reference to an image cache broker class.
    /// </summary>
    public sealed class ImageCacheBrokerElement : ConfigurationElement {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCacheBrokerElement"/> class.
        /// </summary>
        public ImageCacheBrokerElement() : base() { }

        /// <summary>
        /// Gets or sets the name to refer to the image cache broker class.
        /// </summary>
        /// <value>The name.</value>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the fully qualified domain name of the class.
        /// </summary>
        /// <value>The class.</value>
        [ConfigurationProperty("class", IsRequired = true)]
        public string Class {
            get { return this["class"] as string; }
            set { this["class"] = value; }
        }
    }
}
