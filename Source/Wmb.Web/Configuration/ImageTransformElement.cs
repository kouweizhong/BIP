using System.Configuration;

namespace Wmb.Web {
    /// <summary>
    /// The image transform element represents a reference to an image transform class.
    /// </summary>
    public sealed class ImageTransformElement : ConfigurationElement {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTransformElement"/> class.
        /// </summary>
        public ImageTransformElement() : base() { }

        /// <summary>
        /// Gets or sets the name to refer to the image transform class.
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
